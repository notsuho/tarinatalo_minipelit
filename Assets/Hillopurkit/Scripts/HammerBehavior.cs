using UnityEngine;

public class HammerBehavior : MonoBehaviour
{
    public Animator hammerAnimator;
    private bool swingable = false;
    Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && swingable)
        {
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = cam.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if(hit.collider.gameObject.name.Equals("JamJar(Clone)"))
                    hammerAnimator.Play("HammerSwing");
            }
        }
    }

    public void SetSwingable(bool _swingable)
    {
        swingable = _swingable;
    }
}
