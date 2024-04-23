using UnityEngine;

public class Score : MonoBehaviour
{
    public HillopurkitUIManager ui;
    // private int points
    private readonly int pointsPerCorrectAnswer = 11;
    private readonly int pointsPenaltyPerWrongAnswer = -5;
    private readonly int winningPointLimit = 99;
    private int jarClicksWrong = 0;
    private int jarClicksRight = 0;
    private int totalRounds;
    public int currentProgress = 33; // This minigame's progress goes from 33 to 66
    public int progressPerCorrectAnswer;
    public int minStreakValue = 3;



    private void Start()
    {
        ui = GameObject.Find("UIDocument").GetComponent<HillopurkitUIManager>();

        totalRounds = GameObject.Find("MiniGameManager").GetComponent<MiniGameManager>().GetTotalRounds();
        progressPerCorrectAnswer = (33 / totalRounds);
    }

    public void ClearScore()
    {
        ResetStats();
        //ResetPoints();
        ui.ResetProgressBar(currentProgress);
    }

    public void BrokeCorrectJar(bool result)
    {
        // calculate chosen answer's points or penalty points and update progress bar accordingly
        if (result)
        {

            GameManager.AddPoints(true, pointsPerCorrectAnswer);
            Debug.Log("Pelin pisteet: " + GameManager.totalPoints);
            jarClicksRight++;

            if (jarClicksRight == totalRounds) // workaround with integer rounding
                currentProgress = 66;
            else
                currentProgress += progressPerCorrectAnswer;

            print("CURRENT PROGRESS: " + currentProgress);
            ui.UpProgressBar(currentProgress);
            ui.SetFeedback(true);
        }

        else
        {
            GameManager.AddPoints(false, pointsPenaltyPerWrongAnswer);
            Debug.Log("Pelin pisteet: " + GameManager.totalPoints);
            if (GameManager.totalPoints < 0) { // check if we are at negative points now, reset to 0 if true
                GameManager.totalPoints = 0;
            }
            ui.UpProgressBar(33 + (34 / 5 * jarClicksRight));
            jarClicksWrong++;    
            ui.SetFeedback(false);
        }

        // Check if enough points for win. Also progress to win if we've broken enough jars (i.e. we've completed the last round)
        if (jarClicksRight == (GameObject.Find("MiniGameManager").GetComponent<MiniGameManager>().GetTotalRounds()))
        {
            StartCoroutine(ui.DeclareWin());
        }
    }


    /* private void ResetPoints() {
        this.points = GameManager.totalPoints;
    } */

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
