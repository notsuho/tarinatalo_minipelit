using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

            GameObject text = new GameObject();
            text.transform.SetParent(book.transform);

            TextMeshPro t = text.AddComponent<TextMeshPro>();
            t.text = "testi";
            t.fontSize = 40;

            text.transform.localEulerAngles = new Vector3(0, 180, 90);
            text.transform.localPosition = new Vector3(-0.0122f, -0.047f, 0.0785f);
            text.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            t.alignment = (TextAlignmentOptions)TextAnchor.MiddleCenter;

            this.AddBook(book);
            Book bookScript = book.GetComponent<Book>();
            bookScript.SetCurrentHolder(this.gameObject);
        }
        this.UpdateBookPositions();
    }
}
