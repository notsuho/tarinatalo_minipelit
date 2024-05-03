using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class HillopurkitUIManager : MonoBehaviour
{
    public MiniGameManager miniGameManager;
    private VisualElement root;
    private VisualElement panelSection;
    private Label panelHeadline;
    private Label panelText;
    private Button panelButton;
    private VisualElement instructions;
    private Label scoreLabel;
    private Label clickedWrong;
    private Label clickedRight;
    private VisualElement streakImage;
    private UIUtils uiUtils;
    public SoundObject soundObject;
    
    [Tooltip("Streak starts increasing points after this streak is reached.")]
    [SerializeField] private int minimumSteakValue = 3;

    private void OnEnable()
    {
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

        if (GameManager.streak > 2) {
            uiUtils.ScoreLabelToStreakColoring(this.scoreLabel);
        } else {
            uiUtils.ScoreLabelToNormalColoring(this.scoreLabel);
        }

        SetInstructions();

        instructionButton.clicked += () => SetInstructions();
        panelButton.clicked += () => SetPanelExit();
        exitButton.clicked += () => uiUtils.SetConfirmationPanel(root);
    }

    // <summary>
    // Displays the instruction UI pop-up.
    // Clicking on the ? button also calls this method.
    // </summary>
    private void SetInstructions()
    {
        miniGameManager.PauseGame();

        instructions = root.Q<VisualElement>("panel-section");

        Label instructionHeadline = instructions.Q<Label>("panel-headline");
        instructionHeadline.text = TextMaterialHillopurkit.instructionHeadlineText;

        Label instructionText = instructions.Q<Label>("panel-text");
        instructionText.text = TextMaterialHillopurkit.instructionText;

        Button gotItButton = instructions.Q<Button>("panel-button");
        gotItButton.text = TextMaterialHillopurkit.gotItButtonText;

        instructions.style.display = DisplayStyle.Flex;
    }

    private void SetPanelExit()
    {
        if (panelButton.text.Equals(TextMaterialHillopurkit.endGameButtonText)) // minipelin lopussa oleva nappi "Palaa pääpeliin"
        {
            Application.Quit();
        }

        else
        {
            instructions.style.display = DisplayStyle.None; // ohjeista poistumisen "Selvä!"-nappi
            miniGameManager.UnpauseGame();
        }
    }

    public IEnumerator DeclareWin()
    {
        yield return new WaitForSeconds(WaitTimes.MESSAGE_TIME_LONG);

        panelHeadline.text = TextMaterialHillopurkit.winningHeadline;
        panelText.text = TextMaterialHillopurkit.winningText + ("\n\nPisteesi: " + GameManager.totalPoints);

        panelButton.text = TextMaterialHillopurkit.endGameButtonText;

        panelSection.style.display = DisplayStyle.Flex;

        AudioSource.PlayClipAtPoint(soundObject.victorySound, Camera.main.transform.position);

        // Close the minigame when player presses the button
        panelButton.clicked += () => {
            Application.Quit();       
        };
    }

    public void SetFeedback(bool result)
    {
        int points = GameManager.totalPoints;
        Label RightOrWrongLabel;

        // If a breakable jar was clicked, check for streak bonus points
        if (result) 
        {
            if (GameManager.streak >= minimumSteakValue) {
                DisplayStreakImage();
            }
            RightOrWrongLabel = clickedRight;
            RightOrWrongLabel.visible = true;
        }

        // If a wrong jar was clicked, turn score color back to normal 
        else
        {
            if (uiUtils.isStreakColoringOn)
            {
                uiUtils.ScoreLabelToNormalColoring(scoreLabel);
            }
            RightOrWrongLabel = clickedWrong;
            RightOrWrongLabel.visible = true;
        }
        
        scoreLabel = root.Q<Label>("score-label");
        scoreLabel.text = ("" + points);
        StartCoroutine(FeedbackTurnOffDelay(RightOrWrongLabel));
    }

    private IEnumerator FeedbackTurnOffDelay(Label feedbackMsg)
    {
        yield return new WaitForSeconds(WaitTimes.MESSAGE_TIME_SHORT);
        feedbackMsg.visible = false;
    }

    // Shows the streak-image for a moment.
    private void DisplayStreakImage()
    {
        if (!uiUtils.isStreakColoringOn)
        {
            uiUtils.ScoreLabelToStreakColoring(scoreLabel);
        }
        streakImage = root.Q<VisualElement>("streak-image");

        //asettaa kuvaan oikean streakin arvon
        Label streakCount = streakImage.Q<Label>("streak-count");
        streakCount.text = "+" + GameManager.streak;

        streakImage.style.display = DisplayStyle.Flex;
        streakImage.ToggleInClassList("streak-image-transition");
        AudioSource.PlayClipAtPoint(soundObject.streakSound, Camera.main.transform.position);
        StartCoroutine(ToggleStreakClassList());
    }

    // Hides the streak-image.
    private IEnumerator ToggleStreakClassList()
    {
        yield return new WaitForSeconds(3f);
        streakImage.ClearClassList();
        streakImage.style.display = DisplayStyle.None;
    }
}