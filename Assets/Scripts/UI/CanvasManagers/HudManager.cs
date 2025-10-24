using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudManager : MonoBehaviour {
    public static HudManager Instance;

    [SerializeField] private TextMeshProUGUI gold;
    [SerializeField] private TextMeshProUGUI zombieCount;
    [SerializeField] private TextMeshProUGUI zombiesKilled;
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private TextMeshProUGUI playerLevel;
    [SerializeField] private TextMeshProUGUI gunLevel;
    [SerializeField] private TextMeshProUGUI ammo;
    [SerializeField] private Image health;
    [SerializeField] private Image exp;
    [SerializeField] private Image gunImage;
    [SerializeField] private List<Image> lives;
    [SerializeField] private List<GunDefinition> guns;

    [SerializeField] private GameObject logParent;
    [SerializeField] private GameObject logPref;

    private Dictionary<string, GunDefinition> gunsDict = new();
    private string activeGunName;
    private void Awake() {
        Instance = this;
        foreach (GunDefinition data in guns) {
            gunsDict[data.gunName] = data;
        }
    }

    public void UpdateLives(int val) {
        for (int i = 0; i < lives.Count; i++) {
            if (i < val) lives[i].color = Color.white;
            else lives[i].color = Color.black;
        }
    }

    public void UpdateHealth(float fillAmount) {
        health.fillAmount = fillAmount;
    }

    public void UpdateExp(float fillAmount) {
        exp.fillAmount = fillAmount;
    }

    public void UpdatePlayerLevel(int amount) {
        playerLevel.text = amount.ToString();
    }

    public void UpdateName(string value) {
        playerName.text = value;
    }

    public void UpdateGun(IGunBehaviour newGun) {
        gunImage.sprite = gunsDict[newGun.Name].gunSprite;
        string val = newGun.CurAmmo == -1 ? "∞/∞" : newGun.CurAmmo + "/" + newGun.MaxAmmo;
        ammo.text = val;
        gunLevel.text = "Lv " + newGun.CurLevel;

        activeGunName = newGun.Name;
    }

    public void UpdateGunLevel(IGunBehaviour gun) {
        if (gun.Name != activeGunName) return;
        gunLevel.text = "Lv  " + gun.CurLevel;
    }

    public void UpdateAmmo(IGunBehaviour gun) {
        if (gun.Name != activeGunName) return;
        string val = gun.CurAmmo == -1 ? "∞/∞" : gun.CurAmmo + "/" + gun.MaxAmmo;
        ammo.text = val;
    }

    public void UpdateGold(int amount) {
        gold.text = amount.ToString();
    }

    public void UpdateZombieCount(int amount) {
        // zombieCount.text = amount.ToString();
    }

    public void UpdateZombiesKilled(int amount) {
        zombiesKilled.text = amount.ToString();
    }

    public void ShowLog(string message) {
        MessageLog messageLog = Instantiate(logPref, logParent.transform).GetComponent<MessageLog>();
        messageLog.Setup(message);
        Debug.Log(message);
    }
}
