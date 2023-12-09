using System.Net;
using System.Net.Sockets;
using System.Text;
using Domain;

public class UdpClient(string serverIp, int serverPort) {
    private readonly System.Net.Sockets.UdpClient _udpClient = new();
    private readonly IPEndPoint _remoteEndPoint = new(IPAddress.Parse((string)serverIp), serverPort);
    private IPEndPoint _localEndPoint = new(IPAddress.Parse("127.0.0.1"), 0);
    private volatile bool _flag = true;
    private readonly MessagMap _messagMap = new MessagMap();


    public void Run() {
        Console.WriteLine("UDP Client started. Listening for messages...");

        // Start a task to listen for incoming messages
        Task receiveTask = Task.Run(ReceiveMessages);

        // Start a task to wait for user input and send messages
        Task sendTask = Task.Run(SendMessages);

        // Wait for both tasks to finish
        Task.WaitAll(receiveTask, sendTask);

        // Close the UDP client
        _udpClient.Close();
    }

    private void ReceiveMessages() {
        try {
            _udpClient.Client.Bind(_localEndPoint);
            while (_flag) {
                byte[] data = _udpClient.Receive(ref _localEndPoint);
                string message = Encoding.UTF8.GetString(data);
                Console.WriteLine("Received message: " + message);
            }
        }
        catch (SocketException ex) {
            Console.WriteLine("SocketException: " + ex.Message);
        }
    }

    private void SendMessages() {
        try {
            while (_flag) {
                Console.WriteLine("Enter a message (or type 'EXIT' to quit): ");
                string? input = Console.ReadLine();

                if (input == "EXIT") {
                    input = "server -d";
                    byte[] message = _messagMap.BytesFromMessage(
                        input,
                        _localEndPoint.Address + ":" + _localEndPoint.Port);
                    _udpClient.Send(message, message.Length, _remoteEndPoint);
                    _flag = false;
                    break; // Exit the loop and stop sending messages
                }

                if (input != null) {
                    byte[] data = _messagMap.BytesFromMessage(
                        input,
                        _localEndPoint.Address + ":" + _localEndPoint.Port);
                    _udpClient.Send(data, data.Length, _remoteEndPoint);
                }

                Console.WriteLine("Message sent.");
            }
        }
        catch (SocketException ex) {
            Console.WriteLine("SocketException: " + ex.Message);
        }
    }
}