using UnityEngine;

public class JarBehavior : MonoBehaviour
{
    private bool IsCorrectAnswer = false;
    [SerializeField] private GameObject brokenJar;

    public void SetIsCorrectAnswer(bool _isCorrectAnswer)
    {
        IsCorrectAnswer = _isCorrectAnswer;
    }

    // Clicking on a correct jar breaks it and starts the next round. Wrong jar only wobbles. Only works when game is unpaused.
    private void OnMouseDown()
    {
        if (MiniGameManager.isGamePaused)
            return;

        MiniGameManager miniGameManager = GameObject.Find("MiniGameManager").GetComponent<MiniGameManager>();

        if (IsCorrectAnswer)
        {
            Vector3 currentPosition = transform.position;
            miniGameManager.BrokeCorrectJar(true); // update score
            StartCoroutine(miniGameManager.NextRound()); // start next round
            
            Instantiate(brokenJar, currentPosition, Quaternion.identity);
            gameObject.transform.position = currentPosition + new Vector3 (30, 0, 0); // move out of view until NextRound() despawns this jar.
        }
        //
        else
        {
            miniGameManager.BrokeCorrectJar(false); // update score
            GetComponent<Animator>().Play("WrongJar");
        }
    }
}
