using System.Net;
using Domain;

namespace NewServer.infrastructure.Service;

public class ClientSearcher {
    private Dictionary<string, IPEndPoint> _activeClients;

    public ClientSearcher(Dictionary<string, IPEndPoint> activeClients) {
        _activeClients = activeClients;
    }

    public IPEndPoint[] GetClient(string? text) {
        IPEndPoint[] clients = _activeClients.Values.ToArray();

        return (
            from
                client
                in
                clients
            let
                clientText = client.Address.ToString() + ":" + client.Port.ToString()
            where
                text.Contains(clientText)
            select client
        ).ToArray();
    }

    public bool IsClient(string? text) {
        return GetClient(text)?.Length > 0;
    }

    public IPEndPoint[] GetClient(Message message) {
        return GetClient(message.Text);
    }

    public bool IsClient(Message message) {
        return IsClient(message.Text);
    }
}