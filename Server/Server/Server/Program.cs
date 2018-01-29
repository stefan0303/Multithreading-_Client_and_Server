using System;
using System.Net;

class Program
{

    static void Main(string[] args)
    {
        int port = 8888;
        IPAddress iPAddress = IPAddress.Parse(LocalIp.GetLocalIPAddress());
        Console.WriteLine("-- Server Started");
        Console.WriteLine("---- Waiting for clients");
        TcpServer server = new TcpServer(port, iPAddress);

    }
}







