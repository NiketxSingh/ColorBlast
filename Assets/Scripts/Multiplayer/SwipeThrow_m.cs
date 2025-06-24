using Unity.Netcode;
using UnityEngine;

public class SwipeThrow_m : NetworkBehaviour {
    [SerializeField] private GameObject stonePrefab;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private float forceMultiplier = 0.05f;

    private Vector2 swipeStartPos;
    private Vector2 swipeEndPos;
    private bool isSwiping = false;

    void Update() {
        if (!IsOwner) return;

        if (Input.touchSupported && Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);
            HandleTouch(touch.phase, touch.position);
        }
        else {
            if (Input.GetMouseButtonDown(0)) {
                swipeStartPos = Input.mousePosition;
                isSwiping = true;
            }
            else if (Input.GetMouseButtonUp(0) && isSwiping) {
                swipeEndPos = Input.mousePosition;
                ShootFromSwipe();
                isSwiping = false;
            }
        }
    }

    void HandleTouch(TouchPhase phase, Vector2 position) {
        if (phase == TouchPhase.Began) {
            swipeStartPos = position;
        }
        else if (phase == TouchPhase.Ended) {
            swipeEndPos = position;
            ShootFromSwipe();
        }
    }

    void ShootFromSwipe() {
        Vector2 swipeVector = swipeEndPos - swipeStartPos;
        float swipeLength = swipeVector.magnitude;

        if (swipeLength < 15f) return;

        Vector3 direction = new Vector3(swipeVector.x, swipeVector.y, swipeLength).normalized;
        Vector3 forceDir = Camera.main.transform.TransformDirection(direction);

        ThrowStoneServerRpc(forceDir * swipeLength * forceMultiplier);
    }

    [ServerRpc]
    void ThrowStoneServerRpc(Vector3 force, ServerRpcParams rpcParams = default) {
        if (stonePrefab == null) {
            Debug.LogError("[Server] Stone prefab not assigned!");
            return;
        }

        GameObject stone = Instantiate(stonePrefab, throwPoint.position, Quaternion.identity);
        NetworkObject netObj = stone.GetComponent<NetworkObject>();

        if (netObj == null) {
            Debug.LogError("[Server] Stone prefab missing NetworkObject component!");
            return;
        }

        netObj.SpawnWithOwnership(rpcParams.Receive.SenderClientId);
        Debug.Log($"[Server] Spawned stone for client {rpcParams.Receive.SenderClientId}");

        // Setup stone ownership and behavior
        var stoneScript = stone.GetComponent<Stone_m>();
        if (stoneScript != null) {
            stoneScript.SetOwner(rpcParams.Receive.SenderClientId);
        }
        else {
            Debug.LogWarning("[Server] Spawned stone has no Stone_m script.");
        }

        Rigidbody rb = stone.GetComponent<Rigidbody>();
        if (rb == null) {
            Debug.LogError("[Server] Stone prefab missing Rigidbody component!");
            return;
        }

        rb.isKinematic = false;
        rb.AddForce(force, ForceMode.Impulse);
    }
}
