using UnityEngine;

public class Score : MonoBehaviour
{
    public HillopurkitUIManager ui;
    private readonly int pointsPerCorrectAnswer = 30;
    private readonly int pointsPenaltyPerWrongAnswer = -5;
    private int rightAnswers = 0;
    private int totalRounds;

    private void Start()
    {
        ui = GameObject.Find("UIDocument").GetComponent<HillopurkitUIManager>();

        totalRounds = GameObject.Find("MiniGameManager").GetComponent<MiniGameManager>().roundsTotal;
    }

    public void ClearScore()
    {
        rightAnswers = 0;
    }

    public void BrokeCorrectJar(bool result)
    {
        // Calculate chosen answer's points or penalty points and update progress bar accordingly.
        if (result)
        {
            rightAnswers++;
            GameManager.AddPoints(true, pointsPerCorrectAnswer);
            ui.SetFeedback(true);
        }

        else
        {
            GameManager.AddPoints(false, pointsPenaltyPerWrongAnswer);
            ui.SetFeedback(false);
        }

        if (rightAnswers == totalRounds) // right answers = amount of rounds total => game has been won
        {
            ui.SetFeedback(true);
            StartCoroutine(ui.DeclareWin());
        }
    }
}