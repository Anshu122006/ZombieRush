using System.Collections.Generic;
using UnityEngine;

#nullable enable
public struct pqueue {
    List<GridNode> heap;

    public void Push(GridNode n) {
        if (heap == null)
            heap = new List<GridNode>();

        heap.Add(n);

        if (heap.Count == 1)
            return;
        heapifyup();
    }

    public void Pop() {
        if (heap == null || heap.Count == 0)
            return;
        heap[0] = heap[heap.Count - 1];
        heap.RemoveAt(heap.Count - 1);

        if (heap.Count == 0)
            return;

        heapifydown();
    }

    public GridNode? Top() {
        if (heap == null || heap.Count == 0)
            return null;
        else
            return heap[0];
    }

    public bool Empty() {
        return heap.Count == 0;
    }

    private void heapifyup() {
        int i = heap.Count - 1;

        while (i > 0) {
            int p = (i - 1) / 2;

            if (heap[i] > heap[p]) break;

            (heap[i], heap[p]) = (heap[p], heap[i]);
            i = p;
        }
    }

    private void heapifydown() {

        int i = 0;
        int c = 0;

        while (i < heap.Count - 1) {
            c = 2 * i + 1;
            if (c + 1 < heap.Count - 1 && heap[c + 1] < heap[c])
                c += 1;

            if (c > heap.Count - 1)
                break;

            if (heap[i] < heap[c])
                break;
            (heap[i], heap[c]) = (heap[c], heap[i]);
            i = c;
        }
    }
}