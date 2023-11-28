using ConsoleApp1;

namespace Client;

public class Coordinator {
    private Client client;
    private int serverPort;

    public Coordinator(int serverPort) {
        this.serverPort = serverPort;
        client = new Client(serverPort, "127.0.0.1");
    }

    public void Start() {
        Console.WriteLine("Coordinator started. Listening for messages...");

        while (true) {
            SendMessage();
        }
    }

    private void SendMessage() {
        Console.WriteLine("Enter the message text:");
        string text = Console.ReadLine();

        Console.WriteLine("Enter the message author:");
        string author = Console.ReadLine();

        Console.WriteLine("Enter the message transmitter:");
        string transmitter = Console.ReadLine();

        Message message = new Message {
            Text = text,
            Author = author,
            Transmitter = transmitter,
            Date = DateTime.Now
        };

        client.SendMessage(message);
        Console.WriteLine("Message sent successfully.");

        ReceiveMessage();
    }

    private void ReceiveMessage() {
        Message message = client.ReceiveMessage();

        Console.WriteLine("Received message:");
        Console.WriteLine($"Text: {message.Text}");
        Console.WriteLine($"Author: {message.Author}");
        Console.WriteLine($"Transmitter: {message.Transmitter}");
        Console.WriteLine($"Date: {message.Date}");

        Thread.Sleep(100);
    }
}