using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public TextAsset textJSON;
    public ExerciseArray myExcercises;

    public UIManager ui;
    public int numberOfExercisesToLoop;

    private static List<Exercise> allExercises;
    private static List<Exercise> exercisesToAnswer;
    private Exercise currentExercise;
    private Exercise previousExercise;


    public int correctAnswers;
    public int correctAnswersToWin;

    public Animator anim;
    public GameObject rightKey;
    public GameObject leftKey;
    public GameObject chest;


    void Start()
    {
   
        anim = gameObject.GetComponent<Animator>();

        //Haetaan harjoitukset JSONista
        myExcercises = JsonUtility.FromJson<ExerciseArray>(textJSON.text);

        //Arvotaan joukosta tietty määrä harjoituksia, joita pyöritetään pelissä
        if (allExercises == null || allExercises.Count == 0)
        {
            allExercises = myExcercises.exercises.ToList<Exercise>();

            exercisesToAnswer = new List<Exercise>();

            for (int i = 0; i < numberOfExercisesToLoop; i++)
            {
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

    public void SetCurrentExercise()
    {
        if (exercisesToAnswer.Count > 0)
        {

            if (currentExercise == null)
            {
                currentExercise = exercisesToAnswer[0];
            }
            else
            {
                while (previousExercise.Equals(currentExercise))
                {
                    int randomExerciseIndex = Random.Range(0, exercisesToAnswer.Count);
                    currentExercise = exercisesToAnswer[randomExerciseIndex];
                }
            }
            ui.SetSentence(currentExercise.sentence);
            ui.SetLeftWord(currentExercise.word1);
            ui.SetRightWord(currentExercise.word2);

        }

    }

    private void OpenChest()
    {
        chest.GetComponent<Animator>().SetTrigger("ArkunKansiAvaus");
    }

    private void CloseChest()
    {
        chest.GetComponent<Animator>().SetTrigger("ArkunKansiSulku");
    }


    public bool IsAnswerCorrect(string answer)
    {
        previousExercise = currentExercise;

        if (answer.Equals(currentExercise.correctAnswer))
        {
            //animaation laukaisu oikealle puolelle
            if (ui.rightWord.Equals(currentExercise.correctAnswer))
            {
                rightKey.GetComponent<Animator>().SetTrigger("OikeaAvainAvaus");
                //AudioSource.PlayClipAtPoint(soundObject.keytwistSound, cam.transform.position);
                //Tehty erillisinä funktioina ajastustoiminnon takia
                Invoke("OpenChest", 1f);
                Invoke("CloseChest", 4f);
            }

            //animaatio vasemmalle puolelle
            if (ui.leftWord.Equals(currentExercise.correctAnswer))
            {
                leftKey.GetComponent<Animator>().SetTrigger("VasenAvainAvaus");
                Invoke("OpenChest", 1f);
                Invoke("CloseChest", 4f);
            }

            GameManager.AddPoints(true, ScoreArkku.pointPerCorrectAnswer);
            Debug.Log("Pelin pisteet: " + GameManager.totalPoints);
            Debug.Log("Streak " + GameManager.streak);
            correctAnswers += 1;
            exercisesToAnswer.Remove(currentExercise);
            return true;

        }
        else
        {
            GameManager.AddPoints(false, ScoreArkku.pointsReduceForWrongAnswer);

            Debug.Log("Pelin pisteet: " + GameManager.totalPoints);

            return false;
        }


    }

    public bool CheckIfGameEnded()
    {
        if (correctAnswers >= correctAnswersToWin)
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }

   /* public float GetProgressBarValue()
    {
        return progressBarValue;
    }*/

    public float GetCorrectAnswersToWinCount()
    {
        return correctAnswersToWin;
    }

    public string GetCurrentExplanation()
    {
        return currentExercise.explanation;
    }
}

