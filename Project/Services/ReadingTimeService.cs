using Blazored.LocalStorage;
using System.Text.Json;
using Shared.Models;

public class ReadingTimeService
{
    private readonly ILocalStorageService _localStorage;
    private List<int> pastTimes = new List<int>();
    private const string StorageKey = "reading_times";
    private const string UserRecordKey = "user_record";  // Key for storing UserRecord

    public ReadingTimeService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task InitializeAsync()
    {
        // Load saved times from local storage
        var storedTimes = await _localStorage.GetItemAsync<string>(StorageKey);
        if (storedTimes != null)
        {
            pastTimes = JsonSerializer.Deserialize<List<int>>(storedTimes) ?? new List<int>();
        }
    }

    public async Task AddTimeAsync(int time)
    {
        if (pastTimes.Count >= 3)
        {
            pastTimes.RemoveAt(0); // keep only last 3 tries
        }
        pastTimes.Add(time);

        // Saving the reading times to local storage
        await _localStorage.SetItemAsync(StorageKey, JsonSerializer.Serialize(pastTimes));
    }

    public List<int> GetPastTimes()
    {
        return new List<int>(pastTimes); // return a copy of the list
    }

    // save userRecord to local storage
    public async Task SaveUserRecordAsync(AttemptRecord userRecord)
    {
        await _localStorage.SetItemAsync(UserRecordKey, JsonSerializer.Serialize(userRecord));
    }

    // get userRecord from local storage
    public async Task<AttemptRecord?> GetUserRecordAsync()
    {
        var storedRecord = await _localStorage.GetItemAsync<string>(UserRecordKey);
        if (storedRecord != null)
        {
            return JsonSerializer.Deserialize<AttemptRecord>(storedRecord);
        }
        return null;
    }
}
