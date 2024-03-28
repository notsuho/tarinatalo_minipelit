using UnityEngine;

public class JarBehavior : MonoBehaviour
{
    private bool IsCorrectAnswer = false;
    [SerializeField] private GameObject brokenJar;

    public void SetIsCorrectAnswer(bool _isCorrectAnswer)
    {
        IsCorrectAnswer = _isCorrectAnswer;
    }

    // Clicking on a correct jar breaks it and starts the next round. Wrong jar only wobbles.
    private void OnMouseDown()
    {
        if (MiniGameManager.isGamePaused)
            return;

        if (IsCorrectAnswer)
        {
            Vector3 currentPosition = transform.position;
            GameObject.Find("MiniGameManager").GetComponent<MiniGameManager>().BrokeCorrectJar(true); // update score
            StartCoroutine(GameObject.Find("MiniGameManager").GetComponent<MiniGameManager>().NextRound()); // start next round
            
            Instantiate(brokenJar, currentPosition, Quaternion.identity);
            gameObject.transform.position = currentPosition + new Vector3 (30, 0, 0); // move out of view until NextRound() despawns this jar.
        }
        //
        else
        {
            GameObject.Find("MiniGameManager").GetComponent<MiniGameManager>().BrokeCorrectJar(false); // update score
            GetComponent<Animator>().Play("WrongJar");
        }
    }
}
