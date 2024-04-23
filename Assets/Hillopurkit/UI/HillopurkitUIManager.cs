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
    private Label scoreLabel;
    private Label clickedWrong;
    private Label clickedRight;
    private VisualElement streakImage;
    public Camera cam;
    public SoundObject soundObject;
    public UIUtils uiUtils;
    private const int SCORE_MULTIPLIER = 10; // for UI display purposes
    private readonly string continueButtonText = "<allcaps>jatka</allcaps>";
    private readonly string gotItButtonText = "<allcaps>selvä!</allcaps>";
    private readonly string endGameButtonText = "<allcaps>palaa pääpeliin</allcaps>";
    private readonly string nextGameButtonText = "<allcaps>seuraava peli</allcaps>";
    private readonly string instructionHeadlineText = "<allcaps>ohjeet</allcaps>";
    private readonly string winningHeadline = "Läpäisit pelin!";
    private readonly string winningText = "Löysit ja rikoit kaikki joukkoon kuulumattomat purkit!";

    private void Update()
    {
        //gameScore.text = GameManager.totalPoints.ToString();    
    }

    private void OnEnable()
    {
        cam = Camera.main;

        uiUtils = GetComponent<UIUtils>();

        root = GetComponent<UIDocument>().rootVisualElement;

        Button instructionButton = root.Q<Button>("instruction-button");
        Button exitButton = root.Q<Button>("exit-button");

        panelSection = root.Q<VisualElement>("panel-section");
        panelHeadline = panelSection.Q<Label>("panel-headline");
        panelText = panelSection.Q<Label>("panel-text");
        panelButton = panelSection.Q<Button>("panel-button");

        clickedRight = root.Q<Label>("feedback-right");
        clickedWrong = root.Q<Label>("feedback-wrong");

        scoreLabel = root.Q<Label>("score-label");
        scoreLabel.text = "" + GameManager.totalPoints;

        ResetProgressBar(33);
        SetInstructions();

        instructionButton.clicked += () => SetInstructions();
        panelButton.clicked += () => SetPanelExit();
        exitButton.clicked += () => uiUtils.SetConfirmationPanel(root);
    }

    // <summary>
    // Displays the instruction UI pop-up
    // Clicking on the ? UI button also calls this method
    // </summary>
    private void SetInstructions()
    {
        //Siirrä tiedostoon myöhemmin
        string instructionTextText = "Kaappiin on kasattu purkkeja, joiden kyljessä lukee synonyymejä. " 
                                    + "Mutta purkkien joukkoon on eksynyt sana, joka ei kuulu joukkoon. "
                                    + "Etsi joukkoon kuulumaton purkki, ja riko se vasaralla!";

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

        // Play victory jingle
        AudioSource.PlayClipAtPoint(soundObject.victorySound, cam.transform.position);

        int[] stats = score.GetStats();
        int total = stats[0] + stats[1];

        panelHeadline.text = winningHeadline;
        panelText.text = winningText
            + ("\n\nPisteesi: " + GameManager.totalPoints) // Add two linebreaks so it looks just a tiny bit cleaner
            + ("\nArvausten määrä: " + total)
            + ("\nOikeat arvaukset: " + stats[0])
            + ("\nVäärät arvaukset: " + stats[1]);

        panelButton.text = nextGameButtonText;

        panelSection.style.display = DisplayStyle.Flex;

        // Load next minigame scene when player presses the button
        panelButton.clicked += () =>
        {
            SceneManager.LoadScene("KirjahyllyScene");
        };
    }

    public void UpProgressBar(int currentPoints)
    {
        VisualElement star1 = root.Q<VisualElement>("star1");
        VisualElement star2 = root.Q<VisualElement>("star2");
        VisualElement star3 = root.Q<VisualElement>("star3");

        progressBar = root.Q<ProgressBar>("progress-bar");
        Debug.Log("\nProgress bar value before update: " + progressBar.value);
        progressBar.value = currentPoints;
        Debug.Log("\nProgress bar value after update: " + progressBar.value);

        if (progressBar.value < 66)
        {
            UnlightStar(star2);
        }

        if (progressBar.value < 99)
        {
            UnlightStar(star3);
        }

        if (progressBar.value >= 33)
        {
            star1.style.backgroundImage = Resources.Load<Texture2D>("Images/star");

            if (progressBar.value > 33 && progressBar.value <= 35)
            {
                StarScaleTransition(star1);
            }

        }

        if (progressBar.value >= 66)
        {
            star2.style.backgroundImage = Resources.Load<Texture2D>("Images/star");
            StarScaleTransition(star2);
        }
    }

    private void StarScaleTransition(VisualElement star)
    {
        // Twinkle the star when it's filled
        star.ToggleInClassList("star-scale-transition");
        root.schedule.Execute(() => star.ToggleInClassList("star-scale-transition")).StartingIn(500);

        AudioSource.PlayClipAtPoint(soundObject.starSound, cam.transform.position);
    }

    private void UnlightStar(VisualElement star)
    {
        // Make star go back to blank from yellow
        star.style.backgroundImage = Resources.Load<Texture2D>("Images/star_blank");
    }

    public void ResetProgressBar(int resetValue)
    {
        progressBar = root.Q<ProgressBar>("progress-bar");
        UpProgressBar(resetValue);
    }

    public void SetFeedback(bool result)
    {
        // Get the right/wrong click stats and score
        int[] stats = score.GetStats();
        int points = GameManager.totalPoints;

        // If a breakable jar was clicked, check for streak bonus points and update score UI
        if (result == true) 
        {
            if (GameManager.streak >= score.minStreakValue) {
                DisplayStreakImage();
            }
            scoreLabel = root.Q<Label>("score-label");
            scoreLabel.text = ("" + GameManager.totalPoints);
            clickedRight.visible = true;
            StartCoroutine(FeedbackTurnOffDelay(clickedRight));
        }

        else
        {
            scoreLabel = root.Q<Label>("score-label");
            scoreLabel.text = ("" + points);
            clickedWrong.visible = true;
            StartCoroutine(FeedbackTurnOffDelay(clickedWrong));
        }
    }

    private IEnumerator FeedbackTurnOffDelay(Label feedbackMsg)
    {
        yield return new WaitForSeconds(WaitTimes.MESSAGE_TIME_SHORT);
        feedbackMsg.visible = false;
    }

    //asettaa streak imagen käymään näkyvissä
    private void DisplayStreakImage ()
    {       
            streakImage = root.Q<VisualElement>("streak-image");

            //asettaa kuvaan oikean streakin arvon
            Label streakCount = streakImage.Q<Label>("streak-count");
            streakCount.text = "+" + GameManager.streak;

            streakImage.style.display = DisplayStyle.Flex;
            streakImage.ToggleInClassList("streak-image-transition");
            AudioSource.PlayClipAtPoint(soundObject.streakSound, cam.transform.position);
            Invoke("ToggleStreakClassList", 3f);
    }


    //hävittää streak imagen näkyvistä ja asettaa classlistin alkuperäiseen asentoon
    private void ToggleStreakClassList()
    {
        streakImage.ClearClassList();
        streakImage.style.display = DisplayStyle.None;
    }
}