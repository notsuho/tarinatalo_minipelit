using System;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public GameManager gameManager;

    private VisualElement root;
    private Label sentenceLabel;
    private Button leftButton;
    private Button rightButton;
    private VisualElement panelSection;
    private Label panelHeadline;
    private Label panelText;
    private Button panelButton;
    private VisualElement instructions;
    private ProgressBar progressBar;

    private string sentence;
    private string leftWord;
    private string rightWord;

    private string continueButtonText = "<allcaps>jatka</allcaps>";
    private string gotItButtonText = "<allcaps>selvä!</allcaps>";
    private string endGameButtonText = "<allcaps>palaa pääpeliin</allcaps>";
    private string instructionHeadlineText = "<allcaps>ohjeet</allcaps>";
    private string instructionTextText = "Tässä on ohjeet";
    private string winningHeadline = "Läpäisit pelin";
    private string winningText = "Sait sanataiturin arvomerkin<br><br>Pisteesi: 5000";

    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        sentenceLabel = root.Q<Label>("sentence");
        leftButton = root.Q<Button>("left-button");
        rightButton = root.Q<Button>("right-button");

        Button instructionButton = root.Q<Button>("instruction-button");
        Button exitButton = root.Q<Button>("exit-button");

        panelSection = root.Q<VisualElement>("panel-section");
        panelHeadline = panelSection.Q<Label>("panel-headline");
        panelText = panelSection.Q<Label>("panel-text");
        panelButton = panelSection.Q<Button>("panel-button");

        SetInstructions();

        leftButton.clicked += () => gameManager.CheckAnswer(leftWord);
        rightButton.clicked += () => gameManager.CheckAnswer(rightWord);
        instructionButton.clicked += () => SetInstructions();
        panelButton.clicked += () => SetPanelExit();
        exitButton.clicked += () => Application.Quit();
    }

   
    private void Update()
    {
       
        if (sentenceLabel != null) { 
            sentenceLabel.text = sentence;
        } 
     
        if (leftButton != null)
        {
            leftButton.text = leftWord;
        }

        if(rightButton != null)
        {
            rightButton.text = rightWord;
        }
    }

   

    private void SetInstructions ()
    {
        instructions = root.Q<VisualElement>("panel-section");
        Label instructionHeadline = instructions.Q<Label>("panel-headline");
        instructionHeadline.text = instructionHeadlineText;
        Label instructionText = instructions.Q<Label>("panel-text");
        instructionText.text = instructionTextText;
        Button gotItButton = instructions.Q<Button>("panel-button");
        gotItButton.text = gotItButtonText;

        instructions.style.display = DisplayStyle.Flex;
    }

    public void SetSentence(string newSentence)
    {    
        sentence = newSentence;    
    }

    public void SetLeftWord(string newLeftWord)
    {
        leftWord = newLeftWord;
    }

    public void SetRightWord(string newRightWord)
    {
        rightWord = newRightWord;
    }

    public void SetFeedpack(string feedpackFrase, string explanation)
    {
        panelHeadline.text = feedpackFrase;
        panelText.text = explanation;

        panelButton.text = continueButtonText;

        panelSection.style.display = DisplayStyle.Flex;
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

    private void ContinueGame ()
    {
        panelSection.style.display = DisplayStyle.None;
        gameManager.SetCurrentExercise();
    }

    public void DeclareWin () 
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
    }
