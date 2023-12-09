using System.Net;
using Domain;
using NewServer.abstractions;

namespace NewServer.Infrastructure.Senders; 

public class ForOneEndPoint: BaseSender {
    private IPEndPoint _endPoint;
    private Server _server;
    
    /// <summary>
    /// Sent message for one user
    /// </summary>
    /// <param name="endPoint">User for send </param>
    public ForOneEndPoint(IPEndPoint endPoint,Server server)  {
        _endPoint = endPoint;
        _server = server;
    }

    public override Task SendMessage(Message message) {
        message.Transmitter = _endPoint.ToString();
        _server.SendMessage(message, _endPoint);
        return Task.CompletedTask;
    }
}