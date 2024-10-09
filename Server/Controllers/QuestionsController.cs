using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
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
    }
}