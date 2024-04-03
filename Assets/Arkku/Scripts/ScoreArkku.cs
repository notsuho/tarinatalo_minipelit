using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreArkku : MonoBehaviour
{
    public static int pointPerCorrectAnswer = 47;
    public static int pointsReduceForWrongAnswer = 30;
    private static int streakOfThreePoints = 20;
    private static int streakOfFourPoints = 50;
    private static int streakOfFivePoints = 98;

    public static int streak = 0;
    public static int minStreakValue = 3;


    public static int GetStreakPoints()
    {
        switch (streak)
        {
            case 3:
                return streakOfThreePoints;              
            case 4:
                return streakOfFourPoints;              
            case 5:
                return streakOfFivePoints;
            default:
                return 0;
              
        }
    }

}
