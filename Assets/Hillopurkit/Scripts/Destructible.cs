using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public GameObject destroyedVersion;
    public Vector3 offset = new Vector3(-1.2f, 0, -1.2f);

    void OnMouseDown()
    // luo rikotun version objektista vanhan paikalle klikatessa, ja sitten poistaa vanhan.
    // Koska rikotun version keskipiste on vähän oudossa paikassa johtuen syistä, joista
    // en ole vielä varma, niin lisätään position vektoriin offset arvo, joka saa tuhotun
    // version ilmaantumaan oikeaan kohtaan. Vähän purkkafiksi, mutta toiminee väliaikaisesti,
    // koska purkkien lisäksi ei ole muita tuhottavia objekteja joihin tämä voisi vaikuttaa.
    {
        Instantiate(destroyedVersion, transform.position + offset, transform.rotation);
        Destroy(gameObject);
    }
}
