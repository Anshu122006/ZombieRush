using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    public float spawnDelay = 3;
    [SerializeField] private int maxZombieCount = 10;
    [SerializeField] private List<Transform> zombiePrefs;

    private EnemyGlobalData globalData;
    private Dictionary<string, int> zombieCount = new();
    private Dictionary<string, Transform> zombiePrefsDict = new();
    private int zombiesSpawned = 0;

    private Coroutine delayCoroutine;
    private void Start() {
        globalData = EnemyGlobalData.Instance;

        foreach (var z in zombiePrefs) {
            string zombieName = z.GetComponent<EnemyStatsData>().Name;
            zombiePrefsDict[zombieName] = z;
            zombieCount[zombieName] = 0;
        }
        delayCoroutine = StartCoroutine(DelaySpawn());
    }

    private void Update() {
        if (delayCoroutine == null) {
            SpawnZombie();
            delayCoroutine = StartCoroutine(DelaySpawn());
        }
    }

    private IEnumerator DelaySpawn() {
        float time = UnityEngine.Random.Range(spawnDelay * 0.7f, spawnDelay * 1.2f);
        yield return new WaitForSeconds(time);
        delayCoroutine = null;
    }

    private void SpawnZombie() {
        if (zombiesSpawned >= maxZombieCount) return;
        List<string> unlocked = globalData.UnlockedZombies;
        unlocked.RemoveAll(z => zombieCount[z] > zombiePrefsDict[z].GetComponent<EnemyStatsData>().MaxCountPerSpawnPoint);
        if (unlocked == null || unlocked.Count == 0) return;

        string zombieName = unlocked[UnityEngine.Random.Range(0, unlocked.Count)];
        List<Action> onDestroy = new List<Action> {
            () => {
                zombieCount[zombieName]--;
                GlobalVariables.Instance.ZombieCount--;
                GlobalVariables.Instance.ZombiesKilled++;
                zombiesSpawned--;
            },
        };
        Transform zombie = Instantiate(zombiePrefsDict[zombieName], transform.position, Quaternion.identity);
        zombie.GetComponent<EnemyStatsData>().Setup(globalData.curLevel[zombieName], onDestroy);

        zombiesSpawned++;
        zombieCount[zombieName]++;

        GlobalVariables.Instance.ZombieCount++;
    }
}
