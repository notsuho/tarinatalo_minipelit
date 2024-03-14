using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTest : MonoBehaviour
{

    private Vector3 mousePos;
    private Vector3 objectPos;
    public GameObject prefab;
    public GameObject jam;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {

            mousePos = Input.mousePosition;
            mousePos.z = 15.0f;
            objectPos = Camera.main.ScreenToWorldPoint(mousePos);
            Instantiate(prefab, objectPos, Quaternion.identity);
        }
    }
}
