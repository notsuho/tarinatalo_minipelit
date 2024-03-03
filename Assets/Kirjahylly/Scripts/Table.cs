using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : BookHolderBase {
    public GameObject bookPrefab;
    private int booksToGenerateCount = 10;

    void Start() {
        /*
            Generate books from the book prefab and give them random
            position above the table off screen so they fly onto the table.
            Add the books to table's bookStack.
        */

        // TODO: read words and correct categories from file
        // and pass the information to book object.

        for (int i = 0; i < booksToGenerateCount; ++i) {
            Vector3 randomStartPosition = new Vector3(
                Random.Range(-3.4f, 2.4f), // -3 - +3 from table's x value
                Random.Range(5.0f, 7.0f), // + 5-7 from table's y value
                -4.28399992f // table's z value
            );
            Quaternion startRotation = Quaternion.Euler(0f, 90f, 0f);
            GameObject book = Instantiate(bookPrefab, randomStartPosition, startRotation);

            this.AddBook(book);
            Book bookScript = book.GetComponent<Book>();
            bookScript.SetCurrentHolder(this.gameObject);
        }
        this.UpdateBookPositions();
    }
}
