using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using SharpPcap;
using PacketDotNet;

namespace IPK_sniffer
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var arguments = new Arguments(args);
            var sniffer = new Sniffer(arguments);
            sniffer.Start();
        }
    }
}