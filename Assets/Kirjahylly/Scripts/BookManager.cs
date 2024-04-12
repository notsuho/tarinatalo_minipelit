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

    private float points = 0f;
    public float pointsToWin = 33f;
    private int total_rounds = 3;
    private int current_round = 0;

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
        ui = FindObjectOfType<UIManager_Kirjahylly>();
        ui.UpProgressBar(points, pointsToWin);
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

            if (bookScript.word_category == 1 && !preSetBook1)
            {
                SetStartingBook(book, 0);
                bookScript.SetBookFrozen();
                preSetBook1 = true;
            }
            else if (bookScript.word_category == 2 && !preSetBook2)
            {
                SetStartingBook(book, 1);
                bookScript.SetBookFrozen();
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
        bool levelCompleted = this.bookRacks.All(r => r.GetComponent<BookRack>().isCompleted);
        if (levelCompleted)
        {
            points += 11f;
            ui.UpProgressBar(points, pointsToWin);
            ui.SetFeedback();
            Invoke(nameof(RoundEnding), 2);
        }
    }

    public float GetPoints()
    {
        return points;
    }

    public float GetPointsToWin()
    {
        return pointsToWin;
    }

    private void RoundEnding()
    {
        current_round += 1;
        if (current_round >= total_rounds)
        {
            ui.ShowEndFeedback();
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
    }

    public void UseHint()
    {
        foreach (GameObject rack in bookRacks) {
            BookRack rackScript = rack.GetComponent<BookRack>();
            // Rack is already correctly filled
            if (rackScript.isCompleted) {
                continue;
            }

            List<GameObject> bookstack = rackScript.GetBookStack();
            // Rack only has one book
            if (bookstack.Count() < 2) {
                continue;
            }

            List<int> bookCategories = bookstack.Select(book =>
                book.GetComponent<Book>().word_category
            ).ToList();

            // If rack has 2 of same category books, use it as correct
            // category. Otherwise use the first book's category.
            var twoSameBooksGroup = bookCategories
                .GroupBy(category_num => category_num)
                .FirstOrDefault(same_nums => same_nums.Count() > 1);

            int assumedCorrectCategory = twoSameBooksGroup == null
                ? bookstack[0].GetComponent<Book>().word_category
                : twoSameBooksGroup.Key;

            GameObject incorrectBook = bookstack.Find(book => 
                book.GetComponent<Book>().word_category != assumedCorrectCategory
            );

            if (incorrectBook != null) {
                MoveBookToTable(incorrectBook);
                break;
            }
        }
    }

    public void MoveBookToTable(GameObject book){
        Table tableScript = table.GetComponent<Table>();
        Book book_obj = book.GetComponent<Book>();
        BookRack rack = book_obj.GetCurrHolder().GetComponent<BookRack>();
        rack.RemoveBook(book);
        tableScript.AddBook(book);
        book_obj.SetCurrentHolder((GameObject)table);
    }
}