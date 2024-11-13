namespace Server.Services;
using Server.Exceptions;
using System.Text.Json;
using Shared.Models;
using System.Collections.Concurrent;
public class UserDataService
{
    private static ConcurrentDictionary<string, AttemptRecord> userRecords = new ConcurrentDictionary<string, AttemptRecord>();
    public static void SaveUserRecord(string username, int quizScore, string filePath)
    {

        if (string.IsNullOrWhiteSpace(username))
        {
            throw new EmptyNameException("Username cannot be null or empty.");
        }

        if (userRecords.ContainsKey(username))
        {
            throw new UserAlreadyExistsException("A user with this username already exists.");
        }
        
        AttemptRecord userRecord = new AttemptRecord(username, quizScore);
        
        userRecords.AddOrUpdate(username, userRecord, (key, oldValue) => userRecord);

        using StreamWriter streamWriter = new StreamWriter(filePath);
        streamWriter.Write(JsonSerializer.Serialize(userRecord));
    }
    public static void ClearUserRecords()
    {
        userRecords.Clear();
    }

}