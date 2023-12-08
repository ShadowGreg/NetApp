using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Domain;

public class UdpClient(string serverIp, int serverPort) {
    private System.Net.Sockets.UdpClient udpClient = new();
    private IPEndPoint remoteEndPoint = new(IPAddress.Parse((string)serverIp), serverPort);
    private IPEndPoint localEndPoint = new(IPAddress.Any, 0);

    public void Run()
    {
        Console.WriteLine("UDP Client started. Listening for messages...");

        // Start a task to listen for incoming messages
        Task receiveTask = Task.Run(ReceiveMessages);

        // Start a task to wait for user input and send messages
        Task sendTask = Task.Run(SendMessages);

        // Wait for both tasks to finish
        Task.WaitAll(receiveTask, sendTask);

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