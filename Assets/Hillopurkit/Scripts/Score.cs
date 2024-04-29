using UnityEngine;

public class Score : MonoBehaviour
{
    public HillopurkitUIManager ui;
    private readonly int pointsPerCorrectAnswer = 30;
    private readonly int pointsPenaltyPerWrongAnswer = -5;
    private int wrongJarsClicked = 0;
    private int rightJarsClicked = 0;
    private int totalRounds;
    private int currentProgress = 33; // This minigame's progress goes from 33 to 66
    private int progressPerCorrectAnswer;
    public int minStreakValue = 3; // how many right answers before streak bonuses begin

    private void Start()
    {
        ui = GameObject.Find("UIDocument").GetComponent<HillopurkitUIManager>();

        totalRounds = GameObject.Find("MiniGameManager").GetComponent<MiniGameManager>().GetTotalRounds();
        progressPerCorrectAnswer = (33 / totalRounds);
    }

    public void ClearScore()
    {
        ResetStats();
        ui.ResetProgressBar(currentProgress);
    }

    public void BrokeCorrectJar(bool result)
    {
        // Calculate chosen answer's points or penalty points and update progress bar accordingly.
        if (result)
        {
            rightJarsClicked++;
            GameManager.AddPoints(true, pointsPerCorrectAnswer);

            if (rightJarsClicked == totalRounds) // workaround for integer rounding
                currentProgress = 66;
            else
                currentProgress += progressPerCorrectAnswer;

            ui.UpProgressBar(currentProgress);
            ui.SetFeedback(true);
        }

        else
        {
            wrongJarsClicked++;  
            GameManager.AddPoints(false, pointsPenaltyPerWrongAnswer);

            if (GameManager.totalPoints < 0) // never have a negative amount of points
            { 
                GameManager.totalPoints = 0;
            }

            ui.UpProgressBar(33 + (34 / totalRounds * rightJarsClicked));  
            ui.SetFeedback(false);
        }

        if (rightJarsClicked == totalRounds) // right answers = amount of rounds total => game has been won
        {
            StartCoroutine(ui.DeclareWin());
        }
    }

    private void ResetStats()
    {
        rightJarsClicked = 0;
        wrongJarsClicked = 0;
    }

    public int[] GetStats()
    {
        int[] stats = new int[2];
        stats[0] = rightJarsClicked;
        stats[1] = wrongJarsClicked;
        return stats;
    }
}