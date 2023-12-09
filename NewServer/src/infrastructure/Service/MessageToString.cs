using System.Text.Json;
using Domain;

namespace NewServer.Service; 

public class MessageToString {
    public static string ToString(Message message) {
        return JsonSerializer.Serialize(message);
    }
}