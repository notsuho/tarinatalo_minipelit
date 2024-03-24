using UnityEngine;

public class JarBehavior : MonoBehaviour
{
    private bool isBreakable = false;
    public GameObject destroyedVersion;

    public void SetBreakability(bool _isBreakable)
    {
        isBreakable = _isBreakable;
    }

    private void OnMouseDown()
    {
        if (MiniGameManager.isGamePaused)
            return;

        if (isBreakable)
        {
            Vector3 currentPosition = transform.position;
            // Update UI/score in MiniGameManager
            GameObject.Find("MiniGameManager").GetComponent<MiniGameManager>().BrokeCorrectJar(true);
            StartCoroutine(GameObject.Find("MiniGameManager").GetComponent<MiniGameManager>().NextRound());
            
            // Jar breaks
            Instantiate(destroyedVersion, currentPosition, Quaternion.identity);
            gameObject.transform.position = currentPosition + new Vector3 (30, 0, 0);
        }

        else
        {
            //Update UI/score in MiniGameManager
            GameObject.Find("MiniGameManager").GetComponent<MiniGameManager>().BrokeCorrectJar(false);
            //ANIM: jar doesn't break
            Animation anim = GetComponent<Animation>();
            anim.Play();
        }
    }
}
