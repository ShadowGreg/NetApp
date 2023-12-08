using NewServer.Infrastructure;

namespace NewServer; 

public static class Program {
    public static void Main(string[] args) {
        var clients = new ClientBase();
        var server = new Server(clients);
        
        server.Run();
    }
}