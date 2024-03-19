using UnityEngine;

public class HammerBehavior : MonoBehaviour
{
    public Animator hammerAnimator;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            hammerAnimator.Play("HammerSwing");
            Debug.Log("left click hammer");
        }
    }
}
