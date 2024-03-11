using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class BookRack : BookHolderBase
{
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
        }
        else
        {
            // TODO: wrong books, animation
            print("Rack is incorrect");
        }

    }

    bool BooksAreCorrect()
    {
        if (this.bookStack[0].GetComponent<Book>().word_category.Equals(this.bookStack[1].GetComponent<Book>().word_category) && this.bookStack[0].GetComponent<Book>().word_category.Equals(this.bookStack[2].GetComponent<Book>().word_category))
        {
            return true;
        }
        return false;
    }
}
