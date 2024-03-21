using System;
using UnityEngine;
using UnityEngine.UIElements;

public class HillopurkitUIManager : MonoBehaviour
{

    public MiniGameManager gameManager;
    private VisualElement root;
    private VisualElement panelSection;
    private Label panelHeadline;
    private Label panelText;
    private Button panelButton;
    private VisualElement instructions;
    private ProgressBar progressBar;
    private Label topText;

    private string continueButtonText = "<allcaps>jatka</allcaps>";
    private string gotItButtonText = "<allcaps>selvä!</allcaps>";
    private string endGameButtonText = "<allcaps>palaa pääpeliin</allcaps>";
    private string instructionHeadlineText = "<allcaps>ohjeet</allcaps>";
    private string winningHeadline = "Läpäisit pelin!";
    private string winningText = "Löysit ja rikoit kaikki joukkoon kuulumattomat purkit!";

    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        Button instructionButton = root.Q<Button>("instruction-button");
        Button exitButton = root.Q<Button>("exit-button");

        panelSection = root.Q<VisualElement>("panel-section");
        panelHeadline = panelSection.Q<Label>("panel-headline");
        panelText = panelSection.Q<Label>("panel-text");
        panelButton = panelSection.Q<Button>("panel-button");

        SetInstructions();

        instructionButton.clicked += () => SetInstructions();
        panelButton.clicked += () => SetPanelExit();
        exitButton.clicked += () => Application.Quit();

    }

    private void SetInstructions()
    {

        // Tälleen nyt, lopullisessa varmaan parempi jos luetaan txt tiedostosta tms. ohjeet
        string instructionTextText = "Kaappiin on kasattu hillopurkkeja, joiden kyljessä lukee synonyymejä. "
                                    + "Mutta purkkien joukkoon on eksynyt sana, joka ei kuulu joukkoon. "
                                    + "Etsi joukoon kuulumaton purkki, ja riko se vasaralla!";

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
        if (panelButton.text.Equals(continueButtonText))
        {
            ContinueGame();
        }
        else if (panelButton.text.Equals(endGameButtonText))
        {
            Application.Quit();
        }
        else
        {
            instructions.style.display = DisplayStyle.None;
        }
    }

    private void ContinueGame()
    {
        panelSection.style.display = DisplayStyle.None;
    }

    public void DeclareWin()
    {

        panelHeadline.text = winningHeadline;
        panelText.text = winningText;

        panelButton.text = endGameButtonText;

        panelSection.style.display = DisplayStyle.Flex;
    }

    public void UpProgressBar(float points)
    {
        progressBar = root.Q<ProgressBar>("progress-bar");
        progressBar.value = points;
        Debug.Log("progress bar value: " + progressBar.value);

        if (progressBar.value >= 98)
        {
            VisualElement star3 = root.Q<VisualElement>("star3");
            star3.style.backgroundImage = Resources.Load<Texture2D>("Images/star_yellow");

        }

    }

    public void SetFeedback(bool result)
    {
        topText = root.Q<Label>("instructions");
        if (result == true)
        {
            topText.text = ("Yhdessä hillopurkissa oleva sana ei kuulu joukkoon. Etsi se, ja klikkaa se rikki! \nRIKOIT OIKEAN PURKIN!");
        }
        else
        {
            topText.text = ("Yhdessä hillopurkissa oleva sana ei kuulu joukkoon. Etsi se, ja klikkaa se rikki! \nVÄÄRÄ PURKKI, YRITÄ UUDESTAAN");
        }
    }
}