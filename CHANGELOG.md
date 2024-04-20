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
### Run
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

commit 82c06bc8a86b03da65cf36d5c3f3a10e790d9f8e Author: xbonda06 <xbonda06@stud.fit.vutbr.cz> Date: Sat Apr 20 22:38:48 2024 +0200

```
feat: Add Argument class, command line arguments parsing
```

commit 4ede2f0a83e280b2aba9bc58724c4870354babb2 Author: xbonda06 <xbonda06@stud.fit.vutbr.cz> Date: Sat Apr 20 18:47:44 2024 +0200

```
feat: Add solution structure and Makefile
```

commit 1b9b7fc72e97fc545ab532b93615ab08217e956c Author: xbonda06 <xbonda06@stud.fit.vutbr.cz> Date: Sat Apr 20 18:16:22 2024 +0200

```
Initial commit
```
