using SharpPcap;
using SharpPcap.LibPcap;

namespace IPK_sniffer;

public class Sniffer
{
    private static LibPcapLiveDevice? Device { get; set; }
        
    private static Arguments Options { get; set; }
    
    public Sniffer(Arguments options)
    {
        Options = options;
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
        
        int readTimeoutMilliseconds = 1000;
        Device.Open(DeviceModes.Promiscuous, readTimeoutMilliseconds);
        Device.Filter = FilterInit();
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
                    filter += $"src port {Options.Port}) or ";
                }
                else if (Options.DestOnly)
                {
                    filter += $"dst port {Options.Port}) or ";
                }
                else
                {
                    filter += $"port {Options.Port}) or ";
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
                    filter += $"src port {Options.Port}) or ";
                }
                else if (Options.DestOnly)
                {
                    filter += $"dst port {Options.Port}) or ";
                }
                else
                {
                    filter += $"port {Options.Port}) or ";
                }
            }
        }
        
        if(Options.Arp)
        {
            filter += "(arp) or ";
        }
        
        if(Options.Ndp)
        {
            filter += "(ndp) or ";
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
        
        if(Options.Mld)
        {
            filter += "(mld) or ";
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
        Console.WriteLine("Capture stopped");
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