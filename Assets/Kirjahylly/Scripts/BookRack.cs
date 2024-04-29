using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using System;
using System.Linq;
using UnityEngine;

public class BookRack : BookHolderBase
{
    public event EventHandler CompletionEvent;
    private BookManager manager;
    private UIManager_Kirjahylly ui;
    public bool isCompleted = false;

    public void Start(){
        manager = FindObjectOfType<BookManager>();
        ui = FindObjectOfType<UIManager_Kirjahylly>();
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
            MakeBooksGlow();
            this.OnCompletion();
            manager.PlayCorrectSound();
            if (GameManager.streak > 2){
                ui.DisplayStreakImage();
            }
        }
        else
        {
            GameManager.AddPoints(false, manager.pointsForWrongAnswer);
            ui.UpdateScoreLabel();
            MakeBooksRed();
            manager.PlayWrongSound();
        }

    }

    bool BooksAreCorrect()
    {
        int? firstCategory = this.bookStack[0].GetComponent<Book>().word_category;
        if (firstCategory == null) {
            return false;
        }
        return this.bookStack.All(b => b.GetComponent<Book>().word_category == firstCategory);
    }

    public void OnCompletion() {
        // Calls BookManagers method to check if all other book racks
        // are completed too and if the next level should be loaded.
        this.isCompleted = true;
        CompletionEvent?.Invoke(this, EventArgs.Empty);
    }

    private void MakeBooksGlow() {
        foreach (GameObject book in this.bookStack){
            book.GetComponent<GlowControl>().MakeBookGlow();
            book.GetComponent<Book>().isRed = false;
        }
    }

    private void MakeBooksRed() {
        foreach (GameObject book in this.bookStack){
            book.GetComponent<GlowControl>().MakeBookRed();
            book.GetComponent<Book>().isRed = true;
        }
    }

    public void RestoreBookColors() {
        foreach (GameObject book in this.bookStack){
            book.GetComponent<GlowControl>().RestoreBookColor();
            book.GetComponent<Book>().isRed = false;
        }
    }

    public List<GameObject> GetBookStack(){
        return this.bookStack;
    }
}
