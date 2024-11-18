using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services
{
    public class TimerServices
    {
        public static string FindBestReadingTime(string filePath)
        {
            
            if (System.IO.File.Exists(filePath))
            {
                // Open the text file using a stream reader.
                using StreamReader reader = new(filePath);
                
                string fileContent = reader.ReadLine();
                if (fileContent == null) return null;
                
                int bestTime = int.Parse(fileContent);  // Initialize the best time as an integer.
                
                while ((fileContent = reader.ReadLine()) != null)
                {
                    int currentTime = int.Parse(fileContent);  // Convert each line to an integer.
                    if (currentTime < bestTime)
                    {
                        bestTime = currentTime;
                    }
                }
                return bestTime.ToString();
            }  
            return null;                
        }


        public static string WriteTimeToFile(long elapsedMilliseconds, string filePath)
        {

            if (System.IO.File.Exists(filePath))
            {
                // Open the text file using a stream reader.
                using StreamWriter writer = new(filePath, append:true);

                // Read the stream as a string.
                writer.WriteLine(elapsedMilliseconds);
            }

            return null;
        }
    }
}