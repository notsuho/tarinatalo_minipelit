using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangeLabelText : MonoBehaviour
{
    public TMP_Text text;
    public GameObject jar;
    public GameObject jam;
    public Material red;
    public Material purple;
    public Material blue;
    public String[] test = null;


    // Start is called before the first frame update
    void Start()
    {
        // Nimietikettilistan alustaminen
        test = new String[] { "ryökäle", "voro", "marakatti", "hemuli", "haisuli", "kurkkusalaatti" };

        // Haetaan käsiteltävän purkin tarvittava tekstikomponentti
        // myöhempää käyttöä varten
        text = jar.GetComponentInChildren<TMP_Text>();
        jam = jar.transform.GetChild(0).gameObject;

        // Taulukon testiprinttaus debug logiin
        //for (int i = 0; i < test.Length; i++)
        //{
        //    Debug.Log("test array: " + test[i]);
        //}

    }

    void OnMouseDown()
    {
        SwapLabelText();
        SwapJamColor();
    }

    void SwapLabelText()
    {
        // Vaihtaa etiketissä olevan tekstin toiseki.
        // Teksti valitaan satunnaisesti string taulukosta,
        // joka korvaa etiketissä olleen aikaisemman tekstin.
        var rnd = new System.Random();
        int index = rnd.Next(0, test.Length);
        Debug.Log("index: " + index);
        text.text = test[index];
    }

    void SwapJamColor()
    {
        var rnd = new System.Random();
        int i = rnd.Next(0, 2);
        if (i == 0) {
        jam.GetComponent<MeshRenderer>().material = blue;
        }
        if (i == 1) {
        jam.GetComponent<MeshRenderer>().material = purple;
        }
        if (i == 2) {
        jam.GetComponent<MeshRenderer>().material = red;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
