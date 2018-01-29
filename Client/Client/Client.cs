using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

class Client
{
    private TcpClient client;
    private bool isRunning;
    private StreamWriter sWriter;

    private StreamReader sReader;
    public Client(string ipAddress, int port)
    {
        client = new TcpClient();
        client.Connect(ipAddress, port);
    }

    public const int NotFound = -1;
    public int GetPositionOfLastByteWithData(params byte[] array)//When we use NetworkStream
    {
        for (int i = array.Length - 1; i > -1; i--)
        {
            if (array[i] > 0) { return i; }
        }
        return NotFound;
    }
    public int GetPositionOfLastCharWithData(params char[] array)//When we use StreamReader
    {
        for (int i = array.Length - 1; i > -1; i--)
        {
            if (array[i] > 0) { return i; }
        }
        return NotFound;
    }
    public void ConnectToServer()
    {
        sWriter = new StreamWriter(client.GetStream(), Encoding.ASCII);
        sReader = new StreamReader(client.GetStream(), Encoding.ASCII);
        isRunning = true;
        while (isRunning)
        {

            try
            {
                Console.WriteLine("Write type of file you want to send ('b' for base 64 and 'h' for Hex)");
                string typeOfData = Console.ReadLine();
                if (typeOfData == "b")
                {
                    Console.WriteLine("Write text folder");
                    // This is an example of text file folder, use your own text file
                    string textFolder = @"C:\Users\Stefan\Desktop\send.txt";
                    ReadFileFromBase64 readFileFromBase64 = new ReadFileFromBase64(textFolder);
                    string base64File = readFileFromBase64.ConvertFileToBase64();

                    //send data
                    sWriter.WriteLine("b" + base64File);
                    sWriter.Flush();
                }
                else if (typeOfData == "h")
                {
                    Console.WriteLine("Write text folder");
                    //This is an example of text file folder, use your own text file
                    string textFolder = @"C:\Users\Stefan\Desktop\sendHexFormatText.txt";
                    ReadFileFromHex readFileFromHex = new ReadFileFromHex(textFolder);
                    string hexFile = readFileFromHex.ConvertFileToHek();

                    //send data
                    sWriter.WriteLine("h" + hexFile);
                    sWriter.Flush();
                }
                else
                {
                    Console.WriteLine("The file is not in correct format ('b' or 'h')");
                    continue;
                }
                //send Data to server

                //Stream Reader is reading from char array
                //Receive data from server
                //network Stream is reading from byte array
                NetworkStream serverStream = client.GetStream();
                byte[] inStream = new byte[10000];
                serverStream.Read(inStream, 0, client.ReceiveBufferSize);
                string returnData = System.Text.Encoding.ASCII.GetString(inStream);
                //get last index from the receive byte array
                int index = GetPositionOfLastByteWithData(inStream);

                //remove from string empty space
                returnData = returnData.Substring(0, index + 1);

                //Print with NetworkStream or StreamReader
                Console.WriteLine("Data from Server : " + returnData);

            }
            catch (Exception)
            {
                throw new ArgumentException("There is problem with the text size!");
            }

        }

    }


}