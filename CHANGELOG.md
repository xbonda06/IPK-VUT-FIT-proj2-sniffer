# IPK 2024 Project 2
### Author: Andrii Bondarenko (xbonda06)
<hr>

## Description
Packet sniffer program for TCP, UDP, ARP, ICMP4/6, IGMP, NDP and MLD packets, built in .NET 8.0.

## Usage
### Build
```bash
$ make
```
After building the project, the directory `/build` will be created with the executable `ipk-sniffer` in the root. 

### Run
Go to the `/build` directory and run the executable with the following arguments:
```bash
./ipk-sniffer [-i interface | --interface interface] {-p|--port-source|--port-destination port [--tcp|-t] [--udp|-u]} [--arp] [--ndp] [--icmp4] [--icmp6] [--igmp] [--mld] {-n num}
```
`-i eth0` or `--interface` specifies the interface for packet sniffing. If not provided, a list of available interfaces is displayed.

`-p` sets the port number. If not specified, the entire range from 0 to 65535 is considered.

`--port-destination 23` extends port-based filtering to TCP/UDP packets based on destination port number.

`--port-source 23` extends port-based filtering to TCP/UDP packets based on source port number.

`--tcp` includes TCP packets in the filter.

`--udp` includes UDP packets in the filter.

`--arp` includes ARP packets in the filter.

`--icmp4` includes ICMPv4 packets in the filter.

`--icmp6` includes ICMPv6 packets in the filter.

`--igmp` includes IGMP packets in the filter.

`--mld` includes MLD packets in the filter.

`--ndp` includes NDP packets in the filter.

`-n` specifies the number of packets to capture, defaults to 1 if not specified.

### Clean
```bash
$ make clean
```

## Repository changelog

commit 7a2c33d9af62fcaefd90d3df2fa461dd2fc2c64b
Author: xbonda06 <mediabondar@gmail.com>
Date:   Mon Apr 22 21:23:57 2024 +0200

    fix!: Makefile target to build exec file in root dir

commit cd3bf7fdeca701f0bc2a72e1c6d0eeebfeb0c724
Author: xbonda06 <xbonda06@vutbr.cz>
Date:   Mon Apr 22 21:21:30 2024 +0200

    refactor: ipk-sniffer -> ipk_sniffer

commit d37c4024a6ef510e27d9a3ff8f8ebc653b7bbf18
Author: xbonda06 <mediabondar@gmail.com>
Date:   Mon Apr 22 20:20:36 2024 +0200

    fix: list available devices no FriendlyName ignore

commit 7e342a719ae0f4ccf716836a77d3cb338a41d77c
Author: xbonda06 <mediabondar@gmail.com>
Date:   Mon Apr 22 18:11:02 2024 +0200

    refactor: methods PrintData, ConvertMacAddress -> private

commit 0a1a29e0a1080c5b096a2f1bd431149ef247dfe1
Author: xbonda06 <mediabondar@gmail.com>
Date:   Mon Apr 22 17:20:53 2024 +0200

    refactor: add ParseArgs method

    CLI arguments parsing logic moved into the separate method, which is calling from the constructor

commit b421504c39502dda2ca5c57caad563d851cf5ba4
Author: xbonda06 <xbonda06@stud.fit.vutbr.cz>
Date:   Mon Apr 22 15:31:58 2024 +0200

    refactor: add Start Sniffer method

    The start sniffer logic moved into the public Start method, 
    which is called from instance of Sniffer in the Program Main method

commit 12fe9a4c4f97c003dd4cf03d279a07f797a2f58d
Author: xbonda06 <xbonda06@stud.fit.vutbr.cz>
Date:   Mon Apr 22 15:25:31 2024 +0200

    refactor: move Icmp, Igmp packet parse logic to HandleIcmpIgmp method

    Create method HandleIcmpIgmp, which is called from device_OnPacketArrival 
    in cases ProtocolType.Icmp, ProtocolType.IcmpV6, ProtocolType.Igmp. 
    This method realises Mld, Ndp, Icmp4, Icmp6, Igmp packets

commit ab2339781b1d277d4cde8cd835b204873b7e2cef (HEAD -> main, origin/main, origin/HEAD)
Author: xbonda06 <xbonda06@stud.fit.vutbr.cz>
Date:   Mon Apr 22 03:27:12 2024 +0200

    feat: Enhance packet handling for ICMP, ICMPv6, and IGMP protocols

    This commit introduces improved handling for ICMP, ICMPv6, and IGMP packets. 
    It adds support for ICMPv6 Multicast Listener Discovery (MLD) and 
    Neighbor Discovery Protocol (NDP) packets. 
    Additionally, it refactors the existing codebase to ensure clarity and 
    extensibility in handling these protocols.

commit 6cb6c984d50e65c5a779a120cc2ff4fd8bb761b4
Author: xbonda06 <xbonda06@stud.fit.vutbr.cz>
Date:   Mon Apr 22 02:18:33 2024 +0200

    feat: add Icmp, Icmp6, Igmp sniffing

commit b9628ec09996feea8e3de17294ca75b302c37c43
Author: xbonda06 <xbonda06@stud.fit.vutbr.cz>
Date:   Mon Apr 22 02:17:25 2024 +0200

    feat: add PrintIcmpIgmp to Printer class

commit cf638344337cc530f5d5d0bca7cae9182620e5bf
Author: xbonda06 <xbonda06@stud.fit.vutbr.cz>
Date:   Sun Apr 21 22:45:49 2024 +0200

    feat: add TCP and UDP packets execution

commit 068f93c20c06f9449bef931d0b1fc9de8d3ad105
Author: xbonda06 <xbonda06@stud.fit.vutbr.cz>
Date:   Sun Apr 21 21:31:08 2024 +0200

    feat: add ConvertMacAddress method to Printer


commit 33aab4176518b3f01b8c66779a8db7a511f0cb44
Author: xbonda06 <xbonda06@stud.fit.vutbr.cz>
Date:   Sun Apr 21 21:25:55 2024 +0200

    refactor: PrintArpHeader -> PrintArp

    PrintArp now prints data too

commit 1a17b520e1ff1b2d80e6cfe356e99214726ebfdb
Author: xbonda06 <xbonda06@stud.fit.vutbr.cz>
Date:   Sun Apr 21 21:20:07 2024 +0200

    feat: add PrintData

commit 5e13faa8fbf86f8848d0b1d3dd434c46ef933ff9
Author: xbonda06 <xbonda06@stud.fit.vutbr.cz>
Date:   Sun Apr 21 21:02:24 2024 +0200

    feat: add Printer class, ARP packet executing

commit b8cacc404579dcdab21b4e7d8a4c9b732bc66f91
Author: xbonda06 <xbonda06@stud.fit.vutbr.cz>
Date:   Sun Apr 21 21:01:39 2024 +0200

    fix: Argument -n parsing -> default value = 1

commit 5656df1579826377681fd96e53efa1768c87387a
Author: xbonda06 <xbonda06@stud.fit.vutbr.cz>
Date:   Sun Apr 21 20:32:42 2024 +0200

    feat: add OnPacketArrival handler

commit 6164d872dd3f347bbe979a3c3f2c620a424ae266 Author: xbonda06 <xbonda06@stud.fit.vutbr.cz> Date:   Sun Apr 21 20:31:54 2024 +0200


    fix: filter generation on mld and ndp


commit 99a1a40f073bf049f39d349161ebaca28bc6ca08 Author: xbonda06 <xbonda06@stud.fit.vutbr.cz> Date:   Sun Apr 21 02:53:08 2024 +0200


    feat: add CancelKey handle, Filter initialisation


commit fb8508b777b03125d1143ed9b10f20fedb634f55 Author: xbonda06 <xbonda06@stud.fit.vutbr.cz> Date:   Sun Apr 21 01:04:55 2024 +0200


    feat: add device search to sniffer constructor


commit 46824015715ac278c0756f867ed87d21cba0e685 Author: xbonda06 <xbonda06@stud.fit.vutbr.cz>Date:   Sun Apr 21 00:29:10 2024 +0200


    feat: add Sniffer class, ListAvailableDevices



commit 7f3e2a6440f7ffcfa481a30c3dc170943274b935 Author: xbonda06 <xbonda06@stud.fit.vutbr.cz> Date:   Sun Apr 21 00:28:26 2024 +0200


    fix: not provided -i value, wrong argument error


commit 644af58814e699a30b48d6d3788ece0f63cceb4a Author: xbonda06 <xbonda06@stud.fit.vutbr.cz> Date:   Sat Apr 20 23:09:35 2024 +0200


    fix: --port-source, --port-destination parsing


commit 82c06bc8a86b03da65cf36d5c3f3a10e790d9f8e Author: xbonda06 <xbonda06@stud.fit.vutbr.cz> Date: Sat Apr 20 22:38:48 2024 +0200


    feat: Add Argument class, command line arguments parsing


commit 4ede2f0a83e280b2aba9bc58724c4870354babb2 Author: xbonda06 <xbonda06@stud.fit.vutbr.cz> Date: Sat Apr 20 18:47:44 2024 +0200


    feat: Add solution structure and Makefile


commit 1b9b7fc72e97fc545ab532b93615ab08217e956c Author: xbonda06 <xbonda06@stud.fit.vutbr.cz> Date: Sat Apr 20 18:16:22 2024 +0200


    Initial commit
