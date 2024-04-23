using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class GameManager : MonoBehaviour
{

    public static int totalPoints;
    public static int streak = 0;


    //cont ilmaisee jatkuuko (true) vai katkeaako (false) streak. 
    //basicPoints on paljonko tehtävästä annetaan oletuksena pisteitä
    //
    public static void AddPoints(bool cont, int basicPoints)
    {
        if(cont == true) {
            streak++;
            totalPoints += (int)Math.Round(basicPoints * (1+(0.1*(streak)))) ;
        } else {
            streak = 0;
            totalPoints += basicPoints;
        }

    }

}
