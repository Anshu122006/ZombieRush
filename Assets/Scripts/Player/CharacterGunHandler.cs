using System.Collections.Generic;
using UnityEngine;

public class CharacterGunHandler : MonoBehaviour {
    [SerializeField] private List<Transform> guns = new();
    private List<Transform> unlocked = new();
    private int activeIndex;
    public IGunBehaviour Gun => unlocked[activeIndex].GetComponent<IGunBehaviour>();
    public SpriteRenderer GunSprite => unlocked[activeIndex].GetComponent<SpriteRenderer>();
    public float GunWeight => unlocked[activeIndex].GetComponent<IGunBehaviour>().Weight;
    public void Previous() => Equip((activeIndex + 1) % unlocked.Count);
    public void Next() => Equip((activeIndex + 1) % unlocked.Count);

    private void Equip(int i) {
        if (i < 0 || i >= guns.Count || i == activeIndex) return;
        unlocked[i].gameObject.SetActive(true);
        unlocked[activeIndex].gameObject.SetActive(false);
        activeIndex = i;
    }

    private void Start() {
        unlocked.Add(guns[0]);
    }
}
