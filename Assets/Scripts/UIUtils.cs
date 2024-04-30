using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIUtils : MonoBehaviour
{
    private string yesButtonText = "kyll‰";
    private string noButtonText = "ei";
    private string exitQuestionText  = "Suljetaanko synonyymipeli?";

    public bool isStreakColoringOn = false;
    
    //pistelaskurin tekstin v‰ri, kun streak on p‰‰ll‰
    private Color streakScoreLabelColor = new Color(0.81f, 0.57f, 0.24f);
    
    //pistelaskurin tekstin varjo, kun streak on p‰‰ll‰
    private TextShadow streakShadow = new TextShadow
    {
        color = Color.white,
        blurRadius = 3f,
        offset = new Vector2(2f, 2f)
    };
    
    //pistelaskurin tekstin varjo, kun streak ei ole p‰‰ll‰
    private TextShadow normalShadow = new TextShadow
    {
        color = new Color(0.466f, 0.26f, 0.18f),
        blurRadius = 0f,
        offset = new Vector2(3f, 3f)
    };
   

    public Label ScoreLabelToStreakColoring(Label gameScore)
    { 
        gameScore.style.color = streakScoreLabelColor;
        gameScore.style.textShadow = new StyleTextShadow(streakShadow);

        isStreakColoringOn = true;

        return gameScore;
    }

    public Label ScoreLabelToNormalColoring(Label gameScore)
    {
        gameScore.style.color = Color.white;
        gameScore.style.textShadow = new StyleTextShadow(normalShadow);

        isStreakColoringOn = false;

        return gameScore;
    }

    // asettaa varmistuskysymyspaneelin, kun pelaaja on sulkemassa pelin
    public void SetConfirmationPanel(VisualElement root)
    {
        VisualElement exitConfirmationPanelSection = root.Q<VisualElement>("exit-confirmation-panel-section");

        Button yesButton = exitConfirmationPanelSection.Q<Button>("exit-yes-button");
        yesButton.text = yesButtonText;

        Button noButton = exitConfirmationPanelSection.Q<Button>("exit-no-button");
        noButton.text = noButtonText;

        Label exitQuestion = exitConfirmationPanelSection.Q<Label>("exit-text");
        exitQuestion.text = exitQuestionText;

        exitConfirmationPanelSection.style.display = DisplayStyle.Flex;

        yesButton.clicked += () => Application.Quit(); 
        noButton.clicked += () => exitConfirmationPanelSection.style.display = DisplayStyle.None;

       
    }


}
