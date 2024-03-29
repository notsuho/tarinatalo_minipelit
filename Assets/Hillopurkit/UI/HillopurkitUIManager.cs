using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class HillopurkitUIManager : MonoBehaviour
{
    public MiniGameManager miniGameManager;
    public Score score;
    private VisualElement root;
    private VisualElement panelSection;
    private Label panelHeadline;
    private Label panelText;
    private Button panelButton;
    private VisualElement instructions;
    private ProgressBar progressBar;
    private Label tallyText;
    private Label pointsText;
    private Label clickedWrong;
    private Label clickedRight;

    private readonly string continueButtonText = "<allcaps>jatka</allcaps>";
    private readonly string gotItButtonText = "<allcaps>selvä!</allcaps>";
    private readonly string endGameButtonText = "<allcaps>palaa pääpeliin</allcaps>";
    private readonly string nextGameButtonText = "<allcaps>seuraava minipeli</allcaps>";
    private readonly string instructionHeadlineText = "<allcaps>ohjeet</allcaps>";
    private readonly string winningHeadline = "Läpäisit pelin!";
    private readonly string winningText = "Löysit ja rikoit kaikki joukkoon kuulumattomat purkit!";

    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        ResetProgressBar(); // We intentionally reset the progress bar at 1/3 full.
                            // In case someone is debugging later and wonders
                            // why it doesn't start at zero. Once we have global
                            // tracking for score this should be changed back!

        Button instructionButton = root.Q<Button>("instruction-button");
        Button exitButton = root.Q<Button>("exit-button");

        panelSection = root.Q<VisualElement>("panel-section");
        panelHeadline = panelSection.Q<Label>("panel-headline");
        panelText = panelSection.Q<Label>("panel-text");
        panelButton = panelSection.Q<Button>("panel-button");

        clickedRight = root.Q<Label>("feedback-right");
        clickedWrong = root.Q<Label>("feedback-wrong");

        SetInstructions();

        instructionButton.clicked += () => SetInstructions();
        panelButton.clicked += () => SetPanelExit();
        exitButton.clicked += () => Application.Quit();
    }

    private void SetInstructions()
    {
        string instructionTextText = "Kaappiin on kasattu hillopurkkeja, joiden kyljessä lukee synonyymejä. " //Siirrä tiedostoon myöhemmin
                                    + "Mutta purkkien joukkoon on eksynyt sana, joka ei kuulu joukkoon. "
                                    + "Etsi joukoon kuulumaton purkki, ja riko se vasaralla!";

        miniGameManager.PauseGame();

        instructions = root.Q<VisualElement>("panel-section");
        Label instructionHeadline = instructions.Q<Label>("panel-headline");
        instructionHeadline.text = instructionHeadlineText;
        Label instructionText = instructions.Q<Label>("panel-text");
        instructionText.text = instructionTextText;
        Button gotItButton = instructions.Q<Button>("panel-button");
        gotItButton.text = gotItButtonText;

        instructions.style.display = DisplayStyle.Flex;
    }

    private void SetPanelExit()
    {
        if (panelButton.text.Equals(continueButtonText)) // onko aina false?
        {
            ContinueGame();
        }

        else if (panelButton.text.Equals(endGameButtonText)) // minipelin lopussa oleva nappi "Palaa pääpeliin"
        {
            Application.Quit();
        }

        else
        {
            instructions.style.display = DisplayStyle.None; // kysymysmerkistä poistuminen, pelin alun "Selvä!"
            miniGameManager.UnpauseGame();
        }
    }

    private void ContinueGame()
    {
        panelSection.style.display = DisplayStyle.None;
    }

    public IEnumerator DeclareWin()
    {
        yield return new WaitForSeconds(WaitTimes.MESSAGE_TIME_LONG);

        int[] tally = score.GetTally();
        int total = tally[0] + tally[1];

        panelHeadline.text = winningHeadline;
        panelText.text = winningText
            + ("\n\nArvausten määrä: " + total) // Add two linebreaks so it looks just a tiny bit cleaner
            + ("\nOikeat arvaukset: " + tally[0])
            + ("\nVäärät arvaukset: " + tally[1]);

        panelButton.text = nextGameButtonText;

        panelSection.style.display = DisplayStyle.Flex;

        // Load next minigame scene when player presses the button
        panelButton.clicked += () =>
        {
            SceneManager.LoadScene("ArkkuScene");
        };
    }

    public void UpProgressBar(float currentPoints)
    {
        progressBar = root.Q<ProgressBar>("progress-bar");
        Debug.Log("\nProgress bar value before update: " + progressBar.value);
        progressBar.value = currentPoints;
        Debug.Log("\nProgress bar value after update: " + progressBar.value);

        if (progressBar.value >= 33f)
        {
            VisualElement star1 = root.Q<VisualElement>("star1");
            star1.style.backgroundImage = Resources.Load<Texture2D>("Images/star");
        }

        if (progressBar.value >= 66f)
        {
            VisualElement star2 = root.Q<VisualElement>("star2");
            star2.style.backgroundImage = Resources.Load<Texture2D>("Images/star");
        }

        if (progressBar.value >= 99f)
        {
            VisualElement star3 = root.Q<VisualElement>("star3");
            star3.style.backgroundImage = Resources.Load<Texture2D>("Images/star");
        }
    }

    public void ResetProgressBar()
    {
        progressBar = root.Q<ProgressBar>("progress-bar");
        // Get the score for reset, in this case it's 33 not 0!
        progressBar.value = GameObject.Find("Score").GetComponent<Score>().GetPoints(); 
        VisualElement star1 = root.Q<VisualElement>("star1");
        star1.style.backgroundImage = Resources.Load<Texture2D>("Images/star"); // light up the first star
        VisualElement star2 = root.Q<VisualElement>("star2");
        star2.style.backgroundImage = Resources.Load<Texture2D>("Images/star_blank");
        VisualElement star3 = root.Q<VisualElement>("star3");
        star3.style.backgroundImage = Resources.Load<Texture2D>("Images/star_blank");
    }

    public void SetFeedback(bool result)
    {
        int[] tally = score.GetTally();
        float points = score.GetPoints();

        if (result == true)
        {
            tallyText = root.Q<Label>("click-tally-right");
            tallyText.text = ("Särjetyt purkit: " + tally[0]);
            pointsText = root.Q<Label>("score");
            pointsText.text = ("Pisteet: " + points);
            clickedRight.visible = true;
            StartCoroutine(FeedbackTurnOffDelay(clickedRight));
        }

        else
        {
            tallyText = root.Q<Label>("click-tally-wrong");
            tallyText.text = ("Väärät arvaukset: " + tally[1]);
            clickedWrong.visible = true;
            StartCoroutine(FeedbackTurnOffDelay(clickedWrong));
        }
    }

    private IEnumerator FeedbackTurnOffDelay(Label feedbackMsg)
    {
        yield return new WaitForSeconds(WaitTimes.MESSAGE_TIME_SHORT);
        feedbackMsg.visible = false;
    }
}