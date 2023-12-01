using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ConsoleApp1;

namespace MyServer {
    public class MyServer {
        private readonly UdpClient _udpServer;
        public List<IPEndPoint> ActiveClients { get; }
        private volatile bool _flag = true;

        public MyServer(int serverPort) {
            _udpServer = new UdpClient(serverPort);
            ActiveClients = new List<IPEndPoint>();
        }

        public void Start() {
            Console.WriteLine("MyServer started. Listening for messages...");

            // Start a new thread to handle incoming client connections
            var connectionThread = new Thread(HandleClientConnections);
            connectionThread.Start();
            connectionThread.Join();
        }

        private void SendMessage(Message message, IPEndPoint endPoint) {
            string json = JsonSerializer.Serialize(message);
            byte[] data = Encoding.UTF8.GetBytes(json);
            _udpServer.Send(data, data.Length, endPoint);
        }

        private void HandleClientConnections() {
            while (_flag) {
                var clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = _udpServer.Receive(ref clientEndPoint);

                // Start a new  client connection
                try {
                    var connectionThread = new Thread(() => HandleClientConnection(clientEndPoint, data));
                    connectionThread.Start();
                    connectionThread.Join();
                }
                catch (Exception e) {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        private void HandleClientConnection(IPEndPoint clientEndPoint, byte[] data) {
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
                throw new EventSourceException("Server stopped");
            }

            // Send a response to the client
            Message response = new Message {
                Text = "Response from server. Your message has been received ",
                Author = "MyServer",
                Transmitter = message.Author,
                Date = DateTime.Now
            };
            SendMessage(response, clientEndPoint);
        }
    }
}