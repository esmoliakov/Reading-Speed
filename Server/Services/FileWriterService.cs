namespace Server.Services;

public class FileWriterService
{
    public static void WriteToFile(string filePath, string content)
    {
        using (StreamWriter sw = new StreamWriter(filePath))
            sw.Write(content);
    }
}