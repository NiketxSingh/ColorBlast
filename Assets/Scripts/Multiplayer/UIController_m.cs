using System.Collections;
using UnityEngine;

public class UIController_m : MonoBehaviour {
    private PlayerMovement_m localPlayer;

    void Start() {
        StartCoroutine(FindLocalPlayer());
    }

    IEnumerator FindLocalPlayer() {
        while (localPlayer == null) {
            foreach (var player in FindObjectsOfType<PlayerMovement_m>()) {
                if (player.IsOwner) {
                    localPlayer = player;
                    break;
                }
            }
            yield return null;
        }
    }

    // Movement Buttons
    public void OnMoveForward(bool isPressed) => localPlayer?.MoveForward(isPressed);
    public void OnMoveBackward(bool isPressed) => localPlayer?.MoveBackward(isPressed);
    public void OnMoveLeft(bool isPressed) => localPlayer?.MoveLeft(isPressed);
    public void OnMoveRight(bool isPressed) => localPlayer?.MoveRight(isPressed);

    // Camera Buttons
    public void OnLookUp(bool isPressed) => localPlayer?.LookUp(isPressed);
    public void OnLookDown(bool isPressed) => localPlayer?.LookDown(isPressed);
    public void OnLookLeft(bool isPressed) => localPlayer?.LookLeft(isPressed);
    public void OnLookRight(bool isPressed) => localPlayer?.LookRight(isPressed);
}
