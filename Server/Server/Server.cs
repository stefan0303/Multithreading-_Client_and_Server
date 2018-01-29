
//Class handle each client separatly
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class TcpServer
{
    private TcpListener server;
    private bool isRunning;

    public TcpServer(int port, IPAddress iPAddress)
    {
        server = new TcpListener(iPAddress, port);
        server.Start();
        isRunning = true;

        LoopAllClients();

    }
    public void LoopAllClients()
    {
        int count = 0; // Make count for naming threads
        while (isRunning)
        {
            TcpClient newClient = server.AcceptTcpClient();//we found new client connected

            count++;
            //Create a new thread for every client
            object obj = new object();
            Task task = Task.Run(() => { HandleClient(newClient, count.ToString()); });
            Console.WriteLine("This is client number: " + task.Id);

        }
    }
    public void HandleClient(TcpClient client, string name)
    {
        // retrieve client from parameter passed to thread

        // TcpClient client = (TcpClient)obj;
        bool bClientConnected = true;

        StreamReader sReader = new StreamReader(client.GetStream());

        NetworkStream networkStream;

        while (bClientConnected)
        {
            string data = "";
            data = sReader.ReadLine();

            Console.WriteLine($"Client {name} data: {data}");
            Console.WriteLine();
            networkStream = client.GetStream();
            SendDataToClient(networkStream, data, client);

        }
    }
    public static void SendDataToClient(NetworkStream networkStream, string dataforClient, TcpClient clientSocket)
    {
        //decode the data
        //check the file is in Hex or Base64(h or b is the first letter)
        string typeOfData = dataforClient.Substring(0, 1);
        //remove the first char of send data
        dataforClient = dataforClient.Substring(1);

        if (typeOfData == "b")//base64
        {

            dataforClient = GetDataFromClientInBase64(dataforClient);
            Console.WriteLine("Sending  decodet data from base 64 to clinet => : ");

        }
        else//hex
        {

            dataforClient = GetDataFromClientInHex(dataforClient);
            Console.WriteLine("Sending  decodet data from Hex format to clinet => : ");
        }
        Console.WriteLine("-->" + dataforClient);

        Byte[] sendBytes = Encoding.ASCII.GetBytes(dataforClient);
        networkStream.Write(sendBytes, 0, sendBytes.Length);// send data to client
        dataforClient = "";
    }
    public static string GetDataFromClientInBase64(string dataFromBase64)
    {
        byte[] data = Convert.FromBase64String(dataFromBase64);
        string decodedString = Encoding.UTF8.GetString(data);
        data = null;
        return decodedString;
    }
    public static string GetDataFromClientInHex(string dataFromHex)
    {
        byte[] bb = Enumerable.Range(0, dataFromHex.Length)
                 .Where(x => x % 2 == 0)
                 .Select(x => Convert.ToByte(dataFromHex.Substring(x, 2), 16))
                 .ToArray();
        return System.Text.Encoding.ASCII.GetString(bb);
    }
}