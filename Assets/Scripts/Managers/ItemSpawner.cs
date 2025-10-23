using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour {
    public float spawnDelay = 3;
    [SerializeField] private int rows = 3;
    [SerializeField] private int columns = 4;
    [SerializeField] private List<Transform> itemPrefs;

    private BoxCollider2D col;
    private ItemsGlobalData globalData;
    private Dictionary<string, int> itemCount = new();
    private Dictionary<string, Transform> itemPrefsDict = new();
    private float blockWidth;
    private float blockHeight;

    private List<Vector2> blocks = new();
    private List<bool> isOccupied = new();

    private Coroutine delayCoroutine;

    private void Awake() {
        col = GetComponent<BoxCollider2D>();
    }

    private void Start() {
        globalData = ItemsGlobalData.Instance;

        foreach (var item in itemPrefs) {
            string itemName = item.GetComponent<IItem>().Name;
            itemPrefsDict[itemName] = item;
            itemCount[itemName] = 0;
        }

        Vector2 size = col.bounds.size;
        blockWidth = size.x / columns;
        blockHeight = size.y / rows;

        Vector2 start = (Vector2)transform.position - size * 0.5f;

        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < columns; j++) {
                Vector2 pos = start + new Vector2(blockWidth * j + blockWidth * 0.5f, blockHeight * i + blockHeight * 0.5f);
                blocks.Add(pos);
                isOccupied.Add(false);
            }
        }

        delayCoroutine = StartCoroutine(DelaySpawn());
    }

    private void Update() {
        if (delayCoroutine == null) {
            SpawnItem();
            delayCoroutine = StartCoroutine(DelaySpawn());
        }
    }

    private IEnumerator DelaySpawn() {
        yield return new WaitForSeconds(UnityEngine.Random.Range(spawnDelay * 0.7f, spawnDelay * 1.2f));
        delayCoroutine = null;
    }

    private void SpawnItem() {
        List<int> emptyBlocks = new();
        for (int i = 0; i < isOccupied.Count; i++)
            if (!isOccupied[i]) emptyBlocks.Add(i);

        if (emptyBlocks.Count == 0) return;

        List<string> unlocked = globalData.UnlockedItems.FindAll(z => itemCount[z] < itemPrefsDict[z].GetComponent<IItem>().MaxItemPerSpawnArea);
        if (unlocked.Count == 0) return;

        float prob = UnityEngine.Random.Range(0f, 1.4f);
        float curProb = 0;
        string itemName = "";
        for (int i = 0; i < unlocked.Count; i++) {
            float itemChance = itemPrefsDict[unlocked[i]].GetComponent<IItem>().chance / ItemsGlobalData.Instance.totalProb;
            curProb += itemChance;
            if (prob <= curProb) {
                itemName = unlocked[i];
                break;
            }
        }

        if (itemName == "") return;
        int blockId = emptyBlocks[UnityEngine.Random.Range(0, emptyBlocks.Count)];
        Vector2 blockCenter = blocks[blockId];
        Vector2 randomOffset = new Vector2(
            UnityEngine.Random.Range(-blockWidth * 0.4f, blockWidth * 0.4f),
            UnityEngine.Random.Range(-blockHeight * 0.4f, blockHeight * 0.4f)
        );

        Transform item = Instantiate(itemPrefsDict[itemName], blockCenter + randomOffset, Quaternion.identity);
        isOccupied[blockId] = true;
        itemCount[itemName]++;

        IItem iitem = item.GetComponent<IItem>();
        if (iitem != null) {
            iitem.onDestroyed = () => {
                itemCount[itemName]--;
                isOccupied[blockId] = false;
            };
        }
    }
}
