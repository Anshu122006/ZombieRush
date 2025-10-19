using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour {
    [SerializeField] private List<Transform> spawnAreas;
    [SerializeField] private List<Transform> itemPrefs;
    public int maxItemsCount = 6;
    public float spawnDelay = 3;

    private float itemsSpawned = 0;
    private Dictionary<string, Transform> prefsDict = new();

    private void Start() {
        foreach (var item in itemPrefs) {
            // EnemyStatsData stats = item.GetComponent<EnemyStatsData>();
            // prefsDict[stats.definition.enemyName] = zombie;
        }
    }

    // public void SpawnItems() {
    //     List<Vector3> placed = new List<Vector3>();

    //     for (int i = 0; i < itemCount; i++) {
    //         bool placedItem = false;
    //         int tries = 0;

    //         while (!placedItem && tries < 100) {
    //             tries++;
    //             Transform area = spawnAreas[Random.Range(0, spawnAreas.Count)];
    //             Vector3 pos = GetRandomPoint(area);

    //             bool overlap = false;
    //             foreach (var p in placed)
    //                 if (Vector3.Distance(p, pos) < itemRadius * 2) { overlap = true; break; }

    //             if (!overlap) {
    //                 Instantiate(itemPrefab, pos, Quaternion.identity);
    //                 placed.Add(pos);
    //                 placedItem = true;
    //             }
    //         }
    //     }
    // }

    public void SpawnItems(string itemName, Vector2 pos) {
        if (!prefsDict.ContainsKey(itemName)) return;
        Instantiate(prefsDict[itemName], pos, Quaternion.identity);
        itemsSpawned++;
    }

    Vector3 GetRandomPoint(Transform area) {
        Vector3 size = area.localScale;
        Vector3 center = area.position;
        return new Vector3(
            Random.Range(center.x - size.x / 2, center.x + size.x / 2),
            center.y,
            Random.Range(center.z - size.z / 2, center.z + size.z / 2)
        );
    }
}
