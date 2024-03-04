using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    [SerializeField] private GameObject jar;
    [SerializeField] private GameObject shelfFirst;
    [SerializeField] private GameObject shelfSecond;
    [SerializeField] private int numberOfJars_round1 = 4;
    [SerializeField] private int numberOfJars_round2 = 6;
    [SerializeField] private int numberOfJars_round3 = 8;
    private const int JARS_PER_SHELF = 4;
    private int currentRound = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Show tutorial text
        // shelfCoordinates1 = Find(...);
        // shelfCoordinates2 = Find(...);
        StartGame();
    }

    public void StartGame()
    {
        int numberOfJars;
        currentRound++;

        /* 1) Calculate locations
         * 2) Get words for labels
         * 3) Spawn jar
         * 4) Set label
         * 5) Repeat for all 3) and 4) for all jars
         */

        if (currentRound == 1)
            numberOfJars = numberOfJars_round1;

        else if (currentRound == 2)
            numberOfJars = numberOfJars_round2;

        else if (currentRound == 3)
            numberOfJars = numberOfJars_round3;

        else
        {
            // To the next mini game
            return;
        }


        // Jar locations
        if (numberOfJars < 0 || numberOfJars > JARS_PER_SHELF * 2)
        {
            Debug.Log("0-" + JARS_PER_SHELF * 2 + " jars, please.");
            return;
        }

        // One shelf
        Debug.Log(shelfFirst.GetComponent<SpriteRenderer>().bounds.size.x);

        if (numberOfJars > JARS_PER_SHELF && numberOfJars <= JARS_PER_SHELF * 2)
        {
            // Two shelvels
        }
    }



}
