using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class BookManager : MonoBehaviour {
    public GameObject bookPrefab;
    public GameObject table;
    public List<GameObject> bookRacks;
    public TextAsset wordsAndCategoriesJson;

    private int currentSetIndex = 0;
    private List<List<JsonBook>> bookSets = new List<List<JsonBook>>();

    [System.Serializable]
    public class JsonBook {
        public int category;
        public string word;
    }

    [System.Serializable]
    public class JsonRoot {
        public List<JsonBook> bookset1 = new List<JsonBook>();
        public List<JsonBook> bookset2 = new List<JsonBook>();
        public List<JsonBook> bookset3 = new List<JsonBook>();
    }

    void Start() {
        this.LoadBookDataFromFile();
        this.ResetBooks();

        // Init event for bookracks to call BookManagers CheckLevelCompletion
        // when the rack is completed to see if whole level is done.
        foreach (GameObject rack in bookRacks) {
            rack.GetComponent<BookRack>().CompletionEvent += this.CheckLevelCompletion;
        }
    }

    void ResetBooks() {
        this.ClearBooks();
        Table tableScript = table.GetComponent<Table>();
        Renderer tableRenderer = table.GetComponent<Renderer>();
        Vector3 tableMidPoint = tableRenderer.bounds.center;

        foreach (JsonBook bookData in this.bookSets[this.currentSetIndex]) {
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

            tableScript.AddBook(book);
        }

        tableScript.UpdateBookPositions();

        // Increment index of current level/set of books
        this.currentSetIndex = (this.currentSetIndex + 1) % 3;
    }

    void ClearBooks() {
        Table tableScript = table.GetComponent<Table>();
        tableScript.ClearBooks();

        foreach (GameObject rack in bookRacks) {
            BookRack rackScript = rack.GetComponent<BookRack>();
            rackScript.ClearBooks();
            rackScript.isCompleted = false;
        }
    }

    void LoadBookDataFromFile() {
        JsonRoot jsonRoot = JsonUtility.FromJson<JsonRoot>(wordsAndCategoriesJson.text);
        this.bookSets.Add(jsonRoot.bookset1);
        this.bookSets.Add(jsonRoot.bookset2);
        this.bookSets.Add(jsonRoot.bookset3);
    }

    void CheckLevelCompletion(object rack, System.EventArgs args) {
        bool levelCompleted = this.bookRacks.All(r => r.GetComponent<BookRack>().isCompleted);
        if (levelCompleted) {
            this.ResetBooks();
        }

    }
}