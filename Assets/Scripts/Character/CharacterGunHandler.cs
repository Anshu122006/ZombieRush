using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterGunHandler : MonoBehaviour {
    [SerializeField] private CharacterStatsData statsData;
    [SerializeField] private CharacterStatsManager statsManager;
    [SerializeField] private List<Transform> guns = new();

    private GameInputManager input;
    private List<Transform> unlocked = new();
    private int activeIndex = 0;

    public IGunBehaviour Gun => unlocked[activeIndex].GetComponent<IGunBehaviour>();
    public float GunWeight => unlocked[activeIndex].GetComponent<IGunBehaviour>().Weight;


    public void Previous(System.Object sender, EventArgs e) {
        if (activeIndex > 0) Equip(activeIndex - 1);
    }
    public void Next(System.Object sender, EventArgs e) {
        if (activeIndex < unlocked.Count - 1) Equip(activeIndex + 1);
    }

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
        Equip(activeIndex);
    }

    private void Equip(int i) {
        if (i < 0 || i >= unlocked.Count) return;
        unlocked[activeIndex].gameObject.SetActive(false);
        unlocked[i].gameObject.SetActive(true);
        activeIndex = i;

        HudManager.Instance?.UpdateGun(unlocked[activeIndex].GetComponent<IGunBehaviour>());
    }

    public void Refill(string gunName, int amount) {
        Debug.Log(unlocked);
        IGunBehaviour gun = unlocked.Find(x => x.GetComponent<IGunBehaviour>().Name == gunName)?.GetComponent<IGunBehaviour>();
        Debug.Log(gun);
        if (gun == null) return;
        amount = UnityEngine.Random.Range((int)(amount * 0.9f), (int)(amount * 1.1f));
        gun.Refill(amount);
    }
}
