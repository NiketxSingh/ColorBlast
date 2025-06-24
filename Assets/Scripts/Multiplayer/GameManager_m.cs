using System;
using Unity.Netcode;
using UnityEngine;

public class GameManager_m : NetworkBehaviour {

    [Header("Grid Variables")]
    [SerializeField] private int length = 4;
    [SerializeField] private int width = 4;
    [SerializeField] private int height = 4;

    public static MasterGrid masterGrid;

    public override void OnNetworkSpawn() {
        if (IsServer) {
            Debug.Log("GameManager OnNetworkSpawn called, creating masterGrid");
            masterGrid = new MasterGrid(length, width, height);
        }
        else {
            Debug.Log("GameManager OnNetworkSpawn (not server)");
        }
    }
}
