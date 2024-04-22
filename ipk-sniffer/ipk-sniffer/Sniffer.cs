using PacketDotNet;
using SharpPcap;
using SharpPcap.LibPcap;

namespace IPK_sniffer;

/// <summary>
/// Main class which provides packet sniffing
/// </summary>
public class Sniffer
{
    /// <summary>
    /// Currently used device we are listening on
    /// </summary>
    private static LibPcapLiveDevice? Device { get; set; }
        
    /// <summary>
    /// Parsed arguments from the command line
    /// </summary>
    private static Arguments Options { get; set; } = null!;

    /// <summary>
    /// Number of packets already parsed
    /// </summary>
    private static int _parsedPackets;
    
    public Sniffer(Arguments options)
    {
        Options = options;
        _parsedPackets = 0;
        if (Options.Interface == null)
        {
            ListAvailableDevices();
            return;
        }
        
        // Find device by interface name
        var devices = LibPcapLiveDeviceList.Instance;
        Device = devices.FirstOrDefault(d => d.Interface.FriendlyName == Options.Interface);
        if (Device == null)
        {
            Console.WriteLine($"Device {Options.Interface} not found");
        }
    }
    
    public void Start()
    {
        Console.CancelKeyPress += HandleCancelKey; // Handle Ctrl+C
        if (Device != null)
        {
            Device.OnPacketArrival += new PacketArrivalEventHandler(device_OnPacketArrival);

            int readTimeoutMilliseconds = 1000;
            Device.Open(mode: DeviceModes.Promiscuous, read_timeout: readTimeoutMilliseconds);
            Device.Filter = FilterInit();
            Device.Capture();
        }
    }

    private static void device_OnPacketArrival(object sender, PacketCapture e)
    {
        var currPacket = e.GetPacket();
        var parsedPacket = Packet.ParsePacket(currPacket.LinkLayerType, currPacket.Data);
        var time = currPacket.Timeval.Date.ToString("yyyy-MM-dd'T'HH:mm:ss.fffzzz");
        var len = currPacket.Data.Length;
        
        // Extract ARP packet information if available.
        var arpPacket = parsedPacket.Extract<ArpPacket>();
        if (arpPacket != null)
        {
            Printer.PrintAtp(time, arpPacket.SenderHardwareAddress.ToString(), 
                arpPacket.TargetHardwareAddress.ToString(), len.ToString(), arpPacket.BytesSegment.Bytes);
        }

        // Extract IP packet information if available.
        var packet = parsedPacket.Extract<IPPacket>();
        if (packet != null){
            switch (packet.Protocol)
            {
                case ProtocolType.Tcp:
                case ProtocolType.Udp:
                    var transportPacket = parsedPacket.Extract<TransportPacket>();
                    Printer.PrintTransport(time, len.ToString(), packet.SourceAddress.ToString(),
                        packet.DestinationAddress.ToString(), transportPacket.SourcePort.ToString(),
                        transportPacket.DestinationPort.ToString(), transportPacket.BytesSegment.Bytes);
                    break;
                case ProtocolType.Icmp:
                case ProtocolType.IcmpV6:
                case ProtocolType.Igmp:
                    var data = currPacket.Data;
                    HandleIcmpIgmp(packet, parsedPacket, time, len, data);
                    break;
            }
        }
        
        // Stop capturing packets if the required number of packets has been reached
        _parsedPackets++;
        if (_parsedPackets == Options.PacketCount)
        {
            Device?.StopCapture();
            Device?.Close();
            Environment.Exit(0);
        }
    }
    
    /// <summary>
    /// Universal help method for handling behavior of
    /// ICMP, ICMP6, IGMP, NDP, MLD packets
    /// </summary>
    private static void HandleIcmpIgmp(IPPacket packet, Packet parsedPacket, string time, int len, byte[] data)
    {
        var ethernetPacket = parsedPacket.Extract<EthernetPacket>();
        
        // Handle MLD packets if enabled
        if (Options.Mld)
        {
            var mldPacket = parsedPacket.Extract<IcmpV6Packet>();
            if (mldPacket != null && (mldPacket.Type == IcmpV6Type.MulticastListenerQuery ||
                                        mldPacket.Type == IcmpV6Type.MulticastListenerReport ||
                                        mldPacket.Type == IcmpV6Type.MulticastListenerDone))
            {
                data = mldPacket.BytesSegment.Bytes;
                var ipv6Packet = parsedPacket.Extract<IPv6Packet>();
                if (ipv6Packet != null && ethernetPacket != null)
                {
                    Printer.PrintIcmpIgmp(time, len.ToString(), ethernetPacket.SourceHardwareAddress.ToString(),
                        ethernetPacket.DestinationHardwareAddress.ToString(), ipv6Packet.SourceAddress.ToString(),
                        ipv6Packet.DestinationAddress.ToString(), data);
                }
            }
        }

        // Handle NDP packets if enabled
        if (Options.Ndp)
        {
            var ndpPacket = parsedPacket.Extract<NdpPacket>();
            if (ndpPacket != null)
            {
                data = ndpPacket.BytesSegment.Bytes;
                var ipv6Packet = parsedPacket.Extract<IPv6Packet>();
                if (ipv6Packet != null && ethernetPacket != null)
                {
                    Printer.PrintIcmpIgmp(time, len.ToString(), ethernetPacket.SourceHardwareAddress.ToString(),
                        ethernetPacket.DestinationHardwareAddress.ToString(), ipv6Packet.SourceAddress.ToString(),
                        ipv6Packet.DestinationAddress.ToString(), data);
                }
            }
        }

        // if package is not NDP or MLD
        // check if it is ICMP4, ICMP6 or IGMP
        if (ethernetPacket != null)
        {
            if (packet is IPv6Packet ipv6Packet)
            {
                if (ipv6Packet.PayloadPacket is IcmpV6Packet icmpV6Packet)
                {
                    // if package is ICMP6 echo
                    if (icmpV6Packet.Type == IcmpV6Type.EchoRequest || icmpV6Packet.Type == IcmpV6Type.EchoReply)
                    {
                        Printer.PrintIcmpIgmp(time, len.ToString(), ethernetPacket.SourceHardwareAddress.ToString(),
                            ethernetPacket.DestinationHardwareAddress.ToString(), packet.SourceAddress.ToString(),
                            packet.DestinationAddress.ToString(), icmpV6Packet.BytesSegment.Bytes);
                    }
                }
            }
            else
            {
                // if package is ICMP4 or IGMP
                Printer.PrintIcmpIgmp(time, len.ToString(), ethernetPacket.SourceHardwareAddress.ToString(),
                    ethernetPacket.DestinationHardwareAddress.ToString(), packet.SourceAddress.ToString(),
                    packet.DestinationAddress.ToString(), data);
            }
        }
    }

    private static string FilterInit()
    {
        string filter = "";
        if(Options.Tcp)
        {
            filter += "(ip or ip6 and tcp ";
            if (Options.Port == null)
            {
                filter += ") or ";
            }
            else
            {
                if(Options.SourceOnly)
                {
                    filter += $"and src port {Options.Port}) or ";
                }
                else if (Options.DestOnly)
                {
                    filter += $"and dst port {Options.Port}) or ";
                }
                else
                {
                    filter += $"and port {Options.Port}) or ";
                }
            }
        }
        
        if(Options.Udp)
        {
            filter += "(ip or ip6 and udp ";
            if (Options.Port == null)
            {
                filter += ") or ";
            }
            else
            {
                if(Options.SourceOnly)
                {
                    filter += $"and src port {Options.Port}) or ";
                }
                else if (Options.DestOnly)
                {
                    filter += $"and dst port {Options.Port}) or ";
                }
                else
                {
                    filter += $"and port {Options.Port}) or ";
                }
            }
        }
        
        if(Options.Arp)
        {
            filter += "(arp) or ";
        }
        
        if(Options.Icmp4)
        {
            filter += "(icmp) or ";
        }
        
        if(Options.Icmp6)
        {
            filter += "(icmp6) or ";
        }
        
        if(Options.Igmp)
        {
            filter += "(igmp) or ";
        }

        if (!Options.Icmp6)
        {
            if(Options.Ndp)
            {
                filter += "(icmp6) or ";
            } 
            else if (Options.Mld)
            {
                filter += "(icmp6) or ";
            }
        }
        
        if (filter.Length > 0)
        {
            filter = filter.Substring(0, filter.Length - 4);
        }

        return filter;
    }
    
    

    private static void HandleCancelKey(object? sender, ConsoleCancelEventArgs e)
    {
        Device?.StopCapture();
        Device?.Close();
        Environment.Exit(0);
    }
    
    private static void ListAvailableDevices()
    {
        var devices = LibPcapLiveDeviceList.Instance;
        if (devices.Count < 1)
        {
            Console.WriteLine("No devices found on this machine");
            return;
        }
        
        Console.WriteLine("Available devices:");
        for (int i = 0; i < devices.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {devices[i].Interface.FriendlyName}");
        }
    }
}