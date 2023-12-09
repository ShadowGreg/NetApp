using System.Text;
using System.Text.Json;
using Domain;
using NewServer.Infrastructure;

namespace NewServer;

public class SentOperations(Server server, ClientBase clientBase) {

    // public Task Send(Message message) {
    //     var endPoint = clientBase._activeClients[message.Transmitter].IpEndPoint;
    //     var json = JsonSerializer.Serialize(message);
    //     server.Send(Encoding.UTF8.GetBytes(json), json.Length, endPoint);
    //     return Task.CompletedTask;
    // }
    //
    // public Task SendAll(Message message) {
    //     foreach (var user in clientBase._activeClients) {
    //         var endPoint = user.Value.IpEndPoint;
    //         var json = JsonSerializer.Serialize(message);
    //         Server.GetServer().Send(Encoding.UTF8.GetBytes(json), json.Length, endPoint);
    //     }
    //
    //     return Task.CompletedTask;
    // }
    //
    // public Task SendActiveUsers(Message message) {
    //     var endPoint = clientBase._activeClients[message.Author].IpEndPoint;
    //     var newMessage = new Message {
    //         Author = Server.GetName(),
    //         Transmitter = message.Author,
    //         Text = "Active users: " + string.Join(", ", clientBase._activeClients.Keys),
    //         Date = DateTime.Now
    //     };
    //     var json = JsonSerializer.Serialize(newMessage);
    //     Server.GetServer().Send(Encoding.UTF8.GetBytes(json), json.Length, endPoint);
    //     return Task.CompletedTask;
    // }
}