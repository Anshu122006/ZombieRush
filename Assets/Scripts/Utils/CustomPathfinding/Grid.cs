using System;
using NUnit.Framework.Constraints;
using Unity.Collections;
using UnityEditor.Rendering;
using UnityEngine;

[System.Serializable]
public class WorldGrid {
    public int width { get; private set; }
    public int height { get; private set; }
    public float cellSize { get; private set; }
    public Vector3 origin { get; private set; }

    public GridNode[,] gridArray;

    public WorldGrid(int width, int height, float cellSize, Vector3 origin = new Vector3()) {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.origin = origin;

        gridArray = new GridNode[width, height];
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                gridArray[x, y] = new GridNode(this, x, y);
            }
        }
    }

    public void DrawGrid() {
        for (int x = 0; x < width + 1; x++) {
            Vector3 start = new Vector3(x, 0, 0) * cellSize;
            Vector3 end = new Vector3(x, height, 0) * cellSize;
            Debug.DrawLine(start, end, Color.black, 100f);
        }
        for (int y = 0; y < height + 1; y++) {
            Vector3 start = new Vector3(0, y, 0) * cellSize;
            Vector3 end = new Vector3(width, y, 0) * cellSize;
            Debug.DrawLine(start, end, Color.black, 100f);
        }
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (gridArray[x, y].isBlocked)
                    Debug.Log(gridArray[x, y].ToString());
            }
        }
    }

    public Vector3 WorldPos(int x, int y) {
        Vector3 worldPos = new Vector3(x + 0.5f, y + 0.5f, 0) * cellSize + origin;
        return worldPos;
    }

    public void GridPos(Vector3 worldPos, out int x, out int y) {
        x = Mathf.FloorToInt((worldPos - origin).x / cellSize);
        y = Mathf.FloorToInt((worldPos - origin).y / cellSize);
    }

    public bool isValid(int x, int y) {
        bool xvalid = x >= 0 && x < width;
        bool yvalid = y >= 0 && y < height;

        if (!xvalid || !yvalid) {
            return false;
        }

        // return true;

        // return xvalid && yvalid;

        GridNode n = gridArray[x, y];

        return !n.isBlocked;
    }

    public void Deserialize(GridNode[] serializedArray) {
        gridArray = new GridNode[width, height];
        for (int i = 0; i < serializedArray.Length; i++) {
            int x = i % width;
            int y = i / width;
            gridArray[x, y] = serializedArray[i];
        }
    }

    public GridNode[] Serialize() {
        GridNode[] flat = new GridNode[width * height];
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                flat[y * width + x] = gridArray[x, y];
            }
        }
        return flat;
    }
}
