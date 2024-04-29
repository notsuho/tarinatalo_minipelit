using UnityEngine;

public class Score : MonoBehaviour
{
    public HillopurkitUIManager ui;
    private readonly int pointsPerCorrectAnswer = 30;
    private readonly int pointsPenaltyPerWrongAnswer = -5;
    private int rightAnswers = 0;
    private int totalRounds;
    private int currentProgress = 33; // This minigame's progress goes from 33 to 66
    private int progressPerCorrectAnswer;

    private void Start()
    {
        ui = GameObject.Find("UIDocument").GetComponent<HillopurkitUIManager>();

        totalRounds = GameObject.Find("MiniGameManager").GetComponent<MiniGameManager>().roundsTotal;
        progressPerCorrectAnswer = (33 / totalRounds);
    }

    public void ClearScore()
    {
        rightAnswers = 0;
        ui.ResetProgressBar(currentProgress);
    }

    public void BrokeCorrectJar(bool result)
    {
        // Calculate chosen answer's points or penalty points and update progress bar accordingly.
        if (result)
        {
            rightAnswers++;
            GameManager.AddPoints(true, pointsPerCorrectAnswer);

            if (rightAnswers == totalRounds) // workaround for integer rounding
                currentProgress = 66;
            else
                currentProgress += progressPerCorrectAnswer;

            ui.UpProgressBar(currentProgress);
            ui.SetFeedback(true);
        }

        else
        {
            GameManager.AddPoints(false, pointsPenaltyPerWrongAnswer);

            if (GameManager.totalPoints < 0) // never have a negative amount of points
            {
                GameManager.totalPoints = 0;
            }

            ui.UpProgressBar(33 + (34 / totalRounds * rightAnswers));
            ui.SetFeedback(false);
        }

        if (rightAnswers == totalRounds) // right answers = amount of rounds total => game has been won
        {
            StartCoroutine(ui.DeclareWin());
        }
    }
}