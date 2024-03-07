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
    private VisualElement feedpackSection;

    private string sentence;
    private string leftWord;
    private string rightWord;

    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        sentenceLabel = root.Q<Label>("sentence");
        leftButton = root.Q<Button>("left-button");
        rightButton = root.Q<Button>("right-button");

        Button instructionButton = root.Q<Button>("got-it-button");
        VisualElement instructions = root.Q<VisualElement>("instructions-section");

        feedpackSection = root.Q<VisualElement>("feedpack-section");
        Button continueButton = root.Q<Button>("continue-button");

        leftButton.clicked += () => gameManager.CheckAnswer(leftWord);
        rightButton.clicked += () => gameManager.CheckAnswer(rightWord);
        instructionButton.clicked += () => instructions.style.display = DisplayStyle.None;
        continueButton.clicked += () => ContinueGame();

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
        Label feedpackText = feedpackSection.Q<Label>("feedpack-text");
        feedpackText.text = feedpackFrase;

        Label feedpackExplanation = feedpackSection.Q<Label>("feedpack-explanation");
        feedpackExplanation.text = explanation; 

        feedpackSection.style.display = DisplayStyle.Flex;
    }

    private void ContinueGame ()
    {
        feedpackSection.style.display = DisplayStyle.None;
        gameManager.SetCurrentExercise();
    }

    public void DeclareWin () 
    {
        Label feedpackText = feedpackSection.Q<Label>("feedpack-text");
        feedpackText.text = "Läpäisit pelin!";

        Label feedpackExplanation = feedpackSection.Q<Label>("feedpack-explanation");
        feedpackExplanation.text = "Sait sanataiturin arvomerkin.<br><br>Pisteesi: 5000";

        feedpackSection.style.display = DisplayStyle.Flex;
    }

    public void UpProgressBar(float value)
    {
        ProgressBar progressBar = root.Q<ProgressBar>("progress-bar");
        progressBar.value += value;
        Debug.Log(progressBar.value);

        /*VisualElement star3 = root.Q<VisualElement>("star3");
        StyleBackground image  = Resources.Load<Texture2D>("star_yellow.png");
        Debug.Log(image);*/

        if (progressBar.value >= 98)
        {
            VisualElement star3 = root.Q<VisualElement>("star3");
            star3.style.backgroundImage = Resources.Load<Texture2D>("Images/star_yellow");
            //star3.style.backgroundImage = Background.FromSprite(Resources.Load<Sprite>("Images/star_yellow"));

        }

    }
    }
