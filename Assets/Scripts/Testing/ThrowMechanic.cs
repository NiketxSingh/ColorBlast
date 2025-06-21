using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowMechanic : MonoBehaviour {
    private Vector2 startTouchPos;
    private Vector2 endTouchPos;

    [SerializeField] private Transform playerBody;
    [SerializeField] private GameObject stonePrefab;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private float velocityMultiplier = 0.05f;
    [SerializeField] private float verticalMultiplier = 0.01f;

    void Update() {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0)) startTouchPos = Input.mousePosition;
        if (Input.GetMouseButtonUp(0)) {
            endTouchPos = Input.mousePosition;
            Vector2 swipeDir = endTouchPos - startTouchPos;
            ThrowProjectile(swipeDir);
        }
#else
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began) startTouchPos = touch.position;
            if (touch.phase == TouchPhase.Ended)
            {
                endTouchPos = touch.position;
                Vector2 swipeDir = endTouchPos - startTouchPos;
                ThrowProjectile(swipeDir);
            }
        }
#endif
    }

    void ThrowProjectile(Vector2 swipeDir) {
        //Vector3 forwardDir = throwPoint.position - playerBody.position;
        float upDirection = swipeDir.y * verticalMultiplier;
        Vector3 finalDir = new Vector3(swipeDir.x, upDirection, swipeDir.magnitude);
        GameObject obj = Instantiate(stonePrefab, throwPoint.position, Quaternion.identity);
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        //Vector3 throwVelocity = new Vector3(finalDir.x,finalDir.y * verticalMultiplier,finalDir.z * swipeDir.magnitude);
        rb.AddForce(finalDir * velocityMultiplier , ForceMode.Impulse);
    }
}
