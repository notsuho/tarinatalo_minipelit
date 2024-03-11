using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Table : BookHolderBase {
    public GameObject bookPrefab;
    private int booksToGenerateCount = 10;

    public string []sanat1 = {"RYÖVÄTÄ", "VARASTAA", "KÄHVELTÄÄ"};
    string []sanat2 = {"NUKKUA", "UINUA", "KOISIA"};
    string []sanat3 = {"JUKSATA", "NARUTTAA", "HUIJATA"};
    string []muut = {"KAIVATA"};

    List<string> sanalista = new List<string>();

    void Start() {
        /*
            Generate books from the book prefab and give them random
            position above the table off screen so they fly onto the table.
            Add the books to table's bookStack.
        */

        // TODO: read words and correct categories from file
        // and pass the information to book object.

        sanalista.AddRange(sanat1);
        sanalista.AddRange(sanat2);
        sanalista.AddRange(sanat3);
        sanalista.AddRange(muut);

        for (int i = 0; i < booksToGenerateCount; ++i) {
            Vector3 randomStartPosition = new Vector3(
                Random.Range(-3.4f, 2.4f), // -3 - +3 from table's x value
                Random.Range(5.0f, 7.0f), // + 5-7 from table's y value
                -4.28399992f // table's z value
            );
            Quaternion startRotation = Quaternion.Euler(0f, 90f, 0f);
            GameObject book = Instantiate(bookPrefab, randomStartPosition, startRotation);

            TextMeshPro text = book.GetComponentInChildren<TextMeshPro>();

            int index = Random.Range(0, sanalista.Count);
            text.text = sanalista[index];
            sanalista.RemoveAt(index);

            this.AddBook(book);
            Book bookScript = book.GetComponent<Book>();
            bookScript.SetCurrentHolder(this.gameObject);
        }
        this.UpdateBookPositions();
    }
}
