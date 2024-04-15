using UnityEngine;

public class Score : MonoBehaviour
{
    public HillopurkitUIManager ui;
    private int points = 0; // For purposes of running minigames back to back; don't forget to change this back to 0 later
    private readonly int pointsPerCorrectAnswer = 11;
    private readonly int pointsPenaltyPerWrongAnswer = 5;
    private readonly int winningPointLimit = 99;
    private int jarClicksWrong = 0;
    private int jarClicksRight = 0;
    private static readonly int streakOfThreePoints = 20;
    private static readonly int streakOfFourPoints = 50;
    private static readonly int streakOfFivePoints = 98;
    public int streak = 0;
    public int minStreakValue = 3;

    private void Start()
    {
        ui = GameObject.Find("UIDocument").GetComponent<HillopurkitUIManager>();
    }

    public void ClearScore()
    {
        ResetStats();
        ResetPoints();
        ui.ResetProgressBar(points);
    }

    public void BrokeCorrectJar(bool result)
    {
        // calculate chosen answer's points or penalty points and update progress bar accordingly
        if (result == true)
        {
            streak++; // increment streak value
            if (streak >= minStreakValue) {
                Debug.Log("\nStreak: " + streak);
                points += GetStreakPoints();
            }

            Debug.Log("\nIn BrokeCorrectJar, current points: " + points);
            Debug.Log("\nGained: " + pointsPerCorrectAnswer + " points");
            points += pointsPerCorrectAnswer;
            Debug.Log("\nIn BrokeCorrectJar, updated points: " + points);
            ui.UpProgressBar(points);
            jarClicksRight++;
            ui.SetFeedback(true);
        }

        else
        {
            ResetStreak();
            Debug.Log("\nStreak reset\nPenalty for incorrect guess: " + pointsPenaltyPerWrongAnswer);
            points -= pointsPenaltyPerWrongAnswer;
            if (points < 0) { // check if we are at negative points now, reset to 0 if true
                points = 0;
            }
            ui.UpProgressBar(points);
            jarClicksWrong++;    
            ui.SetFeedback(false);
        }

        // Check if enough points for win. Also progress to win if we've broken enough jars (i.e. we've completed the last round)
        if (points >= GetWinningPointLimit() 
        || jarClicksRight == (GameObject.Find("MiniGameManager").GetComponent<MiniGameManager>().GetTotalRounds()))
        {
            StartCoroutine(ui.DeclareWin());
        }
    }

    public int GetStreakPoints()
    {
        switch (streak)
        {
            case 3:
                return streakOfThreePoints;
            case 4:
                return streakOfFourPoints;
            case 5:
                return streakOfFivePoints;
            default:
                return 0;
        }
    }

    public int GetStreak() {
        return streak;
    }

    public void ResetStreak() {
        streak = 0;
    }

    public int GetPoints() {
        return points;
    }

    private void SetPoints(int points) {
        this.points = points;
    }

    private void ResetPoints() {
        // reset points to total points value as long as it's not zero
        if ((GameManager.totalPoints) <= 0) {
            SetPoints(66);
        } else {
            SetPoints(GameManager.totalPoints);
        }
    }

    public int GetPointsPerCorrectAnswer() {
        return pointsPerCorrectAnswer;
    }

    public int GetWinningPointLimit() {
        return winningPointLimit;
    }

    private void ResetStats()
    {
        jarClicksRight = 0;
        jarClicksWrong = 0;
    }

    public int[] GetStats()
    {
        int[] stats = new int[2];
        stats[0] = jarClicksRight;
        stats[1] = jarClicksWrong;
        return stats;
    }
}
