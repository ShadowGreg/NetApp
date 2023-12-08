using System.Net;
using System.Net.Sockets;
using NewServer;
using NewServer.Infrastructure;
using NewServer.Infrastructure.Service;

namespace Tests;

public class ServerTests {
    [Fact]
    public void GetServer_ReturnsServerInstance() {
        // Arrange
        var server = new Server(new ClientBase());

        // Act
        var result = server.GetServer();

        // Assert
        Assert.IsType<UdpClient>(result);
    }

    [Fact]
    public void Run_Online_ReturnsServerInstance() {
        // Arrange
        ClientBase clientBase = new ClientBase();
        var server = new Server(clientBase);
        Task.Run(() => server.Run());

        // Act
        Task.Run(() =>
        {
            UdpClient udpClient = new UdpClient();
            udpClient.Connect("127.0.0.1", 4444);
            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 4444);
            udpClient.Close();
        });

        var result = server.GetServer();

        // Assert
        Assert.IsType<UdpClient>(result);
    }


    [Fact]
    public void IsCommands_ShouldReturnTrue_WhenMessageTextContainsACommand() {
        // Arrange
        var messageText = "Hello, World! server -d";

        // Act
        bool result = CommandsController.IsCommands(messageText);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsCommands_ShouldReturnFalse_WhenMessageTextDoesNotContainAnyCommand() {
        // Arrange
        var messageText = "Hello, World! server ";

        // Act
        bool result = CommandsController.IsCommands(messageText);

        // Assert
        Assert.False(result);
    }
}