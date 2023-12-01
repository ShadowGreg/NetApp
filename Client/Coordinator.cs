using ConsoleApp1;

namespace Client;

public class Coordinator(Client newEntity) {
    private Message? message = new Message();
    private bool _flag = true;
    public void Run() {
        Console.WriteLine("Coordinator started. Listening for messages...");

        while (_flag) {
            SendMessage();
            
            if (message.Text.Contains("Exit"))
            {
                Console.WriteLine("Program execution stopped.");
                _flag=false;
                
            }
            
            Thread.Sleep(1000);
            message = newEntity.ReceiveMessage();
            Console.WriteLine(Massages.ToString(message));
        }
        
        newEntity.CloseConnection();
    }

    private void SendMessage() {
        message = new Message {
            Text = GetMessageInput() ?? string.Empty,
            Author = $"Client number {Client.ID}",
            Transmitter = "Main server",
            Date = DateTime.Now
        };

        newEntity.SendMessage(message);
        Console.WriteLine("Message sent successfully.");
    }

    private static string? GetMessageInput() {
        Console.WriteLine("Enter a message: >");
        return Console.ReadLine();
    }
}