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
    private float points = 66f;
    private float pointsPerCorrectAnswer = 6.66f;
    private float pointsLineForWin = 99f;
    private string correctAnswerFeedpackText = "Oikein meni!";
    private string wrongAnswerFeedpackText = "Nyt ei osunut oikeaan";


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
            points += pointsPerCorrectAnswer;
            ui.UpProgressBar(points);

           
            ui.SetFeedpack(correctAnswerFeedpackText, currentExercise.explanation);
            exercisesToAnswer.Remove(currentExercise);
            
            
        }
        else
        {
            ui.SetFeedpack(wrongAnswerFeedpackText, currentExercise.explanation);
        }

        
    }

    public void CheckIfGameEnded ()
    {
        if (points >= pointsLineForWin)
        {
            ui.Invoke("DeclareWin", 0.7f);
        }
    }
}
