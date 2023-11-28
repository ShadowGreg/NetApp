using System.Text.Json;

namespace ConsoleApp1;

public class MessageMapper {
    public static string SerializeMessageToJson(Message? message) {
        return JsonSerializer.Serialize(message);
    }

    public static Message? DeserializeMessageFromJson(string json) {
        return JsonSerializer.Deserialize<Message>(json);
    }
}