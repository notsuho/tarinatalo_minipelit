using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookRack_0 : MonoBehaviour, BookHolderInterface {
    public GameObject Book;
    private int maxBookAmount = 3;
    private List<GameObject> bookStack = new List<GameObject>();
    private List<Vector3> bookPositions = new List<Vector3>{
        new Vector3(-0.061999999f, 2.26399994f, -3.70799994f),
        new Vector3(0.158999994f, 2.26399994f, -3.70799994f),
        new Vector3(0.377000004f, 2.26399994f, -3.70799994f)
    };
    private Vector3 booksRotation = new Vector3(0f, 180f, 180f);

    void Start() {}

    void Update() {}

    public void AddBook(GameObject book) {
        this.bookStack.Add(book);
        this.UpdateBookPositions();
    }

    public void RemoveBook(GameObject book) {
        this.bookStack.Remove(book);
        this.UpdateBookPositions();
    }

    public bool CanHoldMoreBooks() {
        return bookStack.Count < maxBookAmount;
    }

    public void UpdateBookPositions() {
        for (int i = 0; i < this.bookStack.Count; ++i) {
            Book bookScript = bookStack[i].GetComponent<Book>();
            bookScript.SetNewTargetPositionAndRotation(bookPositions[i], booksRotation);
        }
    }


}
