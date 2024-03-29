using UnityEngine;

public class Score : MonoBehaviour
{
    public HillopurkitUIManager ui;
    private float points = 33f; // For purposes of running minigames back to back; don't forget to change this back to 0 later
    private float pointsPerCorrectAnswer = 11f;
    private float winningPointLimit = 99f;
    private int jarClicksWrong = 0;
    private int jarClicksRight = 0;

    private void Start()
    {
        ui = GameObject.Find("UIDocument").GetComponent<HillopurkitUIManager>();
    }

    public void ClearScore()
    {
        ResetTally();
        ResetPoints();
        ui.ResetProgressBar();
    }

    public void BrokeCorrectJar(bool result)
    {
        if (result)
        {
            Debug.Log("\nIn BrokeCorrectJar, current points: " + points);
            Debug.Log("\nGained: " + pointsPerCorrectAnswer + " points");
            points = points + pointsPerCorrectAnswer;
            Debug.Log("\nIn BrokeCorrectJar, updated points: " + points);
            ui.UpProgressBar(points);
            jarClicksRight++;
            ui.SetFeedback(true);
        }

        else
        {
            jarClicksWrong++;
            ui.SetFeedback(false);
        }

        if (points >= winningPointLimit 
        || jarClicksRight == (GameObject.Find("MiniGameManager").GetComponent<MiniGameManager>().GetTotalRounds()))
        // Check if enough points for win. Also progress to win if we've broken enough jars (i.e. we've completed the last round)
        {
            StartCoroutine(ui.DeclareWin());
        }
    }

    public float GetPoints() {
        return points;
    }

    private void ResetPoints() {
        //points = 0;
        points = 33f;
    }

    public float GetPointsPerCorrectAnswer() {
        return pointsPerCorrectAnswer;
    }

    public float getWinningPointLimit() {
        return winningPointLimit;
    }

    private void ResetTally()
    {
        jarClicksRight = 0;
        jarClicksWrong = 0;
    }

    public int[] GetTally()
    {
        int[] tally = new int[2];
        tally[0] = jarClicksRight;
        tally[1] = jarClicksWrong;
        return tally;
    }
}
