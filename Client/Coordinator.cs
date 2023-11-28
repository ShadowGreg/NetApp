using ConsoleApp1;

namespace Client; 

public class Coordinator {
     private Client client;
        private int serverPort;

        public Coordinator(int serverPort)
        {
            this.serverPort = serverPort;
            client = new Client(serverPort,"127.0.0.1");
        }

        public void Start()
        {
            Console.WriteLine("Coordinator started. Listening for messages...");

            while (true)
            {
                Console.WriteLine("Enter '1' to send a message or '2' to receive a message:");
                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    SendMessage();
                }
                else if (choice == "2")
                {
                    ReceiveMessage();
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please try again.");
                }
            }
        }

        private void SendMessage()
        {
            Console.WriteLine("Enter the message text:");
            string text = Console.ReadLine();

            Console.WriteLine("Enter the message author:");
            string author = Console.ReadLine();

            Console.WriteLine("Enter the message transmitter:");
            string transmitter = Console.ReadLine();

            Message message = new Message
            {
                Text = text,
                Author = author,
                Transmitter = transmitter,
                Date = DateTime.Now
            };

            client.SendMessage(message);
            Console.WriteLine("Message sent successfully.");
        }

        private void ReceiveMessage()
        {
            Message message = client.ReceiveMessage();

            Console.WriteLine("Received message:");
            Console.WriteLine($"Text: {message.Text}");
            Console.WriteLine($"Author: {message.Author}");
            Console.WriteLine($"Transmitter: {message.Transmitter}");
            Console.WriteLine($"Date: {message.Date}");
        }
}