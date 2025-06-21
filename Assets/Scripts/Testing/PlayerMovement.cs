using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour {
    public float moveSpeed = 12f;
    public float lookSpeed = 50f;

    public CharacterController controller;
    public Transform playerBody;
    public Transform cameraTransform;

    private float xRotation = 0f;

    // Movement Flags
    private bool moveForward, moveBackward, moveLeft, moveRight;

    // Camera Rotation Flags
    private bool lookUp, lookDown, lookLeft, lookRight;

    void Start() {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.None;
    }

    void Update() {
        HandleMovement();
        HandleCameraRotation();
    }

    void HandleMovement() {
        Vector3 move = Vector3.zero;

        if (moveForward) move += playerBody.forward;
        if (moveBackward) move -= playerBody.forward;
        if (moveLeft) move -= playerBody.right;
        if (moveRight) move += playerBody.right;

        controller.Move(move * moveSpeed * Time.deltaTime);
    }

    void HandleCameraRotation() {
        float camY = 0f;
        float camX = 0f;

        if (lookRight) camY += 1f;
        if (lookLeft) camY -= 1f;
        if (lookUp) camX -= 1f;
        if (lookDown) camX += 1f;

        playerBody.Rotate(Vector3.up * camY * lookSpeed * Time.deltaTime);

        xRotation += camX * lookSpeed * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    // Movement Controls
    public void MoveForward(bool pressed) => moveForward = pressed;
    public void MoveBackward(bool pressed) => moveBackward = pressed;
    public void MoveLeft(bool pressed) => moveLeft = pressed;
    public void MoveRight(bool pressed) => moveRight = pressed;

    // Camera Controls
    public void LookUp(bool pressed) => lookUp = pressed;
    public void LookDown(bool pressed) => lookDown = pressed;
    public void LookLeft(bool pressed) => lookLeft = pressed;
    public void LookRight(bool pressed) => lookRight = pressed;
}
