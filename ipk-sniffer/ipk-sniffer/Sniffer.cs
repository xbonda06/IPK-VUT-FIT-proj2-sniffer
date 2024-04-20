using SharpPcap.LibPcap;

namespace IPK_sniffer;

public static class Sniffer
{
    private static LibPcapLiveDevice Device { get; set; }
        
    private static Arguments Options { get; set; }
    
    public static void ListAvailableDevices()
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