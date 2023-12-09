using Domain;

namespace NewServer.abstractions;

public abstract class BaseEndPoint {
    protected BaseSender _baseSender;

    public BaseEndPoint(BaseSender baseSender) {
        _baseSender = baseSender;
    }

    public abstract Task SendMessage(Message message);
}