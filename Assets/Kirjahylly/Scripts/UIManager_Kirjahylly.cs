using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIManager_Kirjahylly : MonoBehaviour
{

    private VisualElement root;
    
    private VisualElement panelSection;
    private Label panelHeadline;
    private Label panelText;
    private Button panelButton;
    private VisualElement instructions;
    private Label feedback;
    private ProgressBar progressBar;
    private Button hint;
    private Label scoreLabel;
    private VisualElement streakImage;
    private float lastHintUseTime = -1000;
    private bool hintAvailable = true;

    private string gotItButtonText = "<allcaps>selvä!</allcaps>";
    private string instructionHeadlineText = "<allcaps>3/3 Kirjahylly</allcaps>";
    private string instructionTextText = "Kirjat ovat sekaisin. Järjestä kirjat hyllyyn niiden merkityksen perusteella. \nJos jäät jumiin, voit käyttää vihjettä päästäksesi eteenpäin. Vihjeen käytöstä vähennetään pisteitä.";
    private readonly string winningHeadline = "Hienoa työtä!";
    private readonly string endHeadline = "Läpäisit pelin!";
    private readonly string winningText = "Sait järjestettyä kaikki kirjat oikein hyllyihin. Hienoa!";
    private readonly string endText = "Suoritit kaikki synonyymipelit. Hienoa!";
    private readonly string endGameButtonText = "<allcaps>takaisin päävalikkoon</allcaps>";
    public BookManager manager;
    public UIUtils uiUtils;

    private void OnEnable()
    {
        uiUtils = GetComponent<UIUtils>();

        manager = FindObjectOfType<BookManager>();
        root = FindObjectOfType<UIDocument>().rootVisualElement;

        instructions = root.Q<VisualElement>("panel-section");

        Button instructionButton = root.Q<Button>("instruction-button");
        Button exitButton = root.Q<Button>("exit-button");

        panelSection = root.Q<VisualElement>("panel-section");
        panelHeadline = panelSection.Q<Label>("panel-headline");
        panelText = panelSection.Q<Label>("panel-text");
        panelButton = panelSection.Q<Button>("panel-button");
        feedback = root.Q<Label>("feedback");
        this.hint = root.Q<Button>("clue");
        this.scoreLabel = root.Q<VisualElement>("game-progress-container")
            .Q<VisualElement>("score-container")
            .Q<Label>("score-label");

        instructionButton.clicked += () => SetInstructions();
        panelButton.clicked += () => instructions.style.display = DisplayStyle.None;
        exitButton.clicked += () => uiUtils.SetConfirmationPanel(root);
        this.hint.clicked += () => {
            if (this.hintAvailable) {
                manager.UseHint();
                this.lastHintUseTime = Time.time;
            }
        };
    }

    void Update() {
        this.hintAvailable = Time.time - this.lastHintUseTime > 10;
        this.hint.style.opacity = this.hintAvailable ? 1.0f : 0.25f;

        if (GameManager.streak > 2) {
            uiUtils.ScoreLabelToStreakColoring(this.scoreLabel);
        } else {
            uiUtils.ScoreLabelToNormalColoring(this.scoreLabel);
        }
    }

    public void SetInstructions ()
    {
        Label instructionHeadline = instructions.Q<Label>("panel-headline");
        instructionHeadline.text = instructionHeadlineText;
        Label instructionText = instructions.Q<Label>("panel-text");
        instructionText.text = instructionTextText;
        Button gotItButton = instructions.Q<Button>("panel-button");
        gotItButton.text = gotItButtonText;

        instructions.style.display = DisplayStyle.Flex;
    }

    public void UpProgressBar(float points, float pointsToWin)
    {
        progressBar = root.Q<ProgressBar>("progress-bar");
        progressBar.value = points;
        this.scoreLabel.text = GameManager.totalPoints.ToString();

        if (progressBar.value >= pointsToWin) {
            VisualElement star3 = root.Q<VisualElement>("star3");
            star3.style.backgroundImage = Resources.Load<Texture2D>("Images/star");
            manager.PlayStarSound();

            //tähti suurenee ja pienenee    
            star3.ToggleInClassList("star-scale-transition");
            root.schedule.Execute(() => star3.ToggleInClassList("star-scale-transition")).StartingIn(500);
 
        }
    }

    public void LoadProgressBar(){
        progressBar = root.Q<ProgressBar>("progress-bar");
        progressBar.value = 66f;

        VisualElement star1 = root.Q<VisualElement>("star1");
        VisualElement star2 = root.Q<VisualElement>("star2");
        VisualElement star3 = root.Q<VisualElement>("star3");

        star1.style.backgroundImage = Resources.Load<Texture2D>("Images/star");
        star2.style.backgroundImage = Resources.Load<Texture2D>("Images/star");
        star3.style.backgroundImage = Resources.Load<Texture2D>("Images/star_blank");
    }

    public bool InstructionsShown(){
        return instructions.style.display == DisplayStyle.Flex;
    }

    public void SetFeedback(){
        feedback.visible = true;
        StartCoroutine(FeedbackTurnOffDelay());
    }

    private IEnumerator FeedbackTurnOffDelay(){
        yield return new WaitForSeconds(2);
        feedback.visible = false;
    }

    public void ShowEndFeedback(){
        panelHeadline.text = winningHeadline;
        panelText.text = winningText;
        panelButton.text = gotItButtonText;

        panelSection.style.display = DisplayStyle.Flex;
        panelButton.clicked += () =>
        {
            this.ShowMinigamesEndFeedback();
        };
    }

    public void ShowMinigamesEndFeedback(){
        panelHeadline.text = endHeadline;
        panelText.text = endText;
        panelButton.text = endGameButtonText;

        panelSection.style.display = DisplayStyle.Flex;
        panelButton.clicked += () =>
        {
            // tähän esim application quit, tai kutsu palata päävalikkoon tms.
            // Application.Quit();
        };
    }

    //asettaa streak imagen käymään näkyvissä
    public void DisplayStreakImage ()
    {

        //asettaa score labeliin uuden värin, joka ilmaisee, että streak on päällä
        if (!uiUtils.isStreakColoringOn) { 
            uiUtils.ScoreLabelToStreakColoring(this.scoreLabel);
        }
        //-------------------------------------------------

        streakImage = root.Q<VisualElement>("streak-image");

        //asettaa kuvaan oikean streakin arvon
        Label streakCount = streakImage.Q<Label>("streak-count");
        streakCount.text = "+" + GameManager.streak;

        streakImage.style.display = DisplayStyle.Flex;
        streakImage.ToggleInClassList("streak-image-transition");
        manager.PlayStreakSound();

        Invoke("ToggleStreakClassList", 3f);
       
    }

    //hävittää streak imagen näkyvistä ja asettaa classlistin alkuperäiseen asentoon
    private void ToggleStreakClassList()
    {
        streakImage.ClearClassList();
        streakImage.style.display = DisplayStyle.None;
    }

    public void UpdateScoreLabel(){
        this.scoreLabel.text = GameManager.totalPoints.ToString();
    }
}
