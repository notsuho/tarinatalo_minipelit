using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour {
    private float distanceToCamera;
    private bool bookMoving = false;
    private float moveSpeed = 7.0f;

    private Material book_glow_material;
    private Material wrong_book_material;
    private Material book_material;

    private Vector3 clickOffSet;

    private GameObject currHolder;
    private Vector3 targetPosition;
    private Quaternion targetRotation;

    public int word_category;

    void Start() {
        book_glow_material = Resources.Load("book_glow", typeof(Material)) as Material;
        wrong_book_material = Resources.Load("wrong_books", typeof(Material)) as Material;
        book_material = Resources.Load("books", typeof(Material)) as Material;
    }

    void OnMouseDown() {
        if (this.IsInCompletedRack()) {
            return;
        }
        /*
            Update variables related to calculating transforms to keep the book object
            under the mouse while moving.
            Set moving to false to prevent book flying while dragging.
        */
        distanceToCamera = Camera.main.WorldToScreenPoint(transform.position).z;
        clickOffSet = transform.position - GetMouseWorldPos();
        bookMoving = false;
    }

    void OnMouseUp() {
        /*
            Check if object under mouse can hold books.
            if it can, remove the book from its current holder and add it to the new holder.
        */
        GameObject objectUnderMouse = GetObjectUnderMouse();
        if (objectUnderMouse) {
            BookHolderBase newBookHolder = objectUnderMouse.GetComponent<BookHolderBase>();
            if (newBookHolder.CanHoldMoreBooks()) {
                BookHolderBase prevBookHolder = this.currHolder.GetComponent<BookHolderBase>();
                prevBookHolder.RemoveBook(this.gameObject);
                newBookHolder.AddBook(this.gameObject);
                this.currHolder = objectUnderMouse;
            }
        }
        this.bookMoving = true;
    }

    public void SetNewTargetPositionAndRotation(Vector3 newPos, Vector3 newRot) {
        /*
            Book holders call this method to set new location the book should fly to.
        */
        this.targetPosition = newPos;
        this.targetRotation = Quaternion.Euler(newRot);
        this.bookMoving = true;
    }

    void OnMouseDrag() {
        /*
            Update book object position in scene while dragging.
        */
        if (this.bookMoving || this.IsInCompletedRack()) {
            return;
        }
        
        Vector3 newPos = GetMouseWorldPos() + clickOffSet;
        transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
    }

    GameObject GetObjectUnderMouse() {
        /*
            Get objects under mouse position that can hold books
        */
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray, Mathf.Infinity);

        foreach (RaycastHit hit in hits) {
            GameObject hitObject = hit.collider.gameObject;
            if (hitObject.tag == "CanHoldBooks") {
                return hitObject;
            }
        }
        return null;
    }

    Vector3 GetMouseWorldPos() {
        /*
            Translate mouse position to scene position
        */
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = distanceToCamera;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    void Update() {
        /*
            Move book to its target position if its not there already
        */
        if (this.bookMoving) {
            Vector3 newPos = Vector3.Lerp(transform.position, this.targetPosition, this.moveSpeed * Time.deltaTime);
            Quaternion newRot = Quaternion.Lerp(transform.rotation, this.targetRotation, this.moveSpeed * Time.deltaTime);
            transform.position = newPos;
            transform.rotation = newRot;
            if (transform.position == this.targetPosition && transform.rotation == this.targetRotation) {
                this.bookMoving = false;
            }
        }
    }

    bool IsInCompletedRack() {
        return
            this.currHolder.GetComponent<BookRack>() &&
            this.currHolder.GetComponent<BookRack>().isCompleted;
    }

    public void SetCurrentHolder(GameObject holder) {
        /*
            Only called when table is created to set initial holder
        */
        this.currHolder = holder;
    }

    public int GetWordCategory() {
        return word_category;
    }

    public void MakeBookGlow() {
        this.gameObject.GetComponent<Renderer>().material = book_glow_material;
    }

    public void MakeBookRed() {
        this.gameObject.GetComponent<Renderer>().material = wrong_book_material;
        Invoke("RestoreBookColor", 0.75f);
    }

    void RestoreBookColor() {
        this.gameObject.GetComponent<Renderer>().material = book_material;
    }
}
