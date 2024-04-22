RUNTIME=linux-x64
CURRENT_DIR := $(shell dirname $(realpath $(lastword $(MAKEFILE_LIST))))

all: clean build

clean:
	dotnet clean ipk_sniffer/ipk-sniffer
	rm -rf build
	rm -f PacketDotNet.dll
	rm -f ipk-sniffer.dll
	rm -f ipk-sniffer
	rm -f ipk-sniffer.pdb
	rm -f SharpPcap.dll
	rm -f ipk-sniffer.deps.json
	rm -f ipk-sniffer.runtimeconfig.json

build:
	dotnet build ipk_sniffer/ipk-sniffer -c Release -o build -r $(RUNTIME)
	mv build/* $(CURRENT_DIR)/