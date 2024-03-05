using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextAsset textJSON;
    public ExerciseArray myExcercises;

    public UIManager ui;


    private static List<Exercise> unansweredExercises;
    private Exercise currentExercise;

    // Start is called before the first frame update
    void Start()
    {
        myExcercises = JsonUtility.FromJson<ExerciseArray>(textJSON.text);

        if (unansweredExercises == null || unansweredExercises.Count == 0)
        {
            unansweredExercises = myExcercises.exercises.ToList<Exercise>();    
        }

        SetCurrentExercise();
 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCurrentExercise ()
    {
        if (unansweredExercises.Count > 0) { 
            int randomExerciseIndex = Random.Range(0, unansweredExercises.Count);
            currentExercise = unansweredExercises[randomExerciseIndex];

            ui.SetSentence(currentExercise.sentence);
            ui.SetLeftWord(currentExercise.word1);
            ui.SetRightWord(currentExercise.word2);
            unansweredExercises.RemoveAt(randomExerciseIndex);
        }
   
    }

    public void CheckAnswer (string answer)
    {
        if (answer.Equals(currentExercise.correctAnswer)) 
        {
            ui.SetFeedpack("Oikein meni!", currentExercise.explanation);
        }
        else
        {
            ui.SetFeedpack("Nyt ei osunut oikeaan.", currentExercise.explanation);
        }
    }
}
