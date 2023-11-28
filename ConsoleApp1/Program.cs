
using ConsoleApp1;

// Create a new message.
Message message = new Message();
message.Text = "This is a message.";
message.Author = "John Doe";
message.Transmitter = "Jane Doe";
message.Date = DateTime.Now;
message.Id = 1;


// Serialize the message to JSON.
string json = MessageMapper.SerializeMessageToJson(message);

// Deserialize the message from JSON.
Message? deserializedMessage = MessageMapper.DeserializeMessageFromJson(json);

// Print the message.
Console.WriteLine(deserializedMessage.Text);