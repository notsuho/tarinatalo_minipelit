using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour, BookHolderInterface {
    public GameObject Book;
    private List<GameObject> bookStack = new List<GameObject>();
    private List<Vector3> bookPositions = new List<Vector3>{
        new Vector3(-1.17799997f, 0.666999996f, -4.28399992f),
        new Vector3(-1.17799997f, 0.842999995f, -4.28399992f),
        new Vector3(-1.17799997f, 1.00699997f, -4.28399992f)
    };
    private Vector3 booksRotation = new Vector3(0f, 180f, 90f);

    void Update() {}

    void Start(){
        GameObject[] books = GameObject.FindGameObjectsWithTag("Book");
        foreach (GameObject book in books) {
            this.AddBook(book);
            Book bookScript = book.GetComponent<Book>();
            bookScript.SetCurrentHolder(this.gameObject);
        }
        this.UpdateBookPositions();
    }

    public void AddBook(GameObject book) {
        this.bookStack.Add(book);
        this.UpdateBookPositions();
    }

    public void RemoveBook(GameObject book) {
        this.bookStack.Remove(book);
        this.UpdateBookPositions();
    }

    public void UpdateBookPositions() {
        for (int i = 0; i < this.bookStack.Count; ++i) {
            Book bookScript = bookStack[i].GetComponent<Book>();
            bookScript.SetNewTargetPositionAndRotation(bookPositions[i], booksRotation);
        }
    }
}
