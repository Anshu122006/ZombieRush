using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class TilemapChunk : MonoBehaviour {
    public Vector2Int chunkCoord;
    public BoundsInt Bounds => layers[0]?.cellBounds ?? new BoundsInt();
    public List<Tilemap> layers;
}
