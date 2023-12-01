using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ConsoleApp1;

namespace MyServer {
    public class MyServer {
        private UdpClient udpServer;
        private List<IPEndPoint> activeClients;

        public MyServer(int serverPort) {
            udpServer = new UdpClient(serverPort);
            activeClients = new List<IPEndPoint>();
        }

        public void Start() {
            Console.WriteLine("MyServer started. Listening for messages...");

            // Start a new thread to handle incoming client connections
            Thread connectionThread = new Thread(HandleClientConnections);
            connectionThread.Start();
        }

        private void SendMessage(Message message, IPEndPoint endPoint) {
            string json = JsonSerializer.Serialize(message);
            byte[] data = Encoding.UTF8.GetBytes(json);
            udpServer.Send(data, data.Length, endPoint);
        }

        private void HandleClientConnections() {
            while (true) {
                IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = udpServer.Receive(ref clientEndPoint);

                // Start a new task to handle the client connection
                Task.Run(() => HandleClientConnection(clientEndPoint, data));
            }
        }

        private void HandleClientConnection(IPEndPoint clientEndPoint, byte[] data) {
            string json = Encoding.UTF8.GetString(data);
            Message? message = JsonSerializer.Deserialize<Message>(json);

            // Process the received message from the client
            Console.WriteLine($"Received message from {clientEndPoint}:");
            Console.WriteLine($"Text: {message.Text}");
            Console.WriteLine($"Author: {message.Author}");
            Console.WriteLine($"Transmitter: {message.Transmitter}");
            Console.WriteLine($"Date: {message.Date}");
            Console.WriteLine();

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