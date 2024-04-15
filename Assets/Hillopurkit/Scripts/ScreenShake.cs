using System.Collections;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private AnimationCurve animationCurve;
    ///<summary>
    ///Set to true to give the screen a shake
    ///</summary>
    public static bool shakeTrigger = false;

    void Update()
    {
        if (shakeTrigger)
        {
            shakeTrigger = false;
            StartCoroutine(ShakeEffect());
        }
    }

    private IEnumerator ShakeEffect()
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;
        float shakeStrenght;

        while(elapsedTime < shakeDuration)
        {
            elapsedTime += Time.deltaTime;
            shakeStrenght = animationCurve.Evaluate(elapsedTime / shakeDuration);
            transform.position = startPosition + Random.insideUnitSphere * shakeStrenght;
            yield return null;
        }

        transform.position = startPosition;
    }
}
