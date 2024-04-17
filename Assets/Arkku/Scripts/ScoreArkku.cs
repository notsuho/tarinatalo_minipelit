using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreArkku : MonoBehaviour
{
    public static int pointPerCorrectAnswer = 47;
    public static int pointsReduceForWrongAnswer = 30;
    private static int pointsForStreakOfThree = 20;
    private static int pointsForStreakOfFour = 50;
    private static int pointsForStreakOfFive = 98;

    public static int streak = 0;
    public static int minStreakValue = 3;

    public static int GetStreakPoints()
    {
        switch (streak)
        {
            case 3:
                return pointsForStreakOfThree;              
            case 4:
                return pointsForStreakOfFour;              
            case 5:
                return pointsForStreakOfFive;
            default:
                return 0;
              
        }
    }

}
