namespace Server.Services;

using System.Text.Json;
using Shared.Models;

public class UserDataService
{
    public static void SaveUserRecord(string username, int quizScore, string filePath)
    {
        UserRecord userRecord = new UserRecord(username, quizScore);
        
        using StreamWriter streamWriter = new StreamWriter(filePath);
        
        streamWriter.Write(JsonSerializer.Serialize(userRecord));
    }
}