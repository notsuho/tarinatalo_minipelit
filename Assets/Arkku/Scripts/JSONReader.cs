using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JSONReader : MonoBehaviour
{
    public TextAsset textJSON;
    public ExerciseList myExcercises = new ExerciseList();

    // Start is called before the first frame update
    void Start()
    {
        myExcercises = JsonUtility.FromJson<ExerciseList>(textJSON.text);
    }   

    // Update is called once per frame
    void Update()
    {
        
    }
}
