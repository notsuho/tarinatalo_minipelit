using System.Collections;
using UnityEngine;

public class ApplySmoke : MonoBehaviour
{
    public ParticleSystem smokeEffect;

    void Start()
    {
        Debug.Log("smoke effect");
        smokeEffect.Play();
        StartCoroutine(DisappearAfterAWhile());
    }

    // Makes the GameObject disappear as a new round begins
    private IEnumerator DisappearAfterAWhile()
    {
        yield return new WaitForSeconds(GameObject.Find("MiniGameManager").GetComponent<MiniGameManager>().GetResettingTime());
        Destroy(gameObject);
    }
}
