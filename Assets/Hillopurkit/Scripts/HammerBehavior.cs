using UnityEngine;

public class HammerBehavior : MonoBehaviour
{
    public Animator hammerAnimator;
    private bool swingable = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && swingable)
        {
            hammerAnimator.Play("HammerSwing");
        }
    }

    public void SetSwingable(bool _swingable)
    {
        swingable = _swingable;
    }
}
