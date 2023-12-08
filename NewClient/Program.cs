// using NewClient.Controllers;
//
// namespace NewClient;
//
// public class Program {
//     public static void Main(string[] args) {
//         const int serverPort = 4444;
//         const string serverIp = "127.0.0.1";
//         Lazy<Controllers.Client> client = new Lazy<Controllers.Client>(() => new Controllers.Client( serverPort, serverIp));
//         
//         var coordinator = new Coordinator(client.Value);
//         
//         coordinator.Run();
//     }
// }

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using Domain;

public class UdpClientExample
{
    private UdpClient udpClient;
    private IPEndPoint remoteEndPoint;
    private IPEndPoint localEndPoint;

    public UdpClientExample(string serverIp, int serverPort)
    {
        udpClient = new UdpClient();
        remoteEndPoint = new IPEndPoint(IPAddress.Parse(serverIp), serverPort);
        localEndPoint = new IPEndPoint(IPAddress.Any, 0);
    }

    public void Run()
    {
        Console.WriteLine("UDP Client started. Listening for messages...");

        // Start a thread to listen for incoming messages
        Thread receiveThread = new Thread(ReceiveMessages);
        receiveThread.Start();

        // Start a thread to wait for user input and send messages
        Thread sendThread = new Thread(SendMessages);
        sendThread.Start();

        // Wait for both threads to finish
        receiveThread.Join();
        sendThread.Join();

        // Close the UDP client
        udpClient.Close();
    }

    private void ReceiveMessages()
    {
        try
        {
            udpClient.Client.Bind(localEndPoint);
            while (true)
            {
                byte[] data = udpClient.Receive(ref localEndPoint);
                string message = Encoding.UTF8.GetString(data);
                Console.WriteLine("Received message: " + message);
            }
        }
        catch (SocketException ex)
        {
            Console.WriteLine("SocketException: " + ex.Message);
        }
    }

    private void SendMessages()
    {
        try
        {
            while (true)
            {
                Console.WriteLine("Enter a message: ");
                string input = Console.ReadLine();
                Message message = new Message {
                    Text = input,
                    Author = localEndPoint.Address.ToString(),
                    Transmitter = "Server",
                    Date = DateTime.Now
                };
                byte[] data = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
                udpClient.Send(data, data.Length, remoteEndPoint);
                Console.WriteLine("Message sent.");
            }
        }
        catch (SocketException ex)
        {
            Console.WriteLine("SocketException: " + ex.Message);
        }
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        string serverIp = "127.0.0.1";
        int serverPort = 4444;

        UdpClientExample udpClientExample = new UdpClientExample(serverIp, serverPort);
        udpClientExample.Run();
    }
}