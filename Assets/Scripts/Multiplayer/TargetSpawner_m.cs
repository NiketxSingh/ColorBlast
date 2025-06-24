using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TargetSpawner_m : NetworkBehaviour {

    public static TargetSpawner_m Instance;

    [Header("Grid Variables")]
    [SerializeField] private int length = 4;
    [SerializeField] private int width = 4;
    [SerializeField] private int height = 4;

    public static MasterGrid masterGrid;

    [SerializeField] private List<GameObject> targetPrefabs;
    [SerializeField] private int maxTargets = 10;

    private int currentTargets;
    private bool isRespawning = false;

    private void Awake() {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public override void OnNetworkSpawn() {
        if (!IsServer) return;

        Debug.Log("TargetSpawner OnNetworkSpawn called, creating masterGrid");
        masterGrid = new MasterGrid(length, width, height);

        StartCoroutine(DelayedSpawn());
    }

    private IEnumerator DelayedSpawn() {
        yield return new WaitUntil(() =>
            masterGrid != null &&
            masterGrid.GetLength() > 0
        );

        currentTargets = 0;
        for (int i = 0; i < maxTargets; i++) {
            SpawnGridTarget();
        }
        Debug.Log("SPAWNED INITIAL TARGETS");
    }

    void Update() {
        if (!IsServer) return;

        if (currentTargets < maxTargets && !isRespawning) {
            StartCoroutine(StartRespawn());
        }
    }

    public void DecreaseTargets() {
        currentTargets = Mathf.Max(0, currentTargets - 1);
    }

    void SpawnGridTarget() {
        int x, y, z;
        int tries = 0;

        //if (masterGrid == null) {
        //    Debug.LogError("masterGrid is NULL");
        //    return;
        //}
        //Debug.Log("masterGrid exists");

        do {
            x = Random.Range(0, masterGrid.GetLength());
            y = Random.Range(0, masterGrid.GetWidth());
            z = Random.Range(0, masterGrid.GetHeight());
            tries++;
            if (tries > 100) return;
        }
        while (masterGrid.GetCellValue(x, y, z));

        Vector3 spawnPos = masterGrid.GridToWorld(x, y, z);
        masterGrid.SwitchCellValue(x, y, z);

        GameObject prefab = targetPrefabs[Random.Range(0, targetPrefabs.Count)];
        if (prefab == null) {
            Debug.LogError("Null prefab in targetPrefabs list!");
            return;
        }
        GameObject target = Instantiate(prefab, spawnPos, Quaternion.identity);

        var netObj = target.GetComponent<NetworkObject>();
        if (netObj != null) {
            netObj.Spawn(true);
        }
        else {
            Debug.LogError($"[TargetSpawner] Target prefab '{prefab.name}' is missing NetworkObject component!");
        }

        currentTargets++;
    }

    private IEnumerator StartRespawn() {
        isRespawning = true;
        yield return new WaitForSeconds(1f);
        SpawnGridTarget();
        isRespawning = false;
    }
}
