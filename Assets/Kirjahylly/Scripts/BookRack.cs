using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using System;
using System.Linq;
using UnityEngine;

public class BookRack : BookHolderBase
{
    public event EventHandler CompletionEvent;
    public bool isCompleted = false;

    public override void AddBook(GameObject book)
    {
        this.bookStack.Add(book);
        this.UpdateBookPositions();

        // Return if rack is not full
        if (this.CanHoldMoreBooks())
        {
            return;
        }

        // Rack is full, check if the books are correct
        if (this.BooksAreCorrect())
        {
            // TODO: correct books, animation/lock rack
            print("Rack is correct");
            this.OnCompletion();
        }
        else
        {
            // TODO: wrong books, animation
            print("Rack is incorrect");
        }

    }

    bool BooksAreCorrect()
    {
        int firstCategory = this.bookStack[0].GetComponent<Book>().word_category;
        return this.bookStack.All(b => b.GetComponent<Book>().word_category == firstCategory);
    }

    public void OnCompletion() {
        // Calls BookManagers method to check if all other book racks
        // are completed too and if the next level should be loaded.
        this.isCompleted = true;
        CompletionEvent?.Invoke(this, EventArgs.Empty);
    }
}
