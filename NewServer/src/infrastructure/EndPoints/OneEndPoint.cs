using Domain;
using NewServer.abstractions;

namespace NewServer.Infrastructure.EndPoints; 

public class OneEndPoint:BaseEndPoint {
    public OneEndPoint(BaseSender baseSender): base(baseSender) { }
    public override Task SendMessage(Message message) {
        _baseSender.SendMessage(message);
        return Task.CompletedTask;
    }
}