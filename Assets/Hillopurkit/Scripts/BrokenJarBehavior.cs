using System.Collections;
using UnityEngine;

public class BrokenJarBehavior : MonoBehaviour
{
    private readonly float explosionForce = 1200f;
    public ParticleSystem dustCloud;

    void Start()
    {
        dustCloud.Play();

        Transform[] shards = GetComponentsInChildren<Transform>();

        // Apply explosion force to all the shard in he opposite direction of the jar's center.
        foreach(Transform shard in shards)
        {
            if (shard.GetComponent<Rigidbody>() != null)
            {
                Vector3 forceAmount = explosionForce * Time.deltaTime * ((shard.position - (transform.position - new Vector3 (0f, 1f, 0f))).normalized
                                                                        + new Vector3(Random.Range(-0.33f, 0.33f), Random.Range(-0.33f, 0.33f), Random.Range(-0.33f, 0.33f)));

                // Add force specifically to the cap so it flies out of the shelf and won't clip with the doors.
                if (shard.name.Equals("BottleCap"))
                {
                    forceAmount += new Vector3(0f, 0f, -10f);
                }

                shard.GetComponent<Rigidbody>().AddForce(forceAmount, ForceMode.Impulse);
                shard.GetComponent<Rigidbody>().AddTorque(forceAmount, ForceMode.Impulse);
            }
        }

        StartCoroutine(DisappearAfterAWhile());
    }

    // Makes the GameObject disappear as a new round begins.
    private IEnumerator DisappearAfterAWhile()
    {
        yield return new WaitForSeconds(WaitTimes.MESSAGE_TIME_LONG + WaitTimes.DOOR_CLOSING_TIME);
        Destroy(gameObject);
    }
}