using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public GameManager gameManager;

    private VisualElement root;
    private Label sentenceLabel;
    private Button leftButton;
    private Button rightButton;
    private VisualElement feedpackPanel;

    private string sentence;
    private string leftWord;
    private string rightWord;

    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        sentenceLabel = root.Q<Label>("sentence");
        leftButton = root.Q<Button>("leftButton");
        rightButton = root.Q<Button>("rightButton");

        feedpackPanel = root.Q<VisualElement>("feedpackPanel");

        Button continueButton = root.Q<Button>("continueButton");

        leftButton.clicked += () => gameManager.CheckAnswer(leftWord);
        rightButton.clicked += () => gameManager.CheckAnswer(rightWord);
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
        Label feedpackText = feedpackPanel.Q<Label>("feedpackText");
        feedpackText.text = feedpackFrase;

        Label feedpackExplanation = feedpackPanel.Q<Label>("feedpackExplanation");
        feedpackExplanation.text = explanation; 

        feedpackPanel.style.display = DisplayStyle.Flex;
    }

    private void ContinueGame ()
    {
        feedpackPanel.style.display = DisplayStyle.None;
        gameManager.SetCurrentExercise();
    }
}
