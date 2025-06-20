using UnityEngine;

#nullable enable
[System.Serializable]
public class GridNode {
    public WorldGrid grid { get; private set; }
    public int x { get; private set; }
    public int y { get; private set; }

    public GridNode? parent;
    public bool isBlocked;
    public bool isChecked;
    public bool isMarked;
    public float g, h, f;


    public GridNode(WorldGrid grid, int x, int y) {
        this.x = x;
        this.y = y;
        this.grid = grid;

        isBlocked = false;
        isChecked = false;
        isMarked = false;
        g = h = f = Mathf.Infinity;

        parent = null;
    }
    public void SetHCost(GridNode end) {
        h = Mathf.Sqrt(Mathf.Pow(end.x - this.x, 2) + Mathf.Pow(end.y - this.y, 2));
    }

    public override string ToString() {
        return "(" + x + "," + y + ")";
    }

    public static bool operator >(GridNode a, GridNode b) {
        return a.f > b.f;
    }

    public static bool operator <(GridNode a, GridNode b) {
        return a.f < b.f;
    }
}