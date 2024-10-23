
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
                using StreamReader reader = new StreamReader(filePath);

                // Read the stream as a string.
                fileContent = reader.ReadToEnd();
            }

            return fileContent;
        }
        public static string ReadTextLastLine(string filePath)
        {
            string lastLine = null;
            string currentLine;
            if (System.IO.File.Exists(filePath))
            {
                // Open the text file using a stream reader.
                using StreamReader reader = new StreamReader(filePath);               
            
                while ((currentLine = reader.ReadLine()) != null)
                {
                    lastLine = currentLine; // Keep updating the last line
                }

                if (lastLine == null)
                {
                    Console.WriteLine("File is empty.");
                }   
            }            

            return lastLine;
        }
    }
}