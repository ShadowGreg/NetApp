using System.Net;
using Domain;
using NewServer.abstractions;

namespace NewServer.Infrastructure.Senders;

public class FofAllEndPoints: BaseSender {
    private IPEndPoint _endPoint;
    private Server _server;

    /// <summary>
    /// Sent message for one user
    /// </summary>
    /// <param name="endPoint">User from which send </param>
    public FofAllEndPoints(IPEndPoint endPoint, Server server) {
        _endPoint = endPoint;
        _server = server;
    }

    public override Task SendMessage(Message message) {
        IPEndPoint[] endPoints = _server.GetClientBase().Values.ToArray();
        foreach (var client in endPoints) {
            if (client == _endPoint) continue;
            message.Transmitter = client.ToString();
            _server.SendMessage(message, _endPoint);
        }
        return Task.CompletedTask;
    }
}