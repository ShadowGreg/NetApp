using System.Text;
using NewServer.Service;
using System.Text.Json;
using Domain;


namespace Tests;

public class MessageMapperTests {
    [Fact]
    public void ToMessage_ValidJson_ReturnsMessageObject() {
        // Arrange
        byte[] data =
            Encoding.UTF8.GetBytes(
                "{\n  \"Text\": \"Hello\",\n  \"Author\": \"1\",\n  \"Transmitter\": \"2\",\n  \"Date\": \"2023-12-08T13:24:46.4168493+03:00\"\n}");

        // Act
        Message result = MessageMapper.ToMessage(data);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("1", result.Author);
        Assert.Equal("Hello", result.Text);
    }

    [Fact]
    public void ToBytes_ConvertNullMessageObjectToByteArray_ReturnNull() {
        // Arrange
        Message message = null;
        byte[] expected = new byte[] { 110, 117, 108, 108 };

        // Act
        byte[] result = MessageMapper.ToBytes(message);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToBytes_ConvertEmptyMessageObjectToByteArray_Success() {
        // Arrange
        Message message = new Message() {
            Text = "Hello",
            Author = "1",
            Transmitter = "2",
            Date = DateTime.Now
        };

        // Act
        byte[] result = MessageMapper.ToBytes(message);
        string json = Encoding.UTF8.GetString(result);
        Message deserializedMessage = JsonSerializer.Deserialize<Message>(json);

        // Assert
        Assert.Equal(message.Author, deserializedMessage.Author);
        Assert.Equal(message.Text, deserializedMessage.Text);
    }
}