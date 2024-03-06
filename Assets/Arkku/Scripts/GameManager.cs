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
    public int numberOfExercisesToLoop;
    public int numberOfCorrectAnswersNeeded;


    private static List<Exercise> allExercises;
    private static List<Exercise> exercisesToAnswer; 
    private Exercise currentExercise;
    private Exercise previousExercise;
    private int correctAnswers = 0;

    void Start()
    {
        //Haetaan harjoitukset JSONista
        myExcercises = JsonUtility.FromJson<ExerciseArray>(textJSON.text);

        //Arvotaan joukosta tietty määrä harjoituksia, joita pyöritetään pelissä
        if (allExercises == null || allExercises.Count == 0)
        {
            allExercises = myExcercises.exercises.ToList<Exercise>();  

            exercisesToAnswer = new List<Exercise>();

            for(int i = 0; i < numberOfExercisesToLoop; i++) {
                int randomExerciseIndex = Random.Range(0, allExercises.Count);
                exercisesToAnswer.Add(allExercises[randomExerciseIndex]);
                allExercises.RemoveAt(randomExerciseIndex);
            }  
        }
        SetCurrentExercise();
 
    }

    void Update()
    {
        
    }

    public void SetCurrentExercise ()
    {
        if (exercisesToAnswer.Count > 0) { 
            
            if(currentExercise == null) {
                currentExercise = exercisesToAnswer[0];
            } 
            else 
            {
                while (previousExercise.Equals(currentExercise)) {
                int randomExerciseIndex = Random.Range(0, exercisesToAnswer.Count);
                currentExercise = exercisesToAnswer[randomExerciseIndex];
                }
            }
            ui.SetSentence(currentExercise.sentence);
            ui.SetLeftWord(currentExercise.word1);
            ui.SetRightWord(currentExercise.word2);
            
        }
   
    }

    public void CheckAnswer (string answer)
    {
        previousExercise = currentExercise;

        if (answer.Equals(currentExercise.correctAnswer)) 
        {
            ui.SetFeedpack("Oikein meni!", currentExercise.explanation);
            exercisesToAnswer.Remove(currentExercise);
            correctAnswers ++;
        }
        else
        {
            ui.SetFeedpack("Nyt ei osunut oikeaan.", currentExercise.explanation);
        }

        if(correctAnswers == numberOfCorrectAnswersNeeded) {
            ui.DeclareWin();
        }
    }
}
