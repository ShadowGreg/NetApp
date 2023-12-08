namespace NewServer.Service;

public static class ConsoleLogger {
    public static void Log(string message) {
        Console.WriteLine(DateTime.Now + " " + message);
    }
}