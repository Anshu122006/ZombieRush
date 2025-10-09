using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapChunker : MonoBehaviour {
    [SerializeField] private TilemapChunk basemap;
    [SerializeField] private TileChunkLoader loader;
    [SerializeField] private GameObject chunkPrefab;
    [SerializeField] private int chunkSize = 32;

    void Start() {
        BoundsInt bounds = basemap.Bounds;
        for (int x = bounds.xMin; x < bounds.xMax; x += chunkSize) {
            for (int y = bounds.yMin; y < bounds.yMax; y += chunkSize) {
                // Create chunk
                TilemapChunk chunk = Instantiate(chunkPrefab, loader.transform).GetComponent<TilemapChunk>();
                chunk.transform.position = new Vector3(x, y, 0);
                // loader.chunks.Add(chunk);

                // Copy tiles from original map to chunk
                for (int l = 0; l < chunk.layers.Count; l++) {
                    for (int i = 0; i < chunkSize; i++) {
                        for (int j = 0; j < chunkSize; j++) {
                            Vector3Int tilePos = new Vector3Int(x + i, y + j, 0);
                            TileBase tile = basemap.layers[l].GetTile(tilePos);
                            if (tile != null)
                                chunk.layers[l].SetTile(new Vector3Int(i, j, 0), tile);
                        }
                    }
                }

                chunk.gameObject.SetActive(false);
            }
        }

        // Optionally, disable original tilemap
        Destroy(basemap.gameObject);
        Destroy(gameObject);
    }
}
