using UnityEngine;

public class Score : MonoBehaviour
{
    public HillopurkitUIManager ui;
    private float points = 0f;
    [SerializeField] private float pointsPerCorrectAnswer = 33f;
    [SerializeField] private float winningPointLimit = 99f;
    private int jarClicksWrong = 0;
    private int jarClicksRight = 0;

    private void Start()
    {
        ui = GameObject.Find("UIDocument").GetComponent<HillopurkitUIManager>();
    }

    public void ClearScore()
    {
        ResetTally();
        ui.UpProgressBar(0f);
    }

    public void BrokeCorrectJar(bool result)
    {
        if (result)
        {
            points += pointsPerCorrectAnswer;
            ui.UpProgressBar(points);
            jarClicksRight++;
            ui.SetFeedback(true);
        }

        else
        {
            jarClicksWrong++;
            ui.SetFeedback(false);
        }

        if (points >= winningPointLimit)
        {
            StartCoroutine(ui.DeclareWin());
        }
    }

    private void ResetTally()
    {
        jarClicksRight = 0;
        jarClicksWrong = 0;
    }

    public int[] GetTally()
    {
        int[] points = new int[2];
        points[0] = jarClicksRight;
        points[1] = jarClicksWrong;
        return points;
    }
}
