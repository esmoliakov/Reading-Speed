using System;
using System.IO;

public static class Logger
{
    public static void LogException(Exception ex, string filePath)
    {
        using StreamWriter writer = new StreamWriter(filePath, append: true);
        writer.WriteLine($"{DateTime.Now}: {ex.Message}");
        writer.WriteLine(ex.StackTrace);
    }
}
