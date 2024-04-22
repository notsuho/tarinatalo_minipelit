using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public LevelManager levelManager;
    public UIUtils uiUtils;
    public float RenderTimeForCorrectAnswerFeedpack;
    public float RenderTimeForDeclareWinFeedpack;

    public Camera cam;

    public SoundObject soundObject;

    private VisualElement root;
    private VisualElement gameIntro;
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
    private Image answerImage;


    private string sentence;


    //public koska tarvitaan GameManagerissam,
    //TODO: tee getterit
    public string leftWord;
    public string rightWord;


    void Start()
    {
        uiUtils = GetComponent<UIUtils>();

        progressBar = root.Q<ProgressBar>("progress-bar");
        progressBar.value = levelManager.GetProgressBarValue();
/*
        VisualElement star1 = root.Q<VisualElement>("star1");
        star1.style.backgroundImage = Resources.Load<Texture2D>("Images/star");

        VisualElement star2 = root.Q<VisualElement>("star2");
        star2.style.backgroundImage = Resources.Load<Texture2D>("Images/star");*/

        
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

        SetGameIntro();
        /*SetLevelInstructions();*/

        leftButton.clicked += () => CheckAnswer(leftWord);
        rightButton.clicked += () => CheckAnswer(rightWord);
        instructionButton.clicked += () => SetLevelInstructions();
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

    private void SetGameIntro()
    {
        gameIntro = root.Q<VisualElement>("panel-section");
        Label gameIntroHeadline = gameIntro.Q<Label>("panel-headline");
        gameIntroHeadline.text = TextMaterialArkku.gameIntroHeadlineText;
        Label gameIntroText = gameIntro.Q<Label>("panel-text");
        gameIntroText.text = TextMaterialArkku.gameIntroText;
        Button letsPlayButton = gameIntro.Q<Button>("panel-button");
        letsPlayButton.text = TextMaterialArkku.gameIntroButtonText;

        gameIntro.style.display = DisplayStyle.Flex;
    }

    private void SetLevelInstructions()
    {
        instructions = root.Q<VisualElement>("panel-section");
        Label instructionHeadline = instructions.Q<Label>("panel-headline");
        instructionHeadline.text = TextMaterialArkku.instructionHeadlineText;
        Label instructionText = instructions.Q<Label>("panel-text");
        instructionText.text = TextMaterialArkku.instructionText;
        Button gotItButton = instructions.Q<Button>("panel-button");
        gotItButton.text = TextMaterialArkku.gotItButtonText;

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


    public void SetFeedpack(string feedpackFrase, string explanation, bool isCorrectAnswer)
    {

        answerImage = root.Q<Image>("panel-headline-image");

        if (isCorrectAnswer)
        {
            answerImage.style.backgroundImage = Resources.Load<Texture2D>("Images/correct");
        }
        else
        {
            answerImage.style.backgroundImage = Resources.Load<Texture2D>("Images/wrong");
        }
        answerImage.style.display = DisplayStyle.Flex;


        panelHeadline.text = feedpackFrase;
        panelText.text = explanation;

        panelButton.text = TextMaterialArkku.continueButtonText;

    }

    private void SetFeedpackPanelVisible()
    {
        panelSection.style.display = DisplayStyle.Flex;
        
        AudioSource.PlayClipAtPoint(soundObject.correctAnswerSound, cam.transform.position);

        //laitetaan napit takaisin käyttöön
        leftButton.SetEnabled(true);
        rightButton.SetEnabled(true);

    }

    private void SetPanelExit()
    {
        if (panelButton.text.Equals(TextMaterialArkku.continueButtonText))
        {
            ContinueGame();
            answerImage.style.display = DisplayStyle.None;
        }
        else if (panelButton.text.Equals(TextMaterialArkku.endGameButtonText))
        {
            Application.Quit();
        }
        else if (panelButton.text.Equals(TextMaterialArkku.gameIntroButtonText))
        {
            gameIntro.style.display = DisplayStyle.None;
            SetLevelInstructions();
        }
        else
        {
            instructions.style.display = DisplayStyle.None;
        }
    }

    //ottaa napit pois käytöstä nappispämmin estämiseksi 
    void FreezeButtons()
    {
        leftButton.SetEnabled(false);
        rightButton.SetEnabled(false);
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
           FreezeButtons();
            //IMPLEMENTOI STREAKIT: Kutsu sreak-kuvaketta, jos streakin arvo on tarpeeksi suuri           
           //asetetaan streak-kuvake, jos streak-arvo on tarpeeksi suuri 
           if (ScoreArkku.streak >= ScoreArkku.minStreakValue)
            {
                DisplayStreakImage();
               
            }
           //---------------------------------------------
            SetFeedpack(TextMaterialArkku.correctAnswerFeedpackText, levelManager.GetCurrentExplanation(), true);
            Invoke("SetFeedpackPanelVisible", RenderTimeForCorrectAnswerFeedpack);
            AudioSource.PlayClipAtPoint(soundObject.keytwistSound, cam.transform.position);
        }
        else
        {

            if (uiUtils.isStreakColoringOn)
            {
                uiUtils.ScoreLabelToNormalColoring(gameScore);
            }
           
            SetFeedpack(TextMaterialArkku.wrongAnswerFeedpackText, levelManager.GetCurrentExplanation(), false);

            panelSection.style.display = DisplayStyle.Flex;
            AudioSource.PlayClipAtPoint(soundObject.wrongAnswerSound, cam.transform.position, 1f);

        }

    }

    //IMPLEMENTOI STREAKIT: Ota tämä funktio
    //asettaa streak imagen käymään näkyvissä
    private void DisplayStreakImage ()
    {

   
        //asettaa score labeliin uuden värin, joka ilmaisee, että streak on päällä
        if (!uiUtils.isStreakColoringOn) { 
            uiUtils.ScoreLabelToStreakColoring(gameScore);
        }

        streakImage = root.Q<VisualElement>("streak-image");

        //asettaa kuvaan oikean streakin arvon
        Label streakCount = streakImage.Q<Label>("streak-count");
        streakCount.text = "+" + ScoreArkku.streak;

        streakImage.style.display = DisplayStyle.Flex;
        streakImage.ToggleInClassList("streak-image-transition");

        AudioSource.PlayClipAtPoint(soundObject.streakSound, cam.transform.position);

        Invoke("ToggleStreakClassList", 3f);
       
    }

    //IMPLEMENTOI STREAKIT: Ota tämä funktio
    //hävittää streak imagen näkyvistä ja asettaa classlistin alkuperäiseen asentoon
    private void ToggleStreakClassList()
    {
        streakImage.ClearClassList();
        streakImage.style.display = DisplayStyle.None;
    }
    //------------------------------------------------------------



    public void DeclareWin()
    {

        panelHeadline.text = TextMaterialArkku.winningRoundHeadline;
        panelText.text = TextMaterialArkku.winningRoundText;

        panelButton.text = TextMaterialArkku.nextGameButtonText;

        panelSection.style.display = DisplayStyle.Flex;

       
        panelButton.clicked += () =>
        {
            SceneManager.LoadScene("HillopurkitScene");
        };
     
    }

    public void UpProgressBar(float newProgressBarValue, float progBarValueToWin)
    {
        progressBar.value = newProgressBarValue;
        Debug.Log("progress bar value: " + progressBar.value);

        if (progressBar.value >= progBarValueToWin)
        {
            VisualElement star1 = root.Q<VisualElement>("star1");
            star1.style.backgroundImage = Resources.Load<Texture2D>("Images/star");

            //IMPLEMENTOI TÄHDEN SUURENTUMINEN: Ota nämä rivit oman tähden värinvaihdon jälkeen
            //tähti suurenee ja pienenee    
            star1.ToggleInClassList("star-scale-transition");
            root.schedule.Execute(() => star1.ToggleInClassList("star-scale-transition")).StartingIn(500);
            //----------------------------------------------------------------

            AudioSource.PlayClipAtPoint(soundObject.starSound, cam.transform.position);

        }

    }
}
