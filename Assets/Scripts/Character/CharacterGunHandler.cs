using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGunHandler : MonoBehaviour {
    [SerializeField] private CharacterStatsData statsData;
    [SerializeField] private CharacterStatsManager statsManager;
    [SerializeField] private List<Transform> guns = new();

    private GameInputManager input;
    private List<Transform> unlocked = new();
    private int activeIndex;

    public IGunBehaviour Gun => unlocked[activeIndex].GetComponent<IGunBehaviour>();
    public float GunWeight => unlocked[activeIndex].GetComponent<IGunBehaviour>().Weight;


    public void Previous(System.Object sender, EventArgs e) => Equip((activeIndex + 1) % unlocked.Count);
    public void Next(System.Object sender, EventArgs e) => Equip((activeIndex + 1) % unlocked.Count);

    private void Start() {
        input = GameInputManager.Instance;
        input.OnNextWeaponPerformed += Next;
        input.OnPrevWeaponPerformed += Previous;

        // unlocked.Add(guns[0]);
        foreach (var gun in guns) {
            gun.GetComponent<IGunBehaviour>().CharStatData = statsData;
            gun.GetComponent<IGunBehaviour>().CharStatManager = statsManager;
            gun.gameObject.SetActive(false);
            unlocked.Add(gun);
        }
        activeIndex = 0;
        unlocked[0].gameObject.SetActive(true);
    }

    private void Equip(int i) {
        if (i < 0 || i >= guns.Count || i == activeIndex) return;
        unlocked[activeIndex].gameObject.SetActive(false);
        unlocked[i].gameObject.SetActive(true);
        activeIndex = i;
    }
}
