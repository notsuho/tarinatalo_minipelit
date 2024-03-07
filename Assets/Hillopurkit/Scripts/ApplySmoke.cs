using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class applySmoke : MonoBehaviour
{
    // Hoitaa savuefektin alkamisen siinä objektissa,
    // mihin se on kiinnitetty

    public ParticleSystem ps;

    // Start is called before the first frame update
    void Start()
    {
        ps.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
