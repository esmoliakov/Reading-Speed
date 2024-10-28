using Shared.Models;
using System.Linq;
using System.Collections;
using Server.Extensions;

namespace Server.Services;

public class QuizService
{
    //using arraylist to illustrate boxing and unboxing
    private static ArrayList scoreList = new ArrayList();
    public static int QuizScore(List<Question> questions)
    {
        //using linq to count correct answers
        var correctCount = questions.Count(q => q.correctAnswer.Equals(q.userAnswer));
        var questionCount = questions.Count;

        //calculate score
        // Calculate score and round it to the nearest integer
        int score = (int)Math.Round((double)correctCount / questionCount * 100); // Convert to int directly

        scoreList.Add(score); // Store as an integer directly

        return score; // Return the score as int
    }
    public static void printScores()
    {
        foreach (object score in scoreList)
        {
            int unboxedScore = (int)score; //unboxing
            Console.WriteLine(unboxedScore);
        }
    }
    // New method to get scores as a List<int> using the extension method
    public static List<int> GetScoresAsList()
    {
        return scoreList.ToIntList(); // use this
    }
}