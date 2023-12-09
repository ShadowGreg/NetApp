using System.Net;
using NewServer.infrastructure.Service;

namespace Tests;

public class ClientSearcherTests {
    private ClientSearcher _clientSearcher;

    public ClientSearcherTests() {
        // Arrange
        var activeClients = new Dictionary<string, IPEndPoint>() {
            { "127.0.0.1:8080", new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080) },
            { "127.0.0.1:8081", new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8081) },
        };
        // Initialize _activeClients with some sample data
        _clientSearcher = new ClientSearcher(activeClients);
    }

    [Fact]
    public void GetClient_ShouldReturnMatchingClients_WhenTextMatches() {
        // Act
        var result = _clientSearcher.GetClient("127.0.0.1:8080");

        // Assert
        // Write your assertions here to verify if the returned clients are as expected
        // For example:
        Assert.Single(result);
        Assert.Equal("127.0.0.1", result[0].Address.ToString());
        Assert.Equal("8080", result[0].Port.ToString());
    }

    [Fact]
    public void GetClient_ShouldReturnEmptyArray_WhenTextDoesNotMatch() {
        // Act
        var result = _clientSearcher.GetClient("nonexistent");

        // Assert
        Assert.Empty(result);
    }
    
    [Fact]
    public void GetClient_ShouldReturnMatchingClients_WhenSomeTextMatches() {
        // Act
        var result = _clientSearcher.GetClient("127.0.0.1:8080 Say Hello World!");

        // Assert
        // Write your assertions here to verify if the returned clients are as expected
        // For example:
        Assert.Single(result);
        Assert.Equal("127.0.0.1", result[0].Address.ToString());
        Assert.Equal("8080", result[0].Port.ToString());
    }

}