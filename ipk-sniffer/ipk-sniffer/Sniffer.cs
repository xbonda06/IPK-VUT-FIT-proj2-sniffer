using PacketDotNet;
using SharpPcap;
using SharpPcap.LibPcap;

namespace IPK_sniffer;

public class Sniffer
{
    private static LibPcapLiveDevice? Device { get; set; }
        
    private static Arguments Options { get; set; } = null!;

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
        
        var devices = LibPcapLiveDeviceList.Instance;
        Device = devices.FirstOrDefault(d => d.Interface.FriendlyName == Options.Interface);
        if (Device == null)
        {
            Console.WriteLine($"Device {Options.Interface} not found");
            return;
        }

        Console.CancelKeyPress += HandleCancelKey;
        Device.OnPacketArrival += new PacketArrivalEventHandler(device_OnPacketArrival);
        
        int readTimeoutMilliseconds = 1000;
        Device.Open(mode: DeviceModes.Promiscuous, read_timeout: readTimeoutMilliseconds);
        Device.Filter = FilterInit();
        Device.Capture();
    }

    public static void device_OnPacketArrival(object sender, PacketCapture e)
    {
        var currPacket = e.GetPacket();
        var parsedPacket = Packet.ParsePacket(currPacket.LinkLayerType, currPacket.Data);
        var time = currPacket.Timeval.Date.ToString("yyyy-MM-dd'T'HH:mm:ss.fffzzz");
        var len = currPacket.Data.Length;
        
        var arpPacket = parsedPacket.Extract<ArpPacket>();
        if (arpPacket != null)
        {
            Printer.PrintAtp(time, arpPacket.SenderHardwareAddress.ToString(), 
                arpPacket.TargetHardwareAddress.ToString(), len.ToString(), arpPacket.BytesSegment.Bytes);
        }

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
                    var ethernetPacket = parsedPacket.Extract<EthernetPacket>();
                    Printer.PrintIcmpIgmp(time, len.ToString(), ethernetPacket.SourceHardwareAddress.ToString(), 
                        ethernetPacket.DestinationHardwareAddress.ToString(),packet.SourceAddress.ToString(),
                        packet.DestinationAddress.ToString(), currPacket.Data);
                    break;
            }
        }
        
        
        
        _parsedPackets++;
        if (_parsedPackets == Options.PacketCount)
        {
            Device?.StopCapture();
            Device?.Close();
            Environment.Exit(0);
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