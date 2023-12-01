using ConsoleApp1;

namespace Client;

public class Coordinator(Client newEntity) {
    public void Start() {
        Console.WriteLine("Coordinator started. Listening for messages...");

        while (true) {
            SendMessage();
            Thread.Sleep(1000);
            var message = newEntity.ReceiveMessage();
            Console.WriteLine(Massages.ToString(message));
        }
    }

    private void SendMessage() {
        Message message = new Message {
            Text = "Hello",
            Author = $"Client number {Client.ID}",
            Transmitter = "Main server",
            Date = DateTime.Now
        };

        newEntity.SendMessage(message);
        Console.WriteLine("Message sent successfully.");
    }
}