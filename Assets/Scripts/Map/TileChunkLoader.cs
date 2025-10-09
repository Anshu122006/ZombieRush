using System.Collections.Generic;
using UnityEngine;

public class TileChunkLoader : MonoBehaviour {
    public Transform player;
    public float loadRadius = 10f;
    public float loadMargin = 2f;
    [SerializeField] private int chunkSize = 32;

    void Update() {
        TilemapChunk[] newChunks = FindObjectsByType<TilemapChunk>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None
        );

        foreach (var chunk in newChunks) {
            Vector3 chunkCenter = chunk.transform.position + new Vector3(chunkSize / 2f, chunkSize / 2f, 0);
            float dist = Vector3.Distance(player.position, chunkCenter);

            chunk.gameObject.SetActive(dist < (loadRadius + loadMargin));
        }
    }
}
