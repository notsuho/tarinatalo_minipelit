using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using Unity.VisualScripting;

public class BookManager : MonoBehaviour
{
    public GameObject bookPrefab;
    public GameObject table;
    public List<GameObject> bookRacks;
    public TextAsset wordsAndCategoriesJson;
    public UIManager_Kirjahylly ui;
    public SoundObject soundObject;
    public Camera cam;

    private float pointsToWin = 100;
    private int total_rounds = 3;
    private int current_round = 0;
    private int racksFilled = 0;

    private int currentSetIndex = 0;
    private bool preSetBook1 = false;
    private bool preSetBook2 = false;

    private List<List<JsonBook>> bookSets = new List<List<JsonBook>>();

    [System.Serializable]
    public class JsonBook
    {
        public int category;
        public string word;
    }

    [System.Serializable]
    public class JsonRoot
    {
        public List<JsonBook> bookset1 = new List<JsonBook>();
        public List<JsonBook> bookset2 = new List<JsonBook>();
        public List<JsonBook> bookset3 = new List<JsonBook>();
    }

    IEnumerator Start()
    {
        cam = Camera.main;
        ui = FindObjectOfType<UIManager_Kirjahylly>();
        ui.UpProgressBar(this.GetProgress(), pointsToWin);
        ui.LoadProgressBar();
        ui.SetInstructions();
        yield return new WaitUntil(() => !ui.InstructionsShown());
        this.LoadBookDataFromFile();
        this.ResetBooks();

        // Init event for bookracks to call BookManagers CheckLevelCompletion
        // when the rack is completed to see if whole level is done.
        foreach (GameObject rack in bookRacks)
        {
            rack.GetComponent<BookRack>().CompletionEvent += this.CheckLevelCompletion;
        }
    }

    void ResetBooks()
    {
        this.ClearBooks();
        Table tableScript = table.GetComponent<Table>();
        Renderer tableRenderer = table.GetComponent<Renderer>();
        Vector3 tableMidPoint = tableRenderer.bounds.center;
        preSetBook1 = false;
        preSetBook2 = false;

        foreach (JsonBook bookData in this.bookSets[this.currentSetIndex])
        {
            // Set random location above the table
            Vector3 randomStartPosition = new Vector3(
                Random.Range(tableMidPoint.x - 5, tableMidPoint.x + 5),
                Random.Range(tableMidPoint.y + 5, tableMidPoint.y + 10),
                tableMidPoint.z
            );
            Quaternion startRotation = Quaternion.Euler(0f, 90f, 0f);
            GameObject book = Instantiate(bookPrefab, randomStartPosition, startRotation);

            // Setup book
            Book bookScript = book.GetComponent<Book>();
            bookScript.SetCurrentHolder(table);
            bookScript.word_category = bookData.category;
            book.GetComponentInChildren<TextMeshPro>().text = bookData.word;
            book.name = bookData.word;

            // Setup preset books in bookcase for two uppermost racks: 
            // Correct books in top rack will have category 1, middle category 2 and bottom category 3
            if (bookScript.word_category == 1 && !preSetBook1)
            {
                SetStartingBook(book, 0);
                preSetBook1 = true;
            }
            else if (bookScript.word_category == 2 && !preSetBook2)
            {
                SetStartingBook(book, 1);
                preSetBook2 = true;
            }
            else
            {
                tableScript.AddBook(book);
            }
        }

        tableScript.UpdateBookPositions();

        // Increment index of current level/set of books
        this.currentSetIndex = (this.currentSetIndex + 1) % 3;
    }

    void ClearBooks()
    {
        Table tableScript = table.GetComponent<Table>();
        tableScript.ClearBooks();

        foreach (GameObject rack in bookRacks)
        {
            BookRack rackScript = rack.GetComponent<BookRack>();
            rackScript.ClearBooks();
            rackScript.isCompleted = false;
        }
    }

    void LoadBookDataFromFile()
    {
        JsonRoot jsonRoot = JsonUtility.FromJson<JsonRoot>(wordsAndCategoriesJson.text);
        this.bookSets.Add(ShuffleList(jsonRoot.bookset1));
        this.bookSets.Add(ShuffleList(jsonRoot.bookset2));
        this.bookSets.Add(ShuffleList(jsonRoot.bookset3));
    }

    void CheckLevelCompletion(object rack, System.EventArgs args)
    {
        GameManager.AddPoints(true, 11);
        racksFilled++;
        ui.UpProgressBar(this.GetProgress(), pointsToWin);
        bool levelCompleted = this.bookRacks.All(r => r.GetComponent<BookRack>().isCompleted);
        if (levelCompleted)
        {
            ui.SetFeedback();
            Invoke(nameof(RoundEnding), 2);
        }
    }

    private void RoundEnding()
    {
        current_round += 1;
        if (current_round >= total_rounds)
        {
            ui.ShowEndFeedback();
            PlayVictorySound();
        }
        else
        {
            ResetBooks();
        }
    }

