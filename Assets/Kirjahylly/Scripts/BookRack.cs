using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using System;
using System.Linq;
using UnityEngine;
using UnityEditor.AssetImporters;

public class BookRack : BookHolderBase
{
    public event EventHandler CompletionEvent;
    public bool isCompleted = false;
    private Material book_glow_material;
    private Material wrong_book_material;
    private Material book_material;

    void Start(){
        book_glow_material = Resources.Load("book_glow", typeof(Material)) as Material;
        wrong_book_material = Resources.Load("wrong_books", typeof(Material)) as Material;
        book_material = Resources.Load("books", typeof(Material)) as Material;

    }
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
            // TODO: correct books, lock rack
            LockRack();
            print("Rack is correct");
            MakeBooksGlow();
            this.OnCompletion();
        }
        else
        {
            MakeBooksRed();
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

    private void MakeBooksGlow(){
        foreach (GameObject book in this.bookStack){
            book.GetComponent<Renderer>().material = book_glow_material;
        }
    }

    private void MakeBooksRed(){
        foreach (GameObject book in this.bookStack){
            book.GetComponent<Renderer>().material = wrong_book_material;
        }
        Invoke("RestoreBookColor", 0.75f);
    }

    private void RestoreBookColor(){
        foreach (GameObject book in this.bookStack){
            book.GetComponent<Renderer>().material = book_material;
        }
    }

    private void LockRack(){
        foreach(GameObject book in this.bookStack){
            book.GetComponent<Book>().setLocked(true);
        }
    }
}
