// See https://aka.ms/new-console-template for more information

using Client;

namespace Client;

public class Program {
    public static void Main(string[] args) {
        int serverPort = 3130;
        Client _client = new(serverPort, "127.0.0.1");
        var coordinator = new Coordinator(_client);
        coordinator.Start();
    }
}