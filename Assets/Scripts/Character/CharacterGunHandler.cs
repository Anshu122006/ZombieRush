using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterGunHandler : MonoBehaviour {
    [SerializeField] private CharacterStatsData statsData;
    [SerializeField] private CharacterStatsManager statsManager;
    [SerializeField] private AudioClip equipSound;
    [SerializeField] private List<Transform> guns = new();

    private GameInputManager input;
    private List<Transform> unlocked = new();
    private int activeIndex = 0;

    private Dictionary<string, Transform> gunsDict = new();

    public IGunBehaviour Gun => unlocked[activeIndex].GetComponent<IGunBehaviour>();
    public float GunWeight => unlocked[activeIndex].GetComponent<IGunBehaviour>().Weight;

    private void Awake() {
        foreach (var x in guns) {
            IGunBehaviour gun = x.GetComponent<IGunBehaviour>();
            gun.CharStatData = statsData;
            gun.CharStatManager = statsManager;
            x.gameObject.SetActive(false);
            gunsDict[gun.Name] = x;
        }
        unlocked.Add(gunsDict["pistol"]);
        Equip(activeIndex);
    }

    private void Start() {
        input = GameInputManager.Instance;
        input.OnNextWeaponPerformed += Next;
        input.OnPrevWeaponPerformed += Previous;
    }

    public void Previous(System.Object sender, EventArgs e) {
        if (activeIndex > 0) Equip(activeIndex - 1);
    }
    public void Next(System.Object sender, EventArgs e) {
        if (activeIndex < unlocked.Count - 1) Equip(activeIndex + 1);
    }

    private void Equip(int i) {
        if (i < 0 || i >= unlocked.Count) return;

        GameAudioManager.Instance.PlaySound(equipSound, transform.position);
        unlocked[activeIndex].gameObject.SetActive(false);
        unlocked[i].gameObject.SetActive(true);
        activeIndex = i;
        Gun.AbortShoot();

        HudManager.Instance?.UpdateGun(unlocked[activeIndex].GetComponent<IGunBehaviour>());
    }

    public void Refill(string gunName, int amount) {
        // Debug.Log(unlocked);
        IGunBehaviour gun = unlocked.Find(x => x.GetComponent<IGunBehaviour>().Name == gunName)?.GetComponent<IGunBehaviour>();
        Debug.Log(gun);
        if (gun == null) return;
        amount = UnityEngine.Random.Range((int)(amount * 0.9f), (int)(amount * 1.1f));
        gun.Refill(amount);
    }

    public void AddGun(int curLevel) {
        switch (curLevel) {
            case 2:
                string gunName = "smg";
                unlocked.Add(gunsDict[gunName]);
                ItemsGlobalData.Instance.AddItem(gunName);
                HudManager.Instance?.ShowLog(gunName + " has been unlocked");
                break;
            case 3:
                gunName = "shotgun";
                unlocked.Add(gunsDict[gunName]);
                ItemsGlobalData.Instance.AddItem(gunName);
                HudManager.Instance?.ShowLog(gunName + " has been unlocked");
                break;
            case 5:
                gunName = "grenade";
                unlocked.Add(gunsDict[gunName]);
                ItemsGlobalData.Instance.AddItem(gunName);
                HudManager.Instance?.ShowLog(gunName + " has been unlocked");
                break;
            case 6:
                gunName = "flamethrower";
                unlocked.Add(gunsDict[gunName]);
                ItemsGlobalData.Instance.AddItem(gunName);
                HudManager.Instance?.ShowLog(gunName + " has been unlocked");
                break;
            case 10:
                gunName = "minigun";
                unlocked.Add(gunsDict[gunName]);
                ItemsGlobalData.Instance.AddItem(gunName);
                HudManager.Instance?.ShowLog(gunName + " has been unlocked");
                break;
        }
    }
}
