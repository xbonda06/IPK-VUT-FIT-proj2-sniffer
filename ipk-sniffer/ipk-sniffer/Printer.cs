namespace IPK_sniffer;

/// <summary>
/// Helper class for printing packets
/// </summary>
public abstract class Printer
{
    /// <summary>
    /// Prints UDP, TCP header and data
    /// </summary>
    public static void PrintTransport(string timestamp, string frameLenth, string srcIp, string dstIp, 
        string srcPort, string dstPort, byte[] bytes)
    {
        Console.WriteLine($"timestamp: {timestamp}");
        Console.WriteLine($"src IP: {srcIp}");
        Console.WriteLine($"dst IP: {dstIp}");
        Console.WriteLine($"src port: {srcPort}");
        Console.WriteLine($"dst port: {dstPort}");
        Console.WriteLine($"frame length: {frameLenth}\n");
        PrintData(bytes);
    }
    
    /// <summary>
    /// Universal method for printing ICMP4, ICMP6, IGMP, MLD, NDP packets
    /// </summary>
    public static void PrintIcmpIgmp(string timestamp, string frameLenth, string srcMac, string dstMac, string srcIp, string dstIp, byte[] bytes)
    {
        string srcMacFormatted = ConvertMacAddress(srcMac);
        string dstMacFormatted = ConvertMacAddress(dstMac);
        Console.WriteLine($"timestamp: {timestamp}");
        Console.WriteLine($"src MAC: {srcMacFormatted}");
        Console.WriteLine($"dst MAC: {dstMacFormatted}");
        Console.WriteLine($"frame length: {frameLenth}");
        Console.WriteLine($"src IP: {srcIp}");
        Console.WriteLine($"dst IP: {dstIp}\n");
        PrintData(bytes);
    }
    
    /// <summary>
    /// Prints ARP packet`s header and data
    /// </summary>
    public static void PrintAtp(string timestamp, string srcMac, string dstMac, string frameLenth, byte[] bytes)
    {
        string srcMacFormatted = ConvertMacAddress(srcMac);
        string dstMacFormatted = ConvertMacAddress(dstMac);
        Console.WriteLine($"timestamp: {timestamp}");
        Console.WriteLine($"src MAC: {srcMacFormatted}");
        Console.WriteLine($"dst MAC: {dstMacFormatted}");
        Console.WriteLine($"frame length: {frameLenth}\n");
        PrintData(bytes);
    }

    private static void PrintData(byte[] bytes)
    {
        const int bytesPerLine = 16;
        for (int i = 0; i < bytes.Length; i += bytesPerLine)
        {
            // prints offset
            Console.Write($"0x{i:X4}: ");
            
            // prints hexadecimal bytes
            for (int j = i; j < Math.Min(i + bytesPerLine, bytes.Length); j++)
            {
                Console.Write($"{bytes[j]:X2} ");
            }
            
            // prints spaces if the line is not full
            if (bytes.Length - i < bytesPerLine)
            {
                Console.Write(new string(' ', (bytesPerLine - (bytes.Length - i)) * 3));
            }
            
            // prints ASCII representation of bytes
            Console.Write(" ");
            for (int j = i; j < Math.Min(i + bytesPerLine, bytes.Length); j++)
            {
                if(bytes[j] > 31 && bytes[j] < 127)
                {
                    Console.Write((char)bytes[j]);
                }
                else
                {
                    Console.Write("."); // non-printable characters
                }
            }
            
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    private static string ConvertMacAddress(string macAddress)
    {
        if (macAddress.Length != 12)
        {
            throw new ArgumentException("Invalid MAC address length. It should be 12 characters long.");
        }

        // Formats MAC address to xx:xx:xx:xx:xx:xx
        return string.Join(":", Enumerable.Range(0, 6).Select(i => macAddress.ToLower().Substring(i * 2, 2)));
    }
}