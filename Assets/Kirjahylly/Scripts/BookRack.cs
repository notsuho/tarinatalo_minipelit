using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookRack : BookHolderBase {
    public override void AddBook(GameObject book) {
        this.bookStack.Add(book);
        this.UpdateBookPositions();

        // Return if rack is not full
        if (this.CanHoldMoreBooks()) {
            return;
        }

        // Rack is full, check if the books are correct
        if (this.BooksAreCorrect()) {
            // TODO: correct books, animation/lock rack
        } else {
            // TODO: wrong books, animation
        }

    }

    bool BooksAreCorrect() {
        // TODO: check if all books are same category
        print("rack is full");
        return true;
    }
}
