using UnityEngine;

public class JarBehavior : MonoBehaviour
{
    private bool isBreakable = false;
    public GameObject destroyedVersion;
    public Vector3 SpawningOffset = new(-1.2f, 0, -1.2f);

    public void SetBreakability(bool _isBreakable)
    {
        isBreakable = _isBreakable;
    }

    private void OnMouseDown()
    {
        if (isBreakable)
        {
            Vector3 currentPosition = transform.position;
            Debug.Log("Correct!" + " (You broke: " + gameObject.name + ")");
            StartCoroutine(GameObject.Find("MiniGameManager").GetComponent<MiniGameManager>().NextRound());
            
            // Jar breaks
            Instantiate(destroyedVersion, currentPosition + SpawningOffset, Quaternion.identity);
            gameObject.transform.position = currentPosition + new Vector3 (30, 0, 0);
        }

        else
        {
            Debug.Log("Wrong.");
            //ANIM: jar doesn't break
            var anim = GetComponent<Animation>();
            anim.Play();
        }
    }
}
