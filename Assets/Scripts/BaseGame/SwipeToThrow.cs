using UnityEngine;

public class SwipeToThrow : MonoBehaviour {

    [SerializeField] private GameObject stonePrefab;
    [SerializeField] private Transform throwPoint;
    private Vector2 swipeStartPos;
    private Vector2 swipeEndPos;
    private bool isSwiping = false;

    [SerializeField] private float forwardForceMultiplier = 1f;
    [SerializeField] private float upForceMultiplier = 0.5f;

    void Update() {
        if (GameManager.Instance == null) return;

        if (Input.touchSupported && Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);
            HandleTouch(touch.phase, touch.position);
        }
        else if (Input.GetMouseButtonDown(0)) {
            swipeStartPos = Input.mousePosition;
            isSwiping = true;
        }
        else if (Input.GetMouseButtonUp(0) && isSwiping) {
            swipeEndPos = Input.mousePosition;
            ShootFromSwipe();
            isSwiping = false;
        }
    }

    void HandleTouch(TouchPhase phase, Vector2 position) {
        switch (phase) {
            case TouchPhase.Began:
                swipeStartPos = position;
                break;
            case TouchPhase.Ended:
                swipeEndPos = position;
                ShootFromSwipe();
                break;
        }
    }

    void ShootFromSwipe() {
        Vector2 swipeVector = swipeEndPos - swipeStartPos;
        float swipeLength = swipeVector.magnitude;

        if (swipeLength < 15f) return; 

        Vector3 forwardDir = Camera.main.transform.forward;
        Vector3 upDir = Camera.main.transform.up;

        Vector3 throwDir = (forwardDir * forwardForceMultiplier) + (upDir * swipeVector.y * upForceMultiplier * 0.001f);

        GameObject stone = Instantiate(stonePrefab, throwPoint.position, Quaternion.identity);
        Rigidbody rb = stone.GetComponent<Rigidbody>();

        rb.isKinematic = false;
        rb.AddForce(throwDir.normalized * swipeLength * 0.05f, ForceMode.Impulse);
    }
}
