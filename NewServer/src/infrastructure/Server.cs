using System.Net;
using System.Net.Sockets;
using Domain;
using NewServer.Infrastructure.Service;
using NewServer.Service;
using NewServer.UIUsege;

namespace NewServer.Infrastructure;

public class Server(ClientBase clientBase) {
    private const string Name = "SERVER", Version = "1.0.0";
    private const int Port = 4444;
    private static readonly Lazy<UdpClient> UdpServer = new Lazy<UdpClient>(() => new UdpClient(Port));
    private List<Task> OnlineTasks = new();


    public UdpClient GetServer() => UdpServer.Value;
    public static string GetName() => Name;

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


        if (!CommandsController.IsCommands(message.Text)) return;
        string command = CommandsController.GetCommand(message.Text);
        switch (command) {
            case var _ when command == CommandsText.DeleteUser:
                await clientBase.DeleteUser(message.Author);
                break;
            case var _ when command == CommandsText.ServerHelp:
                Message helpMessage = GetHelpMessage(Name, message.Author);
                await SendMessage(helpMessage, clientEndPoint);
                break;
            case var _ when command == CommandsText.SendAll:
                // await clientBase.SentOperations.SendAll(message);
                break;
        }
        // await clientBase.SentOperations.SendActiveUsers(message);
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

    public Message GetHelpMessage(string messageAuthor, string messageTransmitter) {
        return new Message() {
            Text = "Commands: " + string.Join(", ",
                typeof(CommandsText).GetFields().Select(x => x.GetValue(null).ToString())),
            Author = messageAuthor,
            Transmitter = messageTransmitter,
            Date = DateTime.Now
        };
    }

    public Task SendMessage(Message message, IPEndPoint endPoint) {
        ConsoleLogger.Log(MessageToString.ToString(message) + " sent to " + endPoint);
        Task.Run(() => GetServer().Send(MessageMapper.ToBytes(message), endPoint)).GetAwaiter().GetResult();
        return Task.CompletedTask;
    }
}