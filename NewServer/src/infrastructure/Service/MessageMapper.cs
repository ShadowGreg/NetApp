using System.Text;
using System.Text.Json;
using Domain;

namespace NewServer.Service; 

public static class MessageMapper {
    public static Message ToMessage(byte[] data) {
        string json = Encoding.UTF8.GetString(data);
        return JsonSerializer.Deserialize<Message>(json)!;
    }
    
    public static byte[] ToBytes(Message message) {
        string json = JsonSerializer.Serialize(message);
        return Encoding.UTF8.GetBytes(json);
    }
}