namespace IPK_sniffer;

public abstract class Printer
{
    public static void PrintAtp(string timestamp, string srcMac, string dstMac, string frameLenth, byte[] bytes)
    {
        Console.WriteLine($"timestamp: {timestamp}");
        Console.WriteLine($"src MAC: {srcMac}");
        Console.WriteLine($"dst MAC: {dstMac}");
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
}