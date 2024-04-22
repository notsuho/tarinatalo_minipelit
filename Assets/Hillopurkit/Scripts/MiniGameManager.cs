using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    public GameObject hammer;
    public SoundObject soundObject;
    public Camera cam;
    [SerializeField] private GameObject jar;
    [SerializeField] private GameObject firstShelf;
    [SerializeField] private GameObject secondShelf;
    [SerializeField] private GameObject jarShakeHelper;
    [SerializeField] private Animator cabinetAnimator;
    [SerializeField] private TextAsset [] synonymsLists;
    [SerializeField] private int numberOfJars_round1 = 4;
    [SerializeField] private int numberOfJars_round2 = 6;
    [SerializeField] private int numberOfJars_round3 = 8;
    private readonly int roundsTotal = 5; // change this for more rounds
    private const int JARS_PER_SHELF = 4;
    private const int MAX_AMOUNT_OF_JARS = JARS_PER_SHELF * 2;
    private int currentRound = 0;
    private readonly List<GameObject> jarsOfTheRound = new();
    private List<List<string>> adjectiveGroups = new();
    private List<List<string>> nounGroups = new();
    private List<List<string>> verbGroups = new();

    public static bool isGamePaused = true;

    private void Start() {
        // Get the camera position for sounds and other things
        cam = Camera.main;

        InitializeWordGroups();
    }

    /// <summary>
    /// Takes synonyms from the .txt-files and prepares them for use as List<List<string>> -objects.
    /// </summary>
    private void InitializeWordGroups()
    {
        for (int wordTypeIndex = 0; wordTypeIndex < synonymsLists.Length; wordTypeIndex++)
        {
            TextAsset wordFile = synonymsLists[wordTypeIndex];

            string[] allWordsOfAGroup = wordFile.text.Split("\n");

            foreach (string wordGroupRaw in allWordsOfAGroup)
            {
                string[] wordGroupTxt = wordGroupRaw.Split("|");

                List<string> wordGroup = new();

                foreach (string word in wordGroupTxt)
                {
                    wordGroup.Add(word);
                }

                switch (wordTypeIndex)
                {
                    case 0:
                        adjectiveGroups.Add(wordGroup);
                        break;
                    case 1:
                        nounGroups.Add(wordGroup);
                        break;
                    case 2:
                        verbGroups.Add(wordGroup);
                        break;
                }
            }
        }
    }

    public int GetTotalRounds() {
        return roundsTotal;
    }
    
    public void PauseGame()
    {
        isGamePaused = true;
        hammer.GetComponent<HammerBehavior>().SetCanSwing(false);
    }

    public void UnpauseGame()
    {
        isGamePaused = false;
        hammer.GetComponent<HammerBehavior>().SetCanSwing(true);

        // This starts the game after tutorial screen.
        if(currentRound == 0)
        {
            StartCoroutine(NextRound());
        }
    }

    /// <summary>
    /// Sets up and starts the next round of jam jar minigame.
    /// </summary>
    public IEnumerator NextRound()
    {
        currentRound++;

        if (currentRound == 1)
            StartFirstRound();

        else
        {
            yield return new WaitForSeconds(WaitTimes.MESSAGE_TIME_LONG);

            cabinetAnimator.Play("CloseCabinetDoors");
            AudioSource.PlayClipAtPoint(soundObject.doorClose, cam.transform.position, 1.0f);
            yield return new WaitForSeconds(WaitTimes.DOOR_CLOSING_TIME); // animation duration

            if (currentRound <= roundsTotal) // rounds 2 and 3
            {
                AudioSource.PlayClipAtPoint(soundObject.cabinetShake, cam.transform.position, 0.7f);
                cabinetAnimator.Play("CabinetShake");
                yield return new WaitForSeconds(WaitTimes.CABINET_SHAKING_TIME); // animation duration

                cabinetAnimator.Play("OpenCabinetDoors");
                AudioSource.PlayClipAtPoint(soundObject.doorOpen, cam.transform.position, 1.0f);

                foreach (GameObject jar in jarsOfTheRound)
                    Destroy(jar);

                jarsOfTheRound.Clear();

                SetUpJars();
            }

            else // After last round
            {
                hammer.GetComponent<HammerBehavior>().AnimateHammer("SlideHammerOutOfView");
                PauseGame();
            }
        }

        if(currentRound != roundsTotal + 1)  // release the hammer if there are more rounds left
            hammer.GetComponent<HammerBehavior>().SetCanSwing(true);
    }

    /// <summary>
    /// Plays beginning animations and starts the first round.
    /// </summary>
    private void StartFirstRound()
    {
        // UI stuff
        GameObject.Find("Score").GetComponent<Score>().ClearScore();

        //Game logic and beginning animations
        SetUpJars();
        hammer.GetComponent<HammerBehavior>().AnimateHammer("SlideHammerIntoView");
        cabinetAnimator.Play("OpenCabinetDoors");
        AudioSource.PlayClipAtPoint(soundObject.doorOpen, cam.transform.position, 1.0f);
        UnpauseGame();
    }

    /// <summary>
    /// Instantiates and sets up a round's jars. 
    /// </summary>
    private void SetUpJars()
    {
        int numberOfJars;

        // How many jars this round
        switch (currentRound)
        {
            case 1:
                numberOfJars = numberOfJars_round1;
                break;
            case 2:
                numberOfJars = numberOfJars_round2;
                break;
            case 3:
                numberOfJars = numberOfJars_round3;
                break;
            default: // After round 3, default to round 3 setups
                // Debug.Log("Only rounds 1, 2 and 3 exist");
                // For later: just default to round 3 jars if there are more
                // than 3 rounds? Or randomize the # of jars?
                numberOfJars = numberOfJars_round3;
                break;
                // To the next minigame
                //return;
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

        LabelJars(jars);
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
        Vector3 firstJarPosition = shelf.transform.position                               // middle of the shelf
                                    - new Vector3(shelfWidth / 2, 0, 0)                   // to the left edge
                                    + spaceBetweenJars                                    // one step from the wall
                                    + new Vector3(0, jarHeight / 2 + shelfHeight / 2, 0); // no sinking into the shelf

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
    private void LabelJars(List<GameObject> jars)
    {
        List<string> wordsOfTheRound = GetWordsForTheRound(jars.Count);

        // Specifically handle the wrong jar
        int wrongIndex = Random.Range(0, jars.Count);
        GameObject wrongJar = jars[wrongIndex];
        jars.RemoveAt(wrongIndex);
        
        int wrongWordindex = wordsOfTheRound.Count - 1; // last index is wrong word
        string label = wordsOfTheRound[wrongWordindex];
        wordsOfTheRound.RemoveAt(wrongWordindex); // prevent same word usage

        wrongJar.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = label;
        wrongJar.GetComponent<JarBehavior>().SetIsCorrectAnswer(true);
        jarsOfTheRound.Add(wrongJar);

        // Handle synonym jars
        foreach (GameObject jar in jars)
        {
            // Set up shake animation helper
            GameObject helper = Instantiate(jarShakeHelper);
            jar.transform.parent = helper.transform;
            jarsOfTheRound.Add(helper);

            int index = Random.Range(0, wordsOfTheRound.Count);
            label = wordsOfTheRound[index];
            wordsOfTheRound.RemoveAt(index); // prevent same word usage

            jar.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = label;
            jar.GetComponent<JarBehavior>().SetIsCorrectAnswer(false);
            jarsOfTheRound.Add(jar);
        }
    }

    /// <summary>
    /// Returns a list with words for the round. Last index has the incorrect answer.
    /// </summary>
    private List<string> GetWordsForTheRound(int wordAmount)
    {
        List<List<string>> wordGroups;
        List<string> wordsOfTheRound = new();
        int wordTypeIndex = Random.Range(0, 3);

        // Make sure there are atleast 2 synyonym groups in every word type
        if (adjectiveGroups.Count < 2 || nounGroups.Count < 2 || verbGroups.Count < 2)
            InitializeWordGroups();

        // Choose word type for the round
        switch (wordTypeIndex)
        {
            case 0:
                wordGroups = adjectiveGroups;
                break;
            case 1:
                wordGroups = nounGroups;
                break;
            default:
                wordGroups = verbGroups;
                break;
        }

        // Pick the correct word group and words
        int wordGroupIndex = Random.Range(0, wordGroups.Count);
        List<string> randomWordGroup = wordGroups[wordGroupIndex];

        for (int i = 0; i < (wordAmount - 1); i++) // minus one leaves space for the wrong word
        {
            if (i == 0) // allways have the root correct word from first index
            {
                wordsOfTheRound.Add(randomWordGroup[0]);
                randomWordGroup.RemoveAt(0);
            }
            else
            {
                int index = Random.Range(0, randomWordGroup.Count);
                wordsOfTheRound.Add(randomWordGroup[index]);
                randomWordGroup.RemoveAt(index);
            }
        }

        // Remove used word group to avoid repeating rounds
        wordGroups.RemoveAt(wordGroupIndex);

        // Pick the wrong word
        List<string> wrongWordGroup = wordGroups[Random.Range(0, wordGroups.Count)];
        string wrongWord = wrongWordGroup[Random.Range(0, wrongWordGroup.Count)];
        wordsOfTheRound.Add(wrongWord);

        // Update word group after the removal
        switch (wordTypeIndex)
        {
            case 0:
                adjectiveGroups = wordGroups;
                break;
            case 1:
                nounGroups = wordGroups;
                break;
            default:
                verbGroups = wordGroups;
                break;
        }

        return wordsOfTheRound;
    }
}
