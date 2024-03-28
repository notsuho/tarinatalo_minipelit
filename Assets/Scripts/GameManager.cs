using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public int points;


    public int GetPoints()
    {
        return points;
    }

    public void AddPoints(int PointsToAdd)
    {
        points += PointsToAdd;
    }

    public void ReducePoints(int pointsToReduce)
    {
        points -= pointsToReduce;
    }
}
