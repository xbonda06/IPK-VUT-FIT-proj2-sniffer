namespace IPK_sniffer;

public class Arguments
{
    public string? Interface { get; private set; }
    public int? Port { get; private set; }
    
    public bool SourceOnly { get; private set; }
    
    public bool DestOnly { get; private set; }
    public bool Tcp { get; private set; }
    public bool Udp { get; private set; }
    public bool Arp { get; private set; }
    public bool Ndp { get; private set; }
    public bool Icmp4 { get; private set; }
    public bool Icmp6 { get; private set; }
    public bool Igmp { get; private set; }
    public bool Mld { get; private set; }
    public int PacketCount { get; private set; }

    public Arguments(string[] args)
    {
        // Parse command line arguments
        for (int i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "-i":
                case "--interface":
                    if (i + 1 < args.Length){
                        Interface = args[++i];
                    }
                    break;
                case "-p":
                case "--port-source":
                case "--port-destination":
                    if (args[i] == "--port-source")
                    {
                        SourceOnly = true;
                    }
                    else if (args[i] == "--port-destination")
                    {
                        DestOnly = true;
                    }
                    
                    try 
                    {
                        Port = int.Parse(args[++i]);
                    } 
                    catch (FormatException)
                    {
                        Console.Error.WriteLine("Port must be a number");
                        Environment.Exit(1);
                    }
                    
                    if (Port < 0 || Port > 65535)
                    {
                        Console.Error.WriteLine("Port must be between 0 and 65535");
                        Environment.Exit(1);
                    }
                    break;
                case "-t":
                case "--tcp":
                    Tcp = true;
                    break;
                case "-u":
                case "--udp":
                    Udp = true;
                    break;
                case "--arp":
                    Arp = true;
                    break;
                case "--ndp":
                    Ndp = true;
                    break;
                case "--icmp4":
                    Icmp4 = true;
                    break;
                case "--icmp6":
                    Icmp6 = true;
                    break;
                case "--igmp":
                    Igmp = true;
                    break;
                case "--mld":
                    Mld = true;
                    break;
                case "-n":
                    if(i+1 < args.Length && !args[i+1].StartsWith("-"))
                    {
                        try
                        {
                            PacketCount = int.Parse(args[++i]);
                        }
                        catch (FormatException)
                        {
                            Console.Error.WriteLine("Packet count must be a number");
                            Environment.Exit(1);
                        }
                    }
                    else
                    {
                        PacketCount = 1;
                    }
                    break;
                
                
            }
        }
    }
}