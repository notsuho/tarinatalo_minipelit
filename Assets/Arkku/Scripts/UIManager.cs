using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public LevelManager levelManager;
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
    private Label gameScore;
    private VisualElement streakImage;

    private string sentence;

    //public koska tarvitaan GameManagerissam,
    //TODO: tee getterit
    public string leftWord;
    public string rightWord;

    private string continueButtonText = "<allcaps>jatka</allcaps>";
    private string gotItButtonText = "<allcaps>selvä!</allcaps>";
    private string endGameButtonText = "<allcaps>palaa pääpeliin</allcaps>";
    private string instructionHeadlineText = "<allcaps>Avaa arkku oikealla avaimella</allcaps>";
    private string instructionTextText = "Jotkin sanat voivat muistuttaa toisiaan mutta tarkoittaa silti eri asiaa.<br><br>Päättele, kumpi annetuista sanoista sopii lauseeseen. Valitse oikea avain ja arkku aukeaa!  ";
    private string winningHeadline = "Läpäisit pelin";
    private string winningText = "Sait sanataiturin arvomerkin<br><br>Pisteesi: 5000";
    private string correctAnswerFeedpackText = "Oikein meni!";
    private string wrongAnswerFeedpackText = "Nyt ei osunut oikeaan";

    void Start()
    {
        progressBar = root.Q<ProgressBar>("progress-bar");
        progressBar.value = levelManager.GetProgressBarValue();

        VisualElement star1 = root.Q<VisualElement>("star1");
        star1.style.backgroundImage = Resources.Load<Texture2D>("Images/star");

        VisualElement star2 = root.Q<VisualElement>("star2");
        star2.style.backgroundImage = Resources.Load<Texture2D>("Images/star");
    }
    private void OnEnable()
    {
        //testissä

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

        gameScore = root.Q<Label>("score-label");

        SetInstructions();

        leftButton.clicked += () => CheckAnswer(leftWord);
        rightButton.clicked += () => CheckAnswer(rightWord);
        instructionButton.clicked += () => SetInstructions();
        panelButton.clicked += () => SetPanelExit();
        exitButton.clicked += () => Application.Quit();
    }


    private void Update()
    {

        if (sentenceLabel != null)
        {
            sentenceLabel.text = sentence;
        }

        if (leftButton != null)
        {
            leftButton.text = leftWord;
        }

        if (rightButton != null)
        {
            rightButton.text = rightWord;
        }

        if (gameScore != null)
        {
            if (GameManager.totalPoints >= 0)
            {
                gameScore.text = GameManager.totalPoints.ToString();
            }
            else
            {
                gameScore.text = "0";
            }
        }

    }



    private void SetInstructions()
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
        bool gameEnded = levelManager.CheckIfGameEnded();
        UpProgressBar(levelManager.GetProgressBarValue(), levelManager.GetProgBarValueToWin());

        if (gameEnded)
        {
            Invoke("DeclareWin", RenderTimeForDeclareWinFeedpack);
        }
        else
        {
            levelManager.SetCurrentExercise();
        }
    }

    //FeedpackPanel asetaan täällä, viive oikeassa vastauksessa arkun animaatiota varten
    private void CheckAnswer(string answer)
    {

        if (levelManager.IsAnswerCorrect(answer))
        {
           //asetetaan streak-kuvake, jos streak-arvo on tarpeeksi suuri 
           if (ScoreArkku.streak >= ScoreArkku.minStreakValue)
            {
                DisplayStreakImage();
            }
            SetFeedpack(correctAnswerFeedpackText, levelManager.GetCurrentExplanation());
            Invoke("SetFeedpackPanelVisible", RenderTimeForCorrectAnswerFeedpack);
        }
        else
        {
            SetFeedpack(wrongAnswerFeedpackText, levelManager.GetCurrentExplanation());
            SetFeedpackPanelVisible();
        }
    }

    //IMPLEMENTOI STREAKIT: Ota tämä funktio
    //asettaa streak imagen käymään näkyvissä
    private void DisplayStreakImage ()
    {       
        streakImage = root.Q<VisualElement>("streak-image");

        //asettaa kuvaan oikean streakin arvon
        Label streakCount = streakImage.Q<Label>("streak-count");
        streakCount.text = "+" + ScoreArkku.streak;

        streakImage.style.display = DisplayStyle.Flex;
        streakImage.ToggleInClassList("streak-image-transition");
        Invoke("ToggleStreakClassList", 3f);
       
    }


    //hävittää streak imagen näkyvistä ja asettaa classlistin alkuperäiseen asentoon
    private void ToggleStreakClassList()
    {
        streakImage.ClearClassList();
        streakImage.style.display = DisplayStyle.None;
    }

    public void DeclareWin()
    {

        panelHeadline.text = winningHeadline;
        panelText.text = winningText;

        panelButton.text = endGameButtonText;

        panelSection.style.display = DisplayStyle.Flex;
    }

    public void UpProgressBar(float newProgressBarValue, float progBarValueToWin)
    {
        progressBar.value = newProgressBarValue;
        Debug.Log("progress bar value: " + progressBar.value);

        if (progressBar.value >= progBarValueToWin)
        {
            VisualElement star3 = root.Q<VisualElement>("star3");
            star3.style.backgroundImage = Resources.Load<Texture2D>("Images/star");

            //tähti suurenee ja pienenee    
            star3.ToggleInClassList("star-scale-transition");
            root.schedule.Execute(() => star3.ToggleInClassList("star-scale-transition")).StartingIn(500);

        }

    }
}
