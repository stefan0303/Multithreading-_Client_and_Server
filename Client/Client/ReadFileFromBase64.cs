using System;
using System.IO;

class ReadFileFromBase64
{
    private string fileFolder;

    public ReadFileFromBase64(string fileFolder)
    {
        this.fileFolder = fileFolder;
    }
    public string ConvertFileToBase64()
    {

        Byte[] bytes = File.ReadAllBytes(fileFolder);
        String file = Convert.ToBase64String(bytes);
        return file;

    }
}

