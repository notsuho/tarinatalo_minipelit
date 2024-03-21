using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : BookHolderBase {
    [SerializeField]
    protected List<Vector3> bookPositions2;
    protected List<GameObject> bookStack2 = new List<GameObject>();

    public override void AddBook(GameObject book) {
        if (this.bookStack2.Count > this.bookStack.Count) {
            this.bookStack.Add(book);
        } else {
            this.bookStack2.Add(book);
        }
        this.UpdateBookPositions();
    }

    public override void RemoveBook(GameObject book) {
        // removing object that doesn't exist in list is handled
        this.bookStack.Remove(book);
        this.bookStack2.Remove(book);
        this.UpdateBookPositions();
    }

    public override bool CanHoldMoreBooks() {
        return this.bookStack.Count + this.bookStack.Count < maxBookAmount;
    }

    public override void UpdateBookPositions() {
        for (int i = 0; i < this.bookStack.Count; ++i) {
            Book bookScript = bookStack[i].GetComponent<Book>();
            bookScript.SetNewTargetPositionAndRotation(bookPositions[i], booksRotation);
        }

        for (int i = 0; i < this.bookStack2.Count; ++i) {
            Book bookScript = bookStack2[i].GetComponent<Book>();
            bookScript.SetNewTargetPositionAndRotation(bookPositions2[i], booksRotation);
        }
    }

    public override void ClearBooks() {
        foreach (GameObject book in this.bookStack) {
            Destroy(book);
        }
        foreach (GameObject book in this.bookStack2) {
            Destroy(book);
        }
        this.bookStack.Clear();
        this.bookStack2.Clear();
    }
}
