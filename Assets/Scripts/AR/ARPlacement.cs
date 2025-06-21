using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlacement : MonoBehaviour {
    public GameObject gamePrefab;
    private GameObject spawnedGame;

#if !UNITY_EDITOR
    private ARRaycastManager raycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
#endif

    void Start() {
#if !UNITY_EDITOR
        raycastManager = FindObjectOfType<ARRaycastManager>();
#endif
    }

    void Update() {
        if (spawnedGame != null) return;

#if UNITY_EDITOR
        // Editor testing: Press mouse to place object at a fixed position
        if (Input.GetMouseButtonDown(0)) {
            Vector3 testPosition = new Vector3(0, 0, 2); // 2 meters in front of camera
            spawnedGame = Instantiate(gamePrefab, testPosition, Quaternion.identity);
        }
#else
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
            Vector2 touchPos = Input.GetTouch(0).position;
            if (raycastManager.Raycast(touchPos, hits, TrackableType.PlaneWithinPolygon)) {
                Pose hitPose = hits[0].pose;
                spawnedGame = Instantiate(gamePrefab, hitPose.position, hitPose.rotation);
            }
        }
#endif
    }
}