    List<JsonBook> ShuffleList(List<JsonBook> list)
    {
        System.Random rand = new System.Random();
        return list.OrderBy(_ => rand.Next()).ToList();
    }

    private void SetStartingBook(GameObject book, int rackIndex)
    {
        Table tableScript = table.GetComponent<Table>();
        BookRack rack = bookRacks[rackIndex].GetComponent<BookRack>();
        tableScript.RemoveBook(book);
        rack.AddBook(book);
        Book book_obj = book.GetComponent<Book>();
        book_obj.SetCurrentHolder(table);
        book_obj.SetBookFrozen();
    }

    public void UseHint()
    {
        bool bookMoved = false;
        GameManager.AddPoints(false, -11);
        //Check if there is a wrong book in book case and replace it with correct one
        for (int i = 0; i < 3; i++)
        {
            if (bookMoved)
            {
                break;
            }

            BookRack rackScript = bookRacks[i].GetComponent<BookRack>();

            //Rack is already correctly filled
            if (rackScript.isCompleted)
            {
                continue;
            }

            List<GameObject> bookstack = rackScript.GetBookStack();

            //Check if books are correct category and move possible wrong book to table
            //and replace it with correct book
            for (int j = 0; j < bookstack.Count; j++)
            {
                Book book = bookstack[j].GetComponent<Book>();
                if (book.GetWordCategory() != i + 1)
                {
                    MoveBookToTable(bookstack[j]);
                    bookMoved = true;
                    break;
                }
            }

        }

        //No wrong book in book case: Move one book to correct place
        if (!bookMoved)
        {
            for (int i = 0; i < 3; i++)
            {
                if (bookMoved)
                {
                    break;
                }

                BookRack rackScript = bookRacks[i].GetComponent<BookRack>();

                //Rack is already correctly filled
                if (rackScript.isCompleted)
                {
                    continue;
                }

                StartCoroutine(MoveCorrectBookToRack(i, 0));
                bookMoved = true;
                break;
            }
        }
    }

    public void MoveBookToTable(GameObject book)
    {
        Table tableScript = table.GetComponent<Table>();
        Book book_obj = book.GetComponent<Book>();
        BookRack rack = book_obj.GetCurrHolder().GetComponent<BookRack>();
        rack.RemoveBook(book);
        tableScript.AddBook(book);
        book_obj.SetCurrentHolder((GameObject)table);
    }

    public IEnumerator MoveCorrectBookToRack(int rack_index, float wait)
    {
        yield return new WaitForSeconds(wait);
        bool bookMoved = false;
        Table tableScript = table.GetComponent<Table>();
        List<GameObject> table_bookstack = tableScript.GetBookStack();
        GameObject rack = bookRacks[rack_index];

        //check if book of wanted category is in right stack of books and move it to correct rack
        foreach (GameObject book in table_bookstack)
        {
            Book book_obj = book.GetComponent<Book>();

            if (book_obj.word_category == rack_index + 1)
            {
                tableScript.RemoveBook(book);
                rack.GetComponent<BookRack>().AddBook(book);
                book_obj.SetCurrentHolder(rack);
                book_obj.SetBookFrozen();
                bookMoved = true;
                break;
            }
        }

        if (!bookMoved)
        {
            //check if book of wanted category is in wrong rack and move it to correct one
            for (int i = 0; i < 3; i++)
            {
                BookRack rackScript = bookRacks[i].GetComponent<BookRack>();
                List<GameObject> rack_bookStack = rackScript.GetBookStack();

                if (rackScript.isCompleted || rack_index == i)
                {
                    continue;
                }

                foreach (GameObject book in rack_bookStack.ToList())
                {
                    Book book_obj = book.GetComponent<Book>();
                    if (book_obj.word_category == rack_index + 1)
                    {
                        rackScript.RemoveBook(book);
                        rack.GetComponent<BookRack>().AddBook(book);
                        book_obj.SetCurrentHolder(rack);
                        book_obj.SetBookFrozen();
                        bookMoved = true;
                        break;
                    }
                }
            }

        }


    }

    public void PlayBookDropSound(){
        AudioSource.PlayClipAtPoint(soundObject.bookDrop, cam.transform.position, 0.25f);
    }

    public void PlayCorrectSound(){
        AudioSource.PlayClipAtPoint(soundObject.correctAnswerSound, cam.transform.position);
    }

    public void PlayWrongSound(){
        AudioSource.PlayClipAtPoint(soundObject.wrongAnswerSound, cam.transform.position);
    }

    public void PlayVictorySound(){
        AudioSource.PlayClipAtPoint(soundObject.victorySound, cam.transform.position);
    }

    float GetProgress() {
        if (this.racksFilled == 9) {
            return 100.0f;
        }
        int numOfRacksToComplete = 9;
        int progressAvailableInThisGame = 34;
        int basePointsFromPreviousGames = 66;
        float progressPerRack = progressAvailableInThisGame / numOfRacksToComplete;
        return basePointsFromPreviousGames + (progressPerRack * this.racksFilled);
    }
}