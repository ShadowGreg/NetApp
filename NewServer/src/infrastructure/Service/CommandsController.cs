using NewServer.UIUsege;

namespace NewServer.Infrastructure.Service;

public class CommandsController {
    public static string GetCommand(string? messageText) {
        string?[] commands = GetCommands();
        return commands.First(command => messageText.Contains(command));
    }

    public static bool IsCommands(string? messageText) {
        string?[] commands = GetCommands();
        foreach (var command in commands) {
            if (messageText.Contains(command)) {
                return true;
            }
        }

        return false;
    }

    public static string?[] GetCommands() {
        return typeof(CommandsText)
            .GetFields()
            .Select(
                x => x
                    .GetValue(null)
                    ?.ToString()
                    ?.Trim('\n', ' ')
            )
            .ToArray();
    }
}