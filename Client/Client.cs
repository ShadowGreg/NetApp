﻿using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using ConsoleApp1;

namespace Client;

public class Client {
    private UdpClient udpClient;
    private IPEndPoint remoteEndPoint;

    public static int ID { get; private set; }

    public Client(int serverPort, string serverIp) {
        udpClient = new UdpClient();
        remoteEndPoint = new IPEndPoint(IPAddress.Parse(serverIp), serverPort);
        ID = Guid.NewGuid().GetHashCode() ;
    }

    public void SendMessage(Message? message) {
        string json = JsonSerializer.Serialize(message);
        byte[] data = Encoding.UTF8.GetBytes(json);
        udpClient.Send(data, data.Length, remoteEndPoint);
    }

    public Message? ReceiveMessage() {
        byte[] data = udpClient.Receive(ref remoteEndPoint);
        string json = Encoding.UTF8.GetString(data);
        return JsonSerializer.Deserialize<Message>(json);
    }

    public void CloseConnection() {
        udpClient.Close();
    }
}