using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using ConsoleApp1;

namespace MyServer;

public class MyServer {
    private UdpClient udpServer;
    private IPEndPoint remoteEndPoint;

    public MyServer(int serverPort) {
        udpServer = new UdpClient(serverPort);
        remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
    }

    public void Start() {
        Console.WriteLine("MyServer started. Listening for messages...");

        while (true) {
            byte[] data = udpServer.Receive(ref remoteEndPoint);
            string json = Encoding.UTF8.GetString(data);
            Message? message = JsonSerializer.Deserialize<Message>(json);

            Console.WriteLine("Received message:");
            Console.WriteLine($"Text: {message.Text}");
            Console.WriteLine($"Author: {message.Author}");
            Console.WriteLine($"Transmitter: {message.Transmitter}");
            Console.WriteLine($"Date: {message.Date}");
            Console.WriteLine();

            Console.WriteLine("Enter the response text:");
            string responseText = Console.ReadLine();

            Message response = new Message {
                Text = responseText,
                Author = "MyServer",
                Transmitter = message.Author,
                Date = DateTime.Now
            };

            SendMessage(response, remoteEndPoint);
            Console.WriteLine("Response sent successfully.");
        }
    }

    private void SendMessage(Message message, IPEndPoint endPoint) {
        string json = JsonSerializer.Serialize(message);
        byte[] data = Encoding.UTF8.GetBytes(json);
        udpServer.Send(data, data.Length, endPoint);
    }
}