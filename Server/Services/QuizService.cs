using Shared.Models;
using System.Linq;
using System.Collections;
using Shared.Models.DTOs;

namespace Server.Services;

public class QuizService
{
    //using arraylist to illustrate boxing and unboxing
    private static ArrayList scoreList = new ArrayList();
    public int QuizScore(List<UserAnswerDTO> userAnswers)
    {
        //using linq to count correct answers
        var correctCount = userAnswers.Count(a => a.Question.CorrectAnswer.Equals(a.UserAnswer));
        var answerCount = userAnswers.Count;

        //calculate score
        // Calculate score and round it to the nearest integer
        int score = (int)Math.Round((double)correctCount / answerCount * 100); // Convert to int directly

        scoreList.Add(score); // Store as an integer directly

        return score; // Return the score as int
    }
    
    public double  CalculateWPM(long elapsedMilliseconds, int wordCount)
    {
        if (elapsedMilliseconds == 0 || wordCount == 0) return 0; // Avoid division by zero
        double elapsedMinutes = elapsedMilliseconds / 60000.0; // Convert milliseconds to minutes
        return Math.Round(wordCount / elapsedMinutes); // Calculate WPM
    }
}