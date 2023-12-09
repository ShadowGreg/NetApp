using System.Net;
using Domain;

namespace NewServer.abstractions; 

public abstract class BaseSender {
    public abstract Task SendMessage(Message message,Dictionary<string, IPEndPoint> activeClients);
}