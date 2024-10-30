namespace Server.Services;
using Server.Exceptions;
using System.Text.Json;
using Shared.Models;

public class UserDataService
{
    public static void SaveUserRecord(string username, int quizScore, string filePath)
    {

        if (string.IsNullOrWhiteSpace(username))
        {
            throw new EmptyNameException("Username cannot be null or empty.");
        }
        
        AttemptRecord userRecord = new AttemptRecord(username, quizScore);
        
        using StreamWriter streamWriter = new StreamWriter(filePath);
        
        streamWriter.Write(JsonSerializer.Serialize(userRecord));
    }
}