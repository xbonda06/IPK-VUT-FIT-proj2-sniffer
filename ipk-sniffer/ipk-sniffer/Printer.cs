namespace IPK_sniffer;

public abstract class Printer
{
    public static void PrintTransport(string timestamp, string frameLenth, string srcIp, string dstIp, 
        string srcPort, string dstPort, byte[] bytes)
    {
        Console.WriteLine($"timestamp: {timestamp}");
        Console.WriteLine($"src IP: {srcIp}");
        Console.WriteLine($"dst IP: {dstIp}");
        Console.WriteLine($"src port: {srcPort}");
        Console.WriteLine($"dst port: {dstPort}");
        Console.WriteLine($"frame length: {frameLenth}");
        PrintData(bytes);
    }
    public static void PrintAtp(string timestamp, string srcMac, string dstMac, string frameLenth, byte[] bytes)
    {
        string srcMacFormatted = ConvertMacAddress(srcMac);
        string dstMacFormatted = ConvertMacAddress(dstMac);
        Console.WriteLine($"timestamp: {timestamp}");
        Console.WriteLine($"src MAC: {srcMacFormatted}");
        Console.WriteLine($"dst MAC: {dstMacFormatted}");
        Console.WriteLine($"frame length: {frameLenth}");
        PrintData(bytes);
    }

    public static void PrintData(byte[] bytes)
    {
        const int bytesPerLine = 16;
        for (int i = 0; i < bytes.Length; i += bytesPerLine)
        {
            Console.Write($"0x{i:X4}: ");
            
            for (int j = i; j < Math.Min(i + bytesPerLine, bytes.Length); j++)
            {
                Console.Write($"{bytes[j]:X2} ");
            }
            
            if (bytes.Length - i < bytesPerLine)
            {
                Console.Write(new string(' ', (bytesPerLine - (bytes.Length - i)) * 3));
            }
            
            Console.Write(" ");
            for (int j = i; j < Math.Min(i + bytesPerLine, bytes.Length); j++)
            {
                if(bytes[j] > 31 && bytes[j] < 127)
                {
                    Console.Write((char)bytes[j]);
                }
                else
                {
                    Console.Write(".");
                }
            }
            
            Console.WriteLine();
        }
        Console.WriteLine();
    }
    
    public static string ConvertMacAddress(string macAddress)
    {
        if (macAddress.Length != 12)
        {
            throw new ArgumentException("Invalid MAC address length. It should be 12 characters long.");
        }

        return string.Join(":", Enumerable.Range(0, 6).Select(i => macAddress.Substring(i * 2, 2)));
    }
}