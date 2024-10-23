
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/files")]
    public class FileController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;

        public FileController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [HttpGet("read-text-file")]
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

            // Read the stream as a string.
            string fileContent = reader.ReadToEnd();

            return Ok(fileContent);
        }
        [HttpGet("read-last-line")]
        public IActionResult ReadLastFileLine(string fileName)
        {
            // Getting the filepath when files are in the "Files" folder
            var filePath = Path.Combine(_environment.ContentRootPath, "Files", fileName);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File not found.");
            }

            // Open the text file using a stream reader.
            using StreamReader reader = new(filePath);
            string lastLine = null;
            string currentLine;
            
            while ((currentLine = reader.ReadLine()) != null)
            {
                lastLine = currentLine; // Keep updating the last line
            }

            if (lastLine == null)
            {
                Console.WriteLine("File is empty.");
            }

            return Ok(lastLine);
        }
    }
} 