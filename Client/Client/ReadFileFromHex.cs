using System;
using System.IO;

class ReadFileFromHex
{
    private string fileFolder;

    public ReadFileFromHex(string fileFolder)
    {
        this.fileFolder = fileFolder;
    }
    public string ConvertFileToHek()
    {
        Byte[] bytes = File.ReadAllBytes(fileFolder);
        string hexString = BitConverter.ToString(bytes);
        hexString = hexString.Replace("-", "");
        return hexString;
    }
}


