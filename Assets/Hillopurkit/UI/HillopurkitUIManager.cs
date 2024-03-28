using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

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
    private Label clickedWrong;
    private Label clickedRight;

    private readonly string continueButtonText = "<allcaps>jatka</allcaps>";
    private readonly string gotItButtonText = "<allcaps>selvä!</allcaps>";
    private readonly string endGameButtonText = "<allcaps>palaa pääpeliin</allcaps>";
    private readonly string instructionHeadlineText = "<allcaps>ohjeet</allcaps>";
    private readonly string winningHeadline = "Läpäisit pelin!";
    private readonly string winningText = "Löysit ja rikoit kaikki joukkoon kuulumattomat purkit!";

    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        ResetProgressBar();

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
        yield return new WaitForSeconds(WaitTimes.CONGRATULATION_TIME);

        int[] tally = score.GetTally();
        int total = tally[0] + tally[1];

        panelHeadline.text = winningHeadline;
        panelText.text = winningText
            + ("\nArvausten määrä: " + total)
            + ("\nOikeat arvaukset: " + tally[0])
            + ("\nVäärät arvaukset: " + tally[1]);

        panelButton.text = endGameButtonText;

        panelSection.style.display = DisplayStyle.Flex;
    }

    public void UpProgressBar(float points)
    {
        progressBar = root.Q<ProgressBar>("progress-bar");
        progressBar.value = points;

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

        if (progressBar.value >= 98f)
        {
            VisualElement star3 = root.Q<VisualElement>("star3");
            star3.style.backgroundImage = Resources.Load<Texture2D>("Images/star");
        }
    }

    public void ResetProgressBar()
    {
        progressBar = root.Q<ProgressBar>("progress-bar");
        progressBar.value = 0;
        VisualElement star1 = root.Q<VisualElement>("star1");
        star1.style.backgroundImage = Resources.Load<Texture2D>("Images/star_blank");
        VisualElement star2 = root.Q<VisualElement>("star2");
        star2.style.backgroundImage = Resources.Load<Texture2D>("Images/star_blank");
        VisualElement star3 = root.Q<VisualElement>("star3");
        star3.style.backgroundImage = Resources.Load<Texture2D>("Images/star_blank");
    }

    public void SetFeedback(bool result)
    {
        int[] tally = score.GetTally();

        if (result == true)
        {
            tallyText = root.Q<Label>("click-tally-right");
            tallyText.text = ("Särjetyt purkit: " + tally[0]);
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
        yield return new WaitForSeconds(WaitTimes.CONGRATULATION_TIME);
        feedbackMsg.visible = false;
    }
}