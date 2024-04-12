using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Book : MonoBehaviour {
    private float distanceToCamera;
    private bool bookMoving = false;
    private bool bookZooming = false;
    private bool bookFrozen = false;
    private float moveSpeed = 7.0f;
    private float zoomDistanceToCamera = 2.0f; // How close to zoom the book when dragging

    private Vector3 clickOffSet;
    [SerializeField]
    private GameObject currHolder;
    private Vector3 targetPosition;
    private Quaternion targetRotation;

    public int word_category;
    private UIManager_Kirjahylly ui;

    void Start() {
        ui = FindObjectOfType<UIManager_Kirjahylly>();
    }

    void OnMouseDown() {
        if (this.IsInCompletedRack() || ui.InstructionsShown() || this.bookFrozen) {
            return;
        }
        /*
            Update variables related to calculating transforms to keep the book object
            under the mouse while moving.
            Set moving to false to prevent book flying while dragging.
        */

        this.distanceToCamera = Camera.main.WorldToScreenPoint(transform.position).z;
        this.clickOffSet = transform.position - GetMouseWorldPos();
        this.bookMoving = false;
        this.bookZooming = true;
    }

    void OnMouseUp() {
        /*
            Check if object under mouse can hold books.
            if it can, remove the book from its current holder and add it to the new holder.
        */
        if (this.bookMoving || this.IsInCompletedRack() || ui.InstructionsShown() || this.bookFrozen) {
            return;
        }

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
        this.bookZooming = false;
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
        if (this.bookMoving || this.IsInCompletedRack() || ui.InstructionsShown() || this.bookFrozen) {
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

        if (this.bookZooming) {
            // Move book towards camera position when dragging
            Vector3 directionToCamera = Camera.main.transform.position - transform.position;
            this.distanceToCamera = Camera.main.WorldToScreenPoint(transform.position).z;
            directionToCamera.Normalize();
            Vector3 zoomTargetPosition = transform.position + directionToCamera * 0.5f;
            Vector3 newPos = Vector3.Lerp(
                transform.position,
                zoomTargetPosition,
                this.moveSpeed * Time.deltaTime
            );
            transform.position = newPos;
            if (this.distanceToCamera < this.zoomDistanceToCamera) {
                this.bookZooming = false;
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

    public void SetBookFrozen(){
        bookFrozen = true;
    }

    public GameObject GetCurrHolder(){
        return currHolder;
    }
}