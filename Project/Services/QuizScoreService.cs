using Project.Enums;

public class QuizScoreService
{
    public QuizScoreLevel GetScoreLevel(int score)
    {
        if (score > 80)
        {
            return QuizScoreLevel.High;
        }
        else if (score >= 60)
        {
            return QuizScoreLevel.Moderate;
        }
        else if (score >= 40)
        {
            return QuizScoreLevel.Average;
        }
        else
        {
            return QuizScoreLevel.Low;
        }
    }
}
