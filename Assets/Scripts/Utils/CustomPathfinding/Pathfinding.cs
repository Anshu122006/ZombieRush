using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CPathfinding {
    public List<GridNode> path;
    public Vector3 currPath;
    private int[,] directions = { { 0, 1 }, { 1, 0 }, { -1, 0 }, { 0, -1 }, { -1, -1 }, { -1, 1 }, { 1, -1 }, { 1, 1 } };

    public bool FindPath(WorldGrid grid, GridNode start, GridNode end) {
        if (!grid.isValid(start.x, start.y) || !grid.isValid(end.x, end.y)) {
            Debug.Log("Start or end is not valid");
            return false;
        }

        pqueue neighbours = new pqueue();
        start.g = 0;
        start.SetHCost(end);
        start.f = start.g + start.h;
        neighbours.Push(start);

        while (!neighbours.Empty()) {
            GridNode currNode = neighbours.Top();
            currNode.isChecked = true;
            neighbours.Pop();

            if (currNode.x == end.x && currNode.y == end.y) {
                SetCurrPath(currNode, grid);
                CleanGrid(grid);
                return true;
            }

            int x = currNode.x, y = currNode.y;

            for (int i = 0; i < 8; i++) {
                int dx = directions[i, 0];
                int dy = directions[i, 1];

                int nx = x + dx;
                int ny = y + dy;

                if (!grid.isValid(nx, ny))
                    continue;

                GridNode n = grid.gridArray[nx, ny];

                if (n.isChecked)
                    continue;

                float cost = (dx == 0 || dy == 0) ? 1f : 1.4142f;
                float ng = currNode.g + cost;

                if (ng < n.g) {
                    n.g = ng;
                    n.SetHCost(end);
                    n.f = n.g + n.h;
                    n.parent = currNode;

                    if (!n.isMarked) {
                        n.isMarked = true;
                        neighbours.Push(n);
                    }
                }
            }

        }

        CleanGrid(grid);
        return false;
    }

    private void TracePath(GridNode end) {
        path = new List<GridNode>();

        GridNode n = end;

        while (n.parent != null) {
            path.Add(n);
            n = n.parent;
        }

        path.Add(n);
        path.Reverse();
    }

    private void SetCurrPath(GridNode end, WorldGrid grid) {

        GridNode n = end;

        while (n.parent.parent != null) {
            n = n.parent;
        }

        currPath = grid.WorldPos(n.x, n.y);
    }

    private void CleanGrid(WorldGrid grid) {
        for (int x = 0; x < grid.width; x++) {
            for (int y = 0; y < grid.height; y++) {
                grid.gridArray[x, y].isChecked = false;
                grid.gridArray[x, y].isMarked = false;
                grid.gridArray[x, y].g = Mathf.Infinity;
                grid.gridArray[x, y].h = Mathf.Infinity;
                grid.gridArray[x, y].f = Mathf.Infinity;
            }
        }
    }

    public void ShowPath() {
        if (path == null) {
            Debug.Log("path is null");
        }
        foreach (GridNode n in path) {
            Debug.Log("(" + n.x + "," + n.y + ")");
        }
    }
}
