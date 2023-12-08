using System.Net;

namespace NewServer; 

public class User(string name, IPEndPoint iPEndPoint) {
    public string Name { get;   } = name;
    public IPEndPoint IpEndPoint { get; } = iPEndPoint;
}