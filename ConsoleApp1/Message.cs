using System.Text.Json;

namespace ConsoleApp1; 

public class Message {
    public string Text { get; set; }
    public string Author { get; set; }
    public string Transmitter { get; set; }
    public DateTime Date { get; set; }
    public int Id { get; set; }
    
    public string SerializeMessageToJson() {
        return JsonSerializer.Serialize(this);
    }
    public static Message? DeserializeMessageFromJson(string json) {
        return JsonSerializer.Deserialize<Message>(json);
    }
}