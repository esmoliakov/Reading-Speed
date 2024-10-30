using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Server.Services;
using Shared.Models;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/questions")]

    public class QuestionsController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;

        public QuestionsController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [HttpGet("get-questions")]
        public IActionResult GetQuestions(string fileName)
        {
            var filePath = Path.Combine(_environment.ContentRootPath, "Files", fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("Questions file not found.");
            }

            using StreamReader reader = new(filePath);

            // Read the questions from the JSON file
            string fileContent = reader.ReadToEnd();

            List<Question> questions = JsonSerializer.Deserialize<List<Question>>(fileContent);

            return Ok(questions);
        }
        
        [HttpGet("get-questions-sorted")]
        public IActionResult GetQuestionsSorted(string fileName="random")
        {
            if (fileName.Equals("random"))
            {
                string fileId =
                    FileReaderService.ReadTextFileWhole(Path.Combine(_environment.ContentRootPath, "Files",
                        "paragraphId.txt")).Trim();
                fileName = $"{fileId}_questions.json";
            }
            var filePath = Path.Combine(_environment.ContentRootPath, "Files", fileName);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("Questions file not found.");
            }

            using StreamReader reader = new(filePath);

            // Read the questions from the JSON file
            string fileContent = reader.ReadToEnd();

            List<Question> questions = JsonSerializer.Deserialize<List<Question>>(fileContent);
            
            if(questions != null)
                questions.Sort();
            
            return Ok(questions);
        }
    }
}