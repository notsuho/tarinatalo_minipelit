using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class GameManager : MonoBehaviour
{

    public static int totalPoints;
    public static uint streak = 0;

    private static double streakCoefficient = 0.1;


    //cont ilmaisee jatkuuko (true) vai katkeaako (false) streak. 
    //basicPoints on paljonko tehtävästä annetaan oletuksena pisteitä
    //
    public static void AddPoints(bool cont, int basicPoints)
    {
        
        if(cont == true) {
            streak++;
            totalPoints += (int)Math.Round(basicPoints * (1+(streakCoefficient*(streak))));
            
        } else {
            streak = 0;
            totalPoints += basicPoints;
        }
        if (totalPoints <= 0)
            {
                totalPoints = 0;
            }

    }

}
