
using Microsoft.AspNetCore.Mvc;
using Server.Services;

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

            var fileContent = FileReaderService.ReadTextFileWhole(filePath);
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
            var fileContent = FileReaderService.ReadTextLastLine(filePath);
            
            return Ok(fileContent);
        }
    }
} 