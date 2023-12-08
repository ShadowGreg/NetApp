using NewServer.UIUsege;

namespace NewServer.Infrastructure.Service;

public class CommandsController {
    public static string GetCommand(string messageText) {
        string?[] commands = typeof(CommandsText).GetFields().Select(x => x.GetValue(null).ToString()).ToArray();
        return commands.First(command => messageText.Contains(command));
    }

    public static bool IsCommands(string messageText) {
        string?[] commands = typeof(CommandsText).GetFields().Select(x => x.GetValue(null).ToString()).ToArray();
        return commands.Any(command => messageText.Contains(command));
    }
}