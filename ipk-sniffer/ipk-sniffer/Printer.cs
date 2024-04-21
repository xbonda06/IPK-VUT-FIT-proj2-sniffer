namespace IPK_sniffer;

public abstract class Printer
{
    public static void PrintAtpHeader(string timestamp, string srcMac, string dstMac, string frameLenth)
    {
        Console.WriteLine($"timestamp: {timestamp}");
        Console.WriteLine($"src MAC: {srcMac}");
        Console.WriteLine($"dst MAC: {dstMac}");
        Console.WriteLine($"frame length: {frameLenth}");
    }
}