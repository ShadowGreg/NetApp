using System.Text;
using System.Text.Json;
using Domain;

public class MessagMap {
    public byte[] BytesFromMessage(string input, string address) {
        Message message = new Message {
            Text = input,
            Author = address,
            Transmitter = "Server",
            Date = DateTime.Now
        };
        byte[] data = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
        return data;
    }
}