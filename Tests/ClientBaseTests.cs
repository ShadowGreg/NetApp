using System.Net;
using NewServer;

namespace Tests;

public class ClientBaseTests {
    [Fact]
    public async Task Registration_AddsNewUserToActiveClients() {
        // Arrange
        string name = "John";
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
        ClientBase clientBase = new ClientBase();

        // Act
        await clientBase.Registration(name, endPoint);

        // Assert
        Assert.True(clientBase.GetActiveClients().ContainsKey(name));
    }
    

    [Fact]
    public async Task DeleteUser_FromActiveClients() {
        // Arrange
        string[] name = { "John", "Philip" };
        IPEndPoint[] endPoint = {
            new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080),
            new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8081)
        };
        ClientBase clientBase = new ClientBase();

        // Act
        for (int i = 0; i < name.Length; i++) {
            await clientBase.Registration(name[i], endPoint[i]);
        }
        await clientBase.DeleteUser("John");

        // Assert
        Assert.False(clientBase.GetActiveClients().ContainsKey("John"));
    }
}