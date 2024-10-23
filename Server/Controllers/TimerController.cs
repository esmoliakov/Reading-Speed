using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;

[ApiController]
[Route("api/timer")]
public class TimerController : ControllerBase
{
    private static Stopwatch stopwatch = new Stopwatch();
        private readonly IWebHostEnvironment _environment;

        public TimerController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

    [HttpGet("start")]
    public IActionResult StartTimer()
    {
        stopwatch.Start();
        return Ok("Stopwatch started.");
    }

    [HttpGet("write-to-file")]
    public IActionResult WriteToFile(long elapsedMilliseconds)
     {
    // Save the elapsed time using ReadingTimeService
            string fileName = "stopwatch.txt";
            var filePath = Path.Combine(_environment.ContentRootPath, "Files", fileName);
             if (!System.IO.File.Exists(filePath))
            {
                return NotFound("Questions file not found.");
            }
            

            using StreamWriter writer = new(filePath, append:true);

            // Read the questions from the JSON file
            writer.WriteLine(elapsedMilliseconds);
            return Ok();
    }
    [HttpGet("stop")]
    public async Task<IActionResult> StopTimer()
    {
        if (stopwatch.IsRunning)
        {
            stopwatch.Stop();
            long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
            stopwatch.Reset(); // Reset for next use
            WriteToFile(elapsedMilliseconds);
            

            return Ok();
        }
        return BadRequest("Stopwatch is not running.");
    }

        [HttpGet("read-text-file-find-best-time")]
        public IActionResult ReadTextFile(string fileName)
        {
            // Getting the filepath when files are in the "Files" folder
            var filePath = Path.Combine(_environment.ContentRootPath, "Files", fileName);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File not found.");
            }

            // Open the text file using a stream reader.
            using StreamReader reader = new(filePath);

            string fileContent = "";
            string bestTime = "0";
            while((fileContent = reader.ReadLine()) != null)
            {
                if(int.Parse(fileContent) > int.Parse(bestTime))
                {
                    bestTime = fileContent;
                }
            }

            return Ok(bestTime);
        }
}
