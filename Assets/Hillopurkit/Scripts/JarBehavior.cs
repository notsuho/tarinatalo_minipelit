using UnityEngine;

public class JarBehavior : MonoBehaviour
{
    private bool IsCorrectAnswer = false;
    [SerializeField] private GameObject brokenJar;

    public void SetIsCorrectAnswer(bool _isCorrectAnswer)
    {
        IsCorrectAnswer = _isCorrectAnswer;
    }

    // Activates from HammerBehavior when player clicks a jar.
    // Clicking on a correct jar breaks it and starts the next round. Wrong jar only wobbles.
    // Only works when game is unpaused.
    public void HitByHammer()
    {
        if (MiniGameManager.isGamePaused)
            return;

        MiniGameManager miniGameManager = GameObject.Find("MiniGameManager").GetComponent<MiniGameManager>();

        if (IsCorrectAnswer)
        {
            miniGameManager.hammer.GetComponent<HammerBehavior>().SetCanSwing(false); // stop hammer from swinging until next round starts
            miniGameManager.BrokeCorrectJar(true); // update score
            StartCoroutine(miniGameManager.NextRound()); // start next round
            
            // move out of the way and spawn in broken jar
            Vector3 currentPosition = transform.position;
            Instantiate(brokenJar, currentPosition, Quaternion.identity);
            gameObject.transform.position = currentPosition + new Vector3 (30, 0, 0); // NextRound() despawns the jar.
        }
        //
        else
        {
            miniGameManager.BrokeCorrectJar(false); // update score
            GetComponent<Animator>().Play("WrongJar");
        }
    }
}
