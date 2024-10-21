
namespace Server.Services
{
    public class FileReaderService
    {
        public static string ReadTextFileWhole(string filePath)
        {
            string fileContent = string.Empty;

            if (System.IO.File.Exists(filePath))
            {
                // Open the text file using a stream reader.
                using StreamReader reader = new(filePath);

                // Read the stream as a string.
                fileContent = reader.ReadToEnd();
            }

            return fileContent;
        }
    }
}