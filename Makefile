RUNTIME=linux-x64

all: clean build

clean:
	dotnet clean ipk-sniffer/ipk-sniffer
	rm -rf build

build:
	dotnet build ipk-sniffer/ipk-sniffer -c Release -o build -r $(RUNTIME)
