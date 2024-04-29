using System.Collections;
using UnityEngine;

public class HammerBehavior : MonoBehaviour
{
    private Animator hammerAnimator;
    private bool canSwing = false;


    private void Start()
    {
        hammerAnimator = GetComponent<Animator>();
    }

    // Checks if player clicked on a jar and swings the hammer if they did.
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && canSwing)
        {
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject.name.Equals("JamJar(Clone)"))
                {
                    AnimateHammer("HammerSwing");
                    hit.collider.gameObject.GetComponent<JarBehavior>().HitByHammer();
                }
            }
        }
    }

    public IEnumerator WrongSwing()
    {
        SetCanSwing(false);
        yield return new WaitForSeconds(WaitTimes.MESSAGE_TIME_SHORT);
        SetCanSwing(true);
    }

    // Plays an animation by the given string and despawns hammer after the minigame ends
    public void AnimateHammer(string animationString)
    {
        hammerAnimator.Play(animationString);

        if (string.Equals(animationString, "SlideHammerOutOfView"))
        {
            StartCoroutine(DisappearAfterAWhile());
        }
    }

    public void SetCanSwing(bool _canSwing)
    {
        canSwing = _canSwing;
    }

    private IEnumerator DisappearAfterAWhile()
    {
        yield return new WaitForSeconds(WaitTimes.HAMMER_DISAPPEAR_TIME);
        gameObject.SetActive(false);
    }
}