using System.Collections;
using UnityEngine;

public class BrokenJarBehavior : MonoBehaviour
{
    public ParticleSystem dustCloud;
    private readonly float explosionForce = 1000f;

    void Start()
    {
        dustCloud.Play();

        Transform[] shards = GetComponentsInChildren<Transform>();

        foreach(Transform shard in shards)
        {
            if (shard.gameObject.GetComponent<Rigidbody>() != null)
            {
                Vector3 forceAmount = explosionForce * Time.deltaTime * ((shard.position - transform.position).normalized
                                                                        + new Vector3(Random.Range(-0.33f, 0.33f), Random.Range(-0.33f, 0.33f), Random.Range(-0.33f, 0.33f)));

                shard.GetComponent<Rigidbody>().AddForce(forceAmount, ForceMode.Impulse);

                print(forceAmount);
            }
        }

        StartCoroutine(DisappearAfterAWhile());
    }

    // Makes the GameObject disappear as a new round begins
    private IEnumerator DisappearAfterAWhile()
    {
        yield return new WaitForSeconds(WaitTimes.CONGRATULATION_TIME + WaitTimes.DOOR_CLOSING_TIME);
        Destroy(gameObject);
    }
}