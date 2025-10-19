using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private List<Transform> zombiePrefs;
    public int zombiesPerSpawnPoint = 6;
    public float spawnDelay = 3;

    // private int filledSpawnPoints;
    // private List<Transform> emptySpawnPoints;
    private List<int> zombiesSpawned;
    private Dictionary<string, Transform> prefsDict = new();

    private void Start() {
        foreach (var zombie in zombiePrefs) {
            EnemyStatsData stats = zombie.GetComponent<EnemyStatsData>();
            prefsDict[stats.definition.enemyName] = zombie;
        }
    }

    private void SpawnZombie(string zombieName, int i) {
        if (!prefsDict.ContainsKey(zombieName) || spawnPoints.Count == 0) return;
        EnemyStatsData zombie = Instantiate(prefsDict[zombieName], spawnPoints[i].position, Quaternion.identity).GetComponent<EnemyStatsData>();
        int level = EnemyGlobalData.Instance.curLevel[zombieName];
        zombie.Setup(level, 0);
    }
}
