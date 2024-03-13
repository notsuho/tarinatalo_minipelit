using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    [SerializeField] private GameObject jar;
    [SerializeField] private GameObject firstShelf;
    [SerializeField] private GameObject secondShelf;
    [SerializeField] private TextAsset synonymsList;
    [SerializeField] private int numberOfJars_round1 = 4;
    [SerializeField] private int numberOfJars_round2 = 6;
    [SerializeField] private int numberOfJars_round3 = 8;
    [SerializeField] private int numberOfWrongs_round1 = 1;
    [SerializeField] private int numberOfWrongs_round2 = 1;
    [SerializeField] private int numberOfWrongs_round3 = 1;
    [SerializeField] private float resettingTime = 3.0f;
    private const int JARS_PER_SHELF = 4;
    private const int MAX_AMOUNT_OF_JARS = JARS_PER_SHELF * 2;
    private int currentRound = 0;
    private readonly List<GameObject> jarsOfTheRound = new();

    private void Start()
    {
        // Tutorial text box stuff here
        StartRound();
    }

    /// <summary>
    /// Initiates and starts a round of the jar minigame. 
    /// </summary>
    private void StartRound()
    {
        int numberOfJars;
        int numberOfWrongs;

        currentRound++;

        // How many jars this round
        switch (currentRound)
        {
            case 1:
                numberOfJars = numberOfJars_round1;
                numberOfWrongs = numberOfWrongs_round1;
                break;
            case 2:
                numberOfJars = numberOfJars_round2;
                numberOfWrongs = numberOfWrongs_round2;
                break;
            case 3:
                numberOfJars = numberOfJars_round3;
                numberOfWrongs = numberOfWrongs_round3;
                break;
            default:
                Debug.Log("Only rounds 1, 2 and 3 exist");
                // To the next minigame
                return;
        }

        // Make sure there is a proper amount of jars
        if (numberOfJars < 0 || numberOfJars > MAX_AMOUNT_OF_JARS)
        {
            Debug.Log("0-" + MAX_AMOUNT_OF_JARS + " jars, please.");
            return;
        }

        List<GameObject> jars = new();

        // Placing jars to the shelfs
        if (numberOfJars <= JARS_PER_SHELF)
            FillShelf(firstShelf, numberOfJars, jars);
        else
        {
            FillShelf(firstShelf, JARS_PER_SHELF, jars);
            FillShelf(secondShelf, numberOfJars - JARS_PER_SHELF, jars);
        }

        LabelJars(jars, numberOfWrongs);
    }

    /// <summary>
    /// Places a given number of jars onto a shelf. Jars are evenly placed.
    /// </summary>
    /// <param name="jarList">A list of all jars placed. For later use.</param>
    private void FillShelf(GameObject shelf, int numberOfJars, List<GameObject> jarList)
    {
        // Calculates the space between jars.
        float shelfWidth = shelf.GetComponent<MeshRenderer>().bounds.size.x;
        float shelfHeight = shelf.GetComponent<MeshRenderer>().bounds.size.y;
        float jarHeight = jar.GetComponent<MeshRenderer>().bounds.size.y;
        float jarWidth = jar.GetComponent<MeshRenderer>().bounds.size.z;
        Vector3 spaceBetweenJars = new(shelfWidth / (numberOfJars + 1), 0, 0);

        // Calculates the first jar's position (left to right).
        Vector3 firstJarPosition = shelf.transform.position                                                     // middle of the shelf
                                    - new Vector3(shelfWidth / 2, 0, 0)                                         // to the left edge
                                    + spaceBetweenJars                                                          // one step from the wall
                                    + new Vector3(0, jarHeight / 2 + shelfHeight / 2, 0);                       // no sinking into the shelf

        // Places jars on the shelf
        for (int i = 0; i < numberOfJars; i++)
        {
            float randomOffset_z = Random.Range(-jarWidth / 5, jarWidth / 5); // A small offset on the z axis. Brings a little life.
            float randomOffset_x = Random.Range(-jarWidth / 8, jarWidth / 8); // An even small offset on the x axis. Brings a little life.

            Vector3 jarPosition = firstJarPosition + spaceBetweenJars * i + new Vector3(randomOffset_x, 0, randomOffset_z);
            
            jarList.Add(Instantiate(jar, jarPosition, Quaternion.identity));
        }
    }

    /// <summary>
    /// Picks a round's words and writes them on the jars.
    /// </summary>
    private void LabelJars(List<GameObject> jars, int WrongJarAmount)
    {
        List<string> wordsOfTheRound = new();

        string[] allSynonyms = synonymsList.text.Split("\n");
        List<int> usedGroupIndexes = new();

        CheckForEmptyGroups(allSynonyms, usedGroupIndexes);

        int correctGroupNumber = ChooseSynonymGroup(allSynonyms, usedGroupIndexes);

        // Pick the wrong words.
        for (int i = 0; i < WrongJarAmount; i++)
        {
            int wrongGroupIndex = ChooseSynonymGroup(allSynonyms, usedGroupIndexes);

            string[] wrongGroup = allSynonyms[wrongGroupIndex].Split("|");

            wordsOfTheRound.Add(wrongGroup[Random.Range(0, wrongGroup.Length)]);
        }

        // Convert synonym group from string[] to List<string>.
        string[] allCorrectSynonyms = allSynonyms[correctGroupNumber].Split("|");
        List<string> correctWords = new();
        for (int i = 0; i < allCorrectSynonyms.Length; i++)
            correctWords.Add(allCorrectSynonyms[i]);

        // Pick rest of the words.
        for (int i = 0; i < jars.Count - WrongJarAmount; i++)
        {
            int index = Random.Range(0, correctWords.Count);
            
            wordsOfTheRound.Add(correctWords[index]);

            correctWords.RemoveAt(index); // prevent same word usage
        }

        // Puts the words of the round on the jars randomly.
        int alreadyBreakable = 0;

        foreach (GameObject jar in jars)
        {
            bool isBreakable = false;
            int index = Random.Range(0, wordsOfTheRound.Count);
            string label = wordsOfTheRound[index];
            wordsOfTheRound.RemoveAt(index); // prevent same word usage

            if(index < WrongJarAmount - alreadyBreakable)
            {
                isBreakable = true;
                alreadyBreakable++;
            }

            jar.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = label;
            jar.GetComponent<JarBehavior>().SetBreakability(isBreakable);

            jarsOfTheRound.Add(jar);
        }
    }

    /// <summary>
    /// Checks for empty synonym groups in the synonyymit.txt file. Blacklists empty groups and prints what line they are on.
    /// </summary>
    private void CheckForEmptyGroups(string[] allSynonyms, List<int> usedGroupIndexes)
    {
        bool firstTime = true;
        for (int i = 0; i < allSynonyms.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(allSynonyms[i]))
            {
                if(firstTime)
                {
                    print("There were one or more empty lines in synonyymit.txt. Please erase them.");
                    firstTime = false;
                }
                print("empty line: " + (i + 1));
                usedGroupIndexes.Add(i);
            }
        }
    }

    /// <summary>
    /// Picks a synonym group that isn't being used this round.
    /// </summary>
    /// <returns>A free group index.</returns>
    private int ChooseSynonymGroup(string[] allSynonyms, List<int> usedGroupIndexes)
    {
        int groupIndex = Random.Range(0, allSynonyms.Length);

        while (usedGroupIndexes.Contains(groupIndex))
            groupIndex = Random.Range(0, allSynonyms.Length);

        usedGroupIndexes.Add(groupIndex);

        return groupIndex;
    }

    /// <summary>
    /// Sets up and starts the next round of jam jar minigame.
    /// </summary>
    public IEnumerator NextRound()
    {
        yield return new WaitForSeconds(GetResettingTime());
        //(jar breaking animation done in JarBehavior)
        //ANIM: congratulation message
        //ANIM: door close
        //ANIM: camera backs up

        foreach (GameObject jar in jarsOfTheRound)
            Destroy(jar);

        jarsOfTheRound.Clear();

        StartRound();
        //ANIM: camera back into play position
        //ANIM: doors open
    }

    public float GetResettingTime()
    {
        return resettingTime;
    }
}
