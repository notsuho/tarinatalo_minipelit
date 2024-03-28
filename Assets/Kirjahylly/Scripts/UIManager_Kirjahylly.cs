using System;
using System.Collections.Generic;
using UnityEngine;
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

    private string gotItButtonText = "<allcaps>selvä!</allcaps>";
    private string instructionHeadlineText = "<allcaps>ohjeet</allcaps>";
    private string instructionTextText = "Kirjat ovat sekaisin. Järjestä kirjat hyllyyn niiden merkityksen perusteella.";

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
        panelButton.clicked += () => instructions.style.display = DisplayStyle.None;
        exitButton.clicked += () => Application.Quit();
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

}
