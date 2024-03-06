using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Table : BookHolderBase {
    public GameObject bookPrefab;
    private int booksToGenerateCount = 10;

    string []sanat1 = {"RYÖVÄTÄ", "VARASTAA", "KÄHVELTÄÄ"};
    string []sanat2 = {"NUKKUA", "UINUA", "KOISIA"};
    string []sanat3 = {"JUKSATA", "NARUTTAA", "HUIJATA"};
    string []muut = {"KAIVATA"};

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

            TextMeshPro text = book.GetComponentInChildren<TextMeshPro>();

            if (i < 3){
                text.text = sanat1[i];
            }else if (i < 6){
                text.text = sanat2[i - 3];
            }else if (i < 9){
                text.text = sanat3[i - 6];
            }else{
                text.text = muut[i - 9];
            }
            
            

            this.AddBook(book);
            Book bookScript = book.GetComponent<Book>();
            bookScript.SetCurrentHolder(this.gameObject);
        }
        this.UpdateBookPositions();
    }
}
