using UnityEngine;

public class JarBehavior : MonoBehaviour
{
    private bool IsCorrectAnswer = false;
    [SerializeField] private GameObject brokenJar;
    [SerializeField] private GameObject jam;
    private Color color;
    private Camera cam;
    public SoundObject soundObject;

    private void Start()
    {
        cam = Camera.main;
        color = GetComponentInChildren<RandomMaterial>().GetMaterial().color;
    }

    public void SetIsCorrectAnswer(bool _isCorrectAnswer)
    {
        IsCorrectAnswer = _isCorrectAnswer;
    }

    // Activates from HammerBehavior when player clicks a jar.
    // Clicking on a correct jar breaks it and starts the next round. Wrong jar only wobbles.
    public void HitByHammer()
    {
        if (MiniGameManager.isGamePaused)
            return;

        Score score = GameObject.Find("Score").GetComponent<Score>();
        MiniGameManager miniGameManager = GameObject.Find("MiniGameManager").GetComponent<MiniGameManager>();

        if (IsCorrectAnswer)
        {
            miniGameManager.hammer.GetComponent<HammerBehavior>().SetCanSwing(false); // stop hammer from swinging until next round starts
            score.BrokeCorrectJar(true); // update score
            StartCoroutine(miniGameManager.NextRound()); // start next round
            
            // Move out of the way and spawn in broken jar
            Vector3 currentPosition = transform.position;
            gameObject.transform.position = currentPosition + new Vector3 (0, 0, 50);
            GameObject breakingJar = Instantiate(brokenJar, currentPosition, Quaternion.identity);

            // Play sound for clicking correct jar
            AudioSource.PlayClipAtPoint(soundObject.correctAnswerSound, cam.transform.position, 1.0f);

            // Also play shatter sound (uncomment below part when we get clip)
            AudioSource.PlayClipAtPoint(soundObject.jarShatter, cam.transform.position, 1.0f);

            // Screen shake effect
            ScreenShake.shakeTrigger = true;

            // Set the dust cloud effect to be a little lighter than the jam's color
            ParticleSystem ps = breakingJar.GetComponent<BrokenJarBehavior>().dustCloud;
            ParticleSystem.MainModule main = ps.main;
            main.startColor = color + new Color(0.33f, 0.33f, 0.33f);

            // MiniGameManager despawns the jar.
        }

        else
        {
            // Play sounds for clicking incorrect jar
            AudioSource.PlayClipAtPoint(soundObject.jarClink, cam.transform.position, 1.0f);
            AudioSource.PlayClipAtPoint(soundObject.wrongAnswerSound, cam.transform.position, 1.0f);

            StartCoroutine(miniGameManager.hammer.GetComponent<HammerBehavior>().WrongSwing());
            
            score.BrokeCorrectJar(false); // update score

            transform.parent.gameObject.GetComponent<Animator>().Play("WrongJarShake");
            GetComponent<Animator>().Play("WrongJar"); // jar wobble
        }
    }
}