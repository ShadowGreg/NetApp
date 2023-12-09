using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using Domain;
using NewServer.Infrastructure.Senders;
using NewServer.infrastructure.Service;
using NewServer.Infrastructure.Service;
using NewServer.Service;
using NewServer.UIUsege;

namespace NewServer.Infrastructure;

public class Server(ClientBase clientBase) {
    private const string Name = "SERVER", Version = "1.0.0";
    private const int Port = 4444;
    private static readonly Lazy<UdpClient> UdpServer = new Lazy<UdpClient>(() => new UdpClient(Port));
    private List<Task> OnlineTasks = new();
    private ClientSearcher _clientSearcher = new ClientSearcher(clientBase.GetActiveClients()); //ClientSearcher
    private FofAllEndPoints _forAllEndPoints;


    public UdpClient GetServer() => UdpServer.Value;
    public static string GetName() => Name;

    public static string GetVersion() => Version;

    public Dictionary<string, IPEndPoint> GetClientBase() => clientBase.GetActiveClients();

    public void Run() {
        ConsoleLogger.Log("Server started... Listening...");
        while (true) {
            var clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
            if (clientEndPoint != null) {
                byte[] data = GetServer().Receive(ref clientEndPoint);
                OnlineTasks.Add(
                    Task.Run(() => HandleClientConnectionAsync(clientEndPoint, data))
                );
                ConsoleLogger.Log("Client connected: " + clientEndPoint.ToString());
            }
        }
    }

    private async Task HandleClientConnectionAsync(IPEndPoint clientEndPoint, byte[] data) {
        await clientBase.Registration(clientEndPoint.ToString(), clientEndPoint);

        var message = MessageMapper.ToMessage(data);
        ConsoleLogger.Log(MessageToString.ToString(message));

        SentConfirmation(clientEndPoint);

        if (message.Text != string.Empty) {
            if (CommandsController.IsCommands(message.Text)) {
                string command = CommandsController.GetCommand(message.Text) + " \n";
                switch (command) {
                    case CommandsText.DeleteUser:
                        await clientBase.DeleteUser(message.Author);
                        break;
                    case CommandsText.ServerHelp:
                    {
                        Message helpMessage = GetHelpMessage(Name, message.Author);
                        await SendMessage(helpMessage, clientEndPoint);
                        break;
                    }
                    case CommandsText.GetAllIpPoint:
                    {
                        Message allEndPoint = GetAllEndPoinsMSG(message.Author);
                        await SendMessage(allEndPoint, clientEndPoint);
                        break;
                    }
                }
            }
            else if (_clientSearcher.IsClient(message)) {
                IPEndPoint[] endPoints = _clientSearcher.GetClient(message);
                foreach (IPEndPoint endPoint in endPoints) {
                    await SendMessage(message, endPoint);
                }
            }
            else {
                List<IPEndPoint> endPoints = clientBase.GetActiveClients().Values.ToList();
                foreach (var point in endPoints) {
                    message.Transmitter = point.ToString();
                    if (point.ToString() != message.Author) {
                        await SendMessage(message, point);
                    }
                }
            }
        }
    }

    private Message GetAllEndPoinsMSG(string messageAuthor) {
        return new Message() {
            Author = Name,
            Text = string.Join(",\n", clientBase.GetActiveClients().Keys),
            Date = DateTime.Now,
            Transmitter = messageAuthor
        };
    }

    private void SentConfirmation(IPEndPoint clientEndPoint) {
        Message message = new Message() {
            Text = "Message received",
            Author = Name,
            Transmitter = clientEndPoint.ToString(),
            Date = DateTime.Now
        };
        Task.Delay(500);
        SendMessage(message, clientEndPoint);
    }

    public Task SendMessage(Message message, IPEndPoint endPoint) {
        ConsoleLogger.Log(MessageToString.ToString(message) + " sent to " + endPoint);
        Task.Run(() => GetServer().Send(MessageMapper.ToBytes(message), endPoint)).GetAwaiter().GetResult();
        return Task.CompletedTask;
    }

    public Message GetHelpMessage(string messageAuthor, string messageTransmitter) {
        return new Message() {
            Text = "Commands: " + string.Join(", ",
                typeof(CommandsText).GetFields().Select(x => x.GetValue(null).ToString())),
            Author = messageAuthor,
            Transmitter = messageTransmitter,
            Date = DateTime.Now
        };
    }
}