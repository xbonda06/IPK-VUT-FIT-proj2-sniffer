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