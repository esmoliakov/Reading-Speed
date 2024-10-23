using Shared.Models;
using System.Linq;
using System.Collections;

namespace Server.Services;

public class QuizService
{
    /*public static int CorrectAnswers(List<Question> questions)
    {
        int correctAnswers = 0;
        foreach (var question in questions)
        {
            //Console.WriteLine(question.userAnswer);
            if(question.correctAnswer.Equals(question.userAnswer))
                ++correctAnswers;
        }
        return correctAnswers;
    }

    public static int QuizScore(List<Question> questions)
    {
        var correctCount = CorrectAnswers(questions);
        var questionCount = questions.Count;
        double score = (double)correctCount / questionCount * 100;
        int resultScore = (int)Math.Round(score);
        return resultScore;
    }
    */
    //using arraylist to illustrate boxing and unboxing
    private static ArrayList scoreList = new ArrayList();
    public static int QuizScore(List<Question> questions)
    {
        //using linq to count correct answers
        var correctCount = questions.Count(q => q.correctAnswer.Equals(q.userAnswer));
        var questionCount = questions.Count;

        //calculate score
        double score = (double)correctCount / questionCount * 100;
        int finalScore = (int)Math.Round(score);
        scoreList.Add(finalScore); // boxing
        return finalScore;
    }
    public static void printScores()
    {
        foreach (object score in scoreList)
        {
            int unboxedScore = (int)score; //unboxing
            Console.WriteLine(unboxedScore);
        }
    }
}