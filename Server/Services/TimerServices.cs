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
            
                string fileContent = "";
                string bestTime = reader.ReadLine();
                while((fileContent = reader.ReadLine()) != null)
                {
                    if(int.Parse(fileContent) < int.Parse(bestTime))
                    {
                        bestTime = fileContent;
                    }
                }
                return bestTime;
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