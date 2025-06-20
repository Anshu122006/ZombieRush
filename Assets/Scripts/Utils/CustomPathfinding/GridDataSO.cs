using UnityEngine;

[CreateAssetMenu(menuName = "Grid/Grid Data")]
public class GridDataSO : ScriptableObject {
    public int width;
    public int height;
    public float cellSize;
    public Vector3 origin;

    public GridNode[] serializedNodes;

    public void SetGrid(WorldGrid grid) {
        width = grid.width;
        height = grid.height;
        cellSize = grid.cellSize;
        origin = grid.origin;
        serializedNodes = grid.Serialize();
    }

    public WorldGrid CreateGrid() {
        WorldGrid grid = new WorldGrid(width, height, cellSize, origin);
        grid.Deserialize(serializedNodes);
        return grid;
    }
}
