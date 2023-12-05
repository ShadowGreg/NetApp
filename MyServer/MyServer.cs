using System.Diagnostics.Tracing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using ConsoleApp1;

namespace MyServer {
    public class MyServer {
        private readonly UdpClient _udpServer;
        public List<IPEndPoint> ActiveClients { get; }
        private volatile bool _flag = true;
        private CancellationToken _cancellationToken;

        public MyServer(int serverPort) {
            _udpServer = new UdpClient(serverPort);
            ActiveClients = new List<IPEndPoint>();
        }

        public async Task Start(CancellationToken cancellationToken) {
            Console.WriteLine("MyServer started. Listening for messages...");
            await HandleClientConnectionsAsync();
            _cancellationToken = cancellationToken;
        }

        private async Task SendMessageAsync(Message message, IPEndPoint endPoint) {
            string json = JsonSerializer.Serialize(message);
            byte[] data = Encoding.UTF8.GetBytes(json);
            await _udpServer.SendAsync(data, data.Length, endPoint);
        }

        private async Task HandleClientConnectionsAsync() {
            while (_flag && !_cancellationToken.IsCancellationRequested) {
                var clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = _udpServer.Receive(ref clientEndPoint);

                // Start a new client connection
                await HandleClientConnectionAsync(clientEndPoint, data);
            }
        }

        private async Task HandleClientConnectionAsync(IPEndPoint clientEndPoint, byte[] data) {
            string json = Encoding.UTF8.GetString(data);
            var message = JsonSerializer.Deserialize<Message>(json);

            // Process the received message from the client
            Console.WriteLine($"Received message from {clientEndPoint}:");
            Console.WriteLine($"Text: {message.Text}");
            Console.WriteLine($"Author: {message.Author}");
            Console.WriteLine($"Transmitter: {message.Transmitter}");
            Console.WriteLine($"Date: {message.Date}");
            Console.WriteLine();

            if (message.Text.Contains("Exit")) {
                _flag = false;
                _cancellationToken.Register(() => throw new EventSourceException("Server stopped"));
            }


            // Send a response to the client
            Message response = new Message {
                Text = "Response from server. Your message has been received ",
                Author = "MyServer",
                Transmitter = message.Author,
                Date = DateTime.Now
            };
            await SendMessageAsync(response, clientEndPoint);
        }
    }
}