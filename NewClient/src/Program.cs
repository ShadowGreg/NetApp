

public class Program
{
    public static void Main(string[] args)
    {
        string serverIp = "127.0.0.1";
        int serverPort = 4444;

        UdpClient udpClient = new UdpClient(serverIp, serverPort);
        udpClient.Run();
    }
}