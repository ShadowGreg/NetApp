using Client;
using Domain;

namespace NewClient.Controllers;

public class Coordinator(Client newEntity) {
    private Message? message;
    private bool _flag = true;

    public void Run() {
        Console.WriteLine("Coordinator started. Listening for messages...");
        message = new Message();

        Task.Run(async () => await SendMessage());
        Task.Run(ReceiveMessages);

        while (_flag) {
            if (message.Text != null && message.Text.Contains("Exit")) {
                Console.WriteLine("Program execution stopped.");
                _flag = false;
            }

            Thread.Sleep(500);
        }

        newEntity.CloseConnection();
    }

    private async Task SendMessage() {
        while (_flag) {
            message = new Message {
                Text = GetMessageInput() ?? string.Empty,
                Author = $"Client number {Client.ID}",
                Transmitter = "Main server",
                Date = DateTime.Now
            };

            await newEntity.SendMessageAsync(message);
            Console.WriteLine("Message sent successfully.");
            Thread.Sleep(1000);
        }
    }

    private Task ReceiveMessages() {
        while (_flag) {
            message = newEntity.ReceiveMessage().Result;
            Console.WriteLine(Massages.ToString(message));
        }

        return Task.CompletedTask;
    }

    private static string? GetMessageInput() {
        Console.WriteLine("Enter a message: >");
        return Console.ReadLine();
    }
}