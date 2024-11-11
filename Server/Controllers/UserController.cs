using Microsoft.AspNetCore.Mvc;
using Server.Services;
using Server.Exceptions;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    [HttpPost("save")]
    public IActionResult SaveUser(string username, int quizScore)
    {
        try
        {
            string filePath = "user_data.json"; // Specify your path
            UserDataService.SaveUserRecord(username, quizScore, filePath);
            return Ok("User record saved successfully.");
        }
        catch (UserAlreadyExistsException ex)
        {
            Logger.LogException(ex, "path_to_log_file.log");
            return BadRequest(ex.Message); // Return 400 Bad Request with the error message
        }
        catch (EmptyNameException ex)
        {
            // Log the exception
            Logger.LogException(ex, "path_to_log_file.log");
            return BadRequest(ex.Message); // Return 400 Bad Request with the error message
        }
        catch (Exception ex)
        {
            // Handle other exceptions
            Logger.LogException(ex, "path_to_log_file.log");
            return StatusCode(500, "An unexpected error occurred. Please try again later."); // Return 500 Internal Server Error
        }
    }
}
