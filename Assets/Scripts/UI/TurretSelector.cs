using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretSelector : MonoBehaviour {
    [SerializeField] private Transform pivot;
    [SerializeField] private List<Transform> slots; // 0=left, 1=up, 2=right, 3=hidden
    [SerializeField] private float rotateTime = 0.3f;

    private List<string> labels;
    private List<Sprite> sprites;
    private List<Action> onclicks;
    private int curItemIndex = 0;
    private Coroutine rotateCoroutine;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.P)) Prev();
        if (Input.GetKeyDown(KeyCode.N)) Next();
    }

    public void Setup(List<string> labels, List<Sprite> sprites, List<Action> onclicks) {
        GetComponent<Canvas>().worldCamera = Camera.main;
        if (sprites.Count != onclicks.Count) {
            Debug.LogError("Each image must have an onclick function");
            return;
        }
        this.labels = labels;
        this.sprites = sprites;
        this.onclicks = onclicks;
        UpdateSlots();
    }

    private IEnumerator Rotate(int direction) {
        // direction = +1 → anticlockwise, -1 → clockwise
        float angle = 0, speed = 90f / rotateTime;
        while (Mathf.Abs(angle) < 90f) {
            float delta = speed * Time.deltaTime * direction;
            angle += delta;
            pivot.rotation *= Quaternion.Euler(0, 0, delta);

            // keep children upright
            foreach (var slot in slots) slot.rotation = Quaternion.identity;
            yield return null;
        }

        // snap back pivot
        pivot.rotation = Quaternion.identity;
        foreach (var slot in slots) slot.rotation = Quaternion.identity;

        UpdateSlots();
        rotateCoroutine = null;
    }

    private void UpdateSlots() {
        // Hide all first
        foreach (var slot in slots) slot.gameObject.SetActive(false);

        if (sprites.Count == 0) return;

        // Always show current (slot 1)
        slots[1].gameObject.SetActive(true);
        slots[1].GetComponent<TurretSlot>().UpdateData(sprites[curItemIndex], labels[curItemIndex]);

        // Left neighbor
        if (curItemIndex > 0) {
            slots[0].gameObject.SetActive(true);
            slots[0].GetComponent<TurretSlot>().UpdateData(sprites[curItemIndex - 1], labels[curItemIndex - 1]);
        }

        // Right neighbor
        if (curItemIndex < sprites.Count - 1) {
            slots[2].gameObject.SetActive(true);
            slots[2].GetComponent<TurretSlot>().UpdateData(sprites[curItemIndex + 1], labels[curItemIndex + 1]);
        }

        // Hidden slot (slot 3) is only needed if there are >3 items 
        // and we are not at the ends
        if (sprites.Count > 3 && curItemIndex > 0 && curItemIndex < sprites.Count - 1) {
            slots[3].gameObject.SetActive(true);
        }

        for (int i = 0; i < 4; i++)
            slots[i].GetComponent<TurretSlot>().UpdateListner(i == 1 ? onclicks[curItemIndex] : null);
    }

    private void Prev() {
        if (curItemIndex == 0 || sprites.Count == 0 || rotateCoroutine != null) return;
        curItemIndex--;

        if (curItemIndex == 0) {
            slots[3].gameObject.SetActive(false);
        }
        else {
            slots[3].gameObject.SetActive(true);
            slots[3].GetComponent<TurretSlot>().UpdateData(sprites[curItemIndex - 1], labels[curItemIndex - 1]);
        }
        rotateCoroutine = StartCoroutine(Rotate(-1)); // clockwise
    }

    private void Next() {
        if (curItemIndex == sprites.Count - 1 || sprites.Count == 0 || rotateCoroutine != null) return;
        curItemIndex++;

        if (curItemIndex == sprites.Count - 1) {
            slots[3].gameObject.SetActive(false);
        }
        else {
            slots[3].gameObject.SetActive(true);
            slots[3].GetComponent<TurretSlot>().UpdateData(sprites[curItemIndex + 1], labels[curItemIndex + 1]);
        }
        rotateCoroutine = StartCoroutine(Rotate(1)); // anticlockwise
    }
}
