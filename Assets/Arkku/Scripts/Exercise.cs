using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Exercise
{
    public string sentence;
    public string word1;
    public string word2;
    public string correctAnswer;
    public string explanation;
}


[System.Serializable]
public class ExerciseArray
{
    public Exercise[] exercises;
}