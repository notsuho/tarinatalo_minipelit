using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public GameManager gameManager;
    public float RenderTimeForCorrectAnswerFeedpack;
    public float RenderTimeForDeclareWinFeedpack;


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

    //public koska tarvitaan GameManagerissam,
    //TODO: tee getterit
    public string leftWord;
    public string rightWord;

    private string continueButtonText = "<allcaps>jatka</allcaps>";
    private string gotItButtonText = "<allcaps>selvä!</allcaps>";
    private string endGameButtonText = "<allcaps>palaa pääpeliin</allcaps>";
    private string instructionHeadlineText = "<allcaps>ohjeet</allcaps>";
    private string instructionTextText = "Tässä on ohjeet";
    private string winningHeadline = "Läpäisit pelin";
    private string winningText = "Sait sanataiturin arvomerkin<br><br>Pisteesi: 5000";
    private string correctAnswerFeedpackText = "Oikein meni!";
    private string wrongAnswerFeedpackText = "Nyt ei osunut oikeaan";

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

        leftButton.clicked += () => CheckAnswer(leftWord);
        rightButton.clicked += () => CheckAnswer(rightWord);
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
   
    }

    private void SetFeedpackPanelVisible()
    {
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

   private void ContinueGame()
    {
        panelSection.style.display = DisplayStyle.None;
        bool gameEnded = gameManager.CheckIfGameEnded();
        UpProgressBar(gameManager.GetPoints(), gameManager.GetPointsToWin());

        if (gameEnded)
        {
            Invoke("DeclareWin", RenderTimeForDeclareWinFeedpack);
        }
        else
        {
            gameManager.SetCurrentExercise();
        }
    }

    //FeedpackPanel asetaan täällä, viive oikeassa vastauksessa arkun animaatiota varten
    private void CheckAnswer(string answer)
    {

        if (gameManager.IsAnswerCorrect(answer))
        {
            SetFeedpack(correctAnswerFeedpackText, gameManager.GetCurrentExplanation());
            Invoke("SetFeedpackPanelVisible", RenderTimeForCorrectAnswerFeedpack); 
        }
        else
        {
            SetFeedpack(wrongAnswerFeedpackText, gameManager.GetCurrentExplanation());
            SetFeedpackPanelVisible();
        }
    }

    public void DeclareWin()
    {

        panelHeadline.text = winningHeadline;
        panelText.text = winningText;

        panelButton.text = endGameButtonText;

        panelSection.style.display = DisplayStyle.Flex;
    }

    public void UpProgressBar(float points, float pointsToWin)
    {
        progressBar = root.Q<ProgressBar>("progress-bar");
        progressBar.value = points;
        Debug.Log("progress bar value: " + progressBar.value);

        if (progressBar.value >= pointsToWin)
        {
            VisualElement star3 = root.Q<VisualElement>("star3");
            star3.style.backgroundImage = Resources.Load<Texture2D>("Images/star");

            //tähti suurenee ja pienenee    
            star3.ToggleInClassList("star-scale-transition");
            root.schedule.Execute(() => star3.ToggleInClassList("star-scale-transition")).StartingIn(500);
 
        }

    }
    }
