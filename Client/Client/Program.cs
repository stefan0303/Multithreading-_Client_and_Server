
//First Start the Server!
class Program
{
    static void Main()
    {
        string mycurrentIp = LocalIp.GetLocalIPAddress();

        Client client = new Client(mycurrentIp, 8888);
        client.ConnectToServer();
    }
}

