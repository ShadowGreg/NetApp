// See https://aka.ms/new-console-template for more information

using Client;

namespace Client;

public static class Program {
    public static void Main(string[] args) {
        const int serverPort = 3130;
        const string serverIp = "127.0.0.1";
        Client client = new(serverPort, serverIp);
        
        var coordinator = new Coordinator(client);
        
        coordinator.Run();
    }
}