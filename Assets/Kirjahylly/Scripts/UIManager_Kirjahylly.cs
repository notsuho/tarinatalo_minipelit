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

    private string gotItButtonText = "<allcaps>selvä!</allcaps>";
    private string instructionHeadlineText = "<allcaps>ohjeet</allcaps>";
    private string instructionTextText = "Kirjat ovat sekaisin. Järjestä kirjat hyllyyn niiden merkityksen perusteella.";
    private readonly string winningHeadline = "Läpäisit pelin!";
    private readonly string winningText = "Sait järjestettyä kaikki kirjat oikein hyllyihin. Hienoa!";
    private readonly string nextGameButtonText = "<allcaps>seuraava minipeli</allcaps>";
    public BookManager manager;
    
    private void OnEnable()
    {
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
        hint = root.Q<Button>("clue");

        instructionButton.clicked += () => SetInstructions();
        instructionButton.clicked += () => print("click");
        panelButton.clicked += () => instructions.style.display = DisplayStyle.None;
        exitButton.clicked += () => Application.Quit();
        hint.clicked += () => manager.UseHint();
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

        if (progressBar.value >= pointsToWin)
        {
            VisualElement star1 = root.Q<VisualElement>("star1");
            star1.style.backgroundImage = Resources.Load<Texture2D>("Images/star");

            //tähti suurenee ja pienenee    
            star1.ToggleInClassList("star-scale-transition");
            root.schedule.Execute(() => star1.ToggleInClassList("star-scale-transition")).StartingIn(500);
 
        }

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
        panelButton.text = nextGameButtonText;

        panelSection.style.display = DisplayStyle.Flex;
        panelButton.clicked += () =>
        {
            SceneManager.LoadScene("HillopurkitScene");
        };

    }
}
