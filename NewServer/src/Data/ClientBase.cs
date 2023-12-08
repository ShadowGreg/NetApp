using System.Net;
using NewServer.Service;

namespace NewServer;

public class ClientBase {
    private readonly Dictionary<string, User> _activeClients = new();
    public Task Registration(string name, IPEndPoint endPoint) {
        try {
            _activeClients.TryAdd(name, new User(name, endPoint));
            ConsoleLogger.Log("Client add to base: " + endPoint.ToString());
        }
        catch (Exception e) {
            Console.WriteLine(e);
            throw;
        }

        return Task.CompletedTask;
    }

    public Task DeleteUser(string name) {
        try {
            _activeClients.Remove(name);
            ConsoleLogger.Log("Client remove from base: " + name);
        }
        catch (Exception e) {
            Console.WriteLine(e);
            throw;
        }

        return Task.CompletedTask;
    }

    public Dictionary<string, User> GetActiveClients() => _activeClients;
}