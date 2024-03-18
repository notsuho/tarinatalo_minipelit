using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookHolderBase : MonoBehaviour {
    [SerializeField]
    protected List<Vector3> bookPositions;
    [SerializeField]
    protected Vector3 booksRotation;
    [SerializeField]
    protected int maxBookAmount;
    protected List<GameObject> bookStack = new List<GameObject>();

    public virtual void AddBook(GameObject book) {
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

    public void ClearBooks() {
        foreach (GameObject book in this.bookStack) {
            Destroy(book);
        }
        this.bookStack.Clear();
    }
}
