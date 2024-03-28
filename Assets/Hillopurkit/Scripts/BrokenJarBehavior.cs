using System.Collections;
using UnityEngine;

public class BrokenJarBehavior : MonoBehaviour
{
    [SerializeField] private ParticleSystem dustCloud;
    [SerializeField] private float explosionForce = 1000f;

    // Play explosion effect when spawned
    void Start()
    {
        dustCloud.Play();

        Transform[] shards = GetComponentsInChildren<Transform>();

        //Apply explosion force away from object's center.
        foreach(Transform shard in shards)
        {
            if (shard.gameObject.GetComponent<Rigidbody>() != null)
            {
                Vector3 forceAmount = explosionForce * Time.deltaTime * ((shard.position - transform.position).normalized
                                                                        + new Vector3(Random.Range(-0.33f, 0.33f), Random.Range(-0.33f, 0.33f), Random.Range(-0.33f, 0.33f)));

                shard.GetComponent<Rigidbody>().AddForce(forceAmount, ForceMode.Impulse);
            }
        }

        StartCoroutine(DisappearAfterAWhile());
    }

    // Makes the GameObject disappear at as a new round begins.
    private IEnumerator DisappearAfterAWhile()
    {
        yield return new WaitForSeconds(WaitTimes.CONGRATULATION_TIME + WaitTimes.DOOR_CLOSING_TIME);
        Destroy(gameObject);
    }
}