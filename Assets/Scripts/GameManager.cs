using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public int points;

    public void SetPoints(int newPoints)
    {
        points = newPoints;
    }

    public int GetPoints()
    {
        return points;
    }
}
