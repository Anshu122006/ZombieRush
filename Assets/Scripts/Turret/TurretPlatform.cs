using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurretPlatform : MonoBehaviour {
    [SerializeField] private TurretSelector turretSelector;
    [SerializeField] private OptionSelector optionSelector;
    [SerializeField] private List<TurretDefinition> turretData = new();
    [SerializeField] private List<GameObject> turretPref = new();

    private Dictionary<string, TurretDefinition> turretDataDict = new();
    private Dictionary<string, GameObject> turretPrefDict = new();

    private bool isOccupied = false;
    private GameObject placedTurret;

    private void Start() {
        for (int i = 0; i < turretData.Count; i++) {
            turretDataDict[turretData[i].turretName] = turretData[i];
            turretPrefDict[turretData[i].turretName] = turretPref[i];
        }
        UpdateTurretsList();
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D col = GetComponent<Collider2D>();
            if (col != null && col.OverlapPoint(mousePos)) {
                OnPlatformClicked();
            }
        }
    }

    private void UpdateTurretsList() {
        List<string> labels = new();
        List<Sprite> sprites = new();
        List<Action> onclicks = new();
        foreach (var data in turretData) {
            if (GlobalTurretData.Instance.isUnlocked[data.turretName]) {
                labels.Add(data.cost.ToString());
                sprites.Add(data.sprite);
                onclicks.Add(() => PlaceTurret(data.turretName));
            }
        }
        turretSelector.Setup(labels, sprites, onclicks);
    }


    private void OnPlatformClicked() {
        if (!isOccupied) ShowTurrets();
        else ShowOptions();
    }

    private void ShowTurrets() {
        if (!turretSelector.gameObject.activeSelf) {
            turretSelector.gameObject.SetActive(true);
            optionSelector.gameObject.SetActive(false);
        }
        else {
            turretSelector.gameObject.SetActive(false);
        }
    }

    private void ShowOptions() {
        ITurretBehaviour turret = placedTurret.GetComponent<ITurretBehaviour>();
        if (turret == null) return;

        int upgradeCost = turret.UpgradeCost;
        int demolishReturn = (int)(turretDataDict[turret.Name].cost * 0.8f);

        if (!optionSelector.gameObject.activeSelf) {
            optionSelector.gameObject.SetActive(true);
            turretSelector.gameObject.SetActive(false);
            optionSelector.UpdateData(upgradeCost, demolishReturn, RepairTurret, UpgradeTurret, DemolishTurret);
        }
        else {
            optionSelector.gameObject.SetActive(false);
        }
    }


    private void PlaceTurret(string turretName) {
        if (!CanPlaceTurret(turretName)) return;

        placedTurret = Instantiate(turretPrefDict[turretName], transform.position, Quaternion.identity);
        isOccupied = true;

        int goldRequired = turretDataDict[turretName].cost;
        GlobalVariables.Instance.Gold -= goldRequired;
        turretSelector.gameObject.SetActive(false);
    }

    private void UpgradeTurret() {
        ITurretBehaviour turret = placedTurret.GetComponent<ITurretBehaviour>();
        if (turret == null) return;
        if (CanUpgradeTurret(turret.Name, out int upgradeCost)) {
            GlobalVariables.Instance.Gold -= upgradeCost;
            turret.LevelUp();

            optionSelector.gameObject.SetActive(false);
        }
    }

    private void DemolishTurret() {
        if (!isOccupied) return;

        ITurretBehaviour turret = placedTurret.GetComponent<ITurretBehaviour>();
        int goldReturned = (int)(turretDataDict[turret.Name].cost * 0.8f);
        GlobalVariables.Instance.Gold += goldReturned;

        Destroy(placedTurret);
        placedTurret = null;
        isOccupied = false;

        optionSelector.gameObject.SetActive(false);
    }

    private void RepairTurret() {
        ITurretBehaviour turret = placedTurret.GetComponent<ITurretBehaviour>();
        if (CanRepairTurret(turret.Name, out int repairCost)) {
            turret.HP = turret.MHP;
            GlobalVariables.Instance.Gold -= repairCost;
            optionSelector.gameObject.SetActive(false);
        }
    }


    private bool CanPlaceTurret(string turretName) {
        int curGold = GlobalVariables.Instance.Gold;
        int goldRequired = turretDataDict[turretName].cost;

        return !isOccupied && curGold >= goldRequired;
    }

    private bool CanRepairTurret(string turretName, out int repairCost) {
        int curGold = GlobalVariables.Instance.Gold;
        repairCost = turretDataDict[turretName].cost;

        return !isOccupied && curGold >= repairCost;
    }

    private bool CanUpgradeTurret(string turretName, out int upgradeCost) {
        int curGold = GlobalVariables.Instance.Gold;
        upgradeCost = placedTurret.GetComponent<ITurretBehaviour>().UpgradeCost;
        int curLevel = placedTurret.GetComponent<ITurretBehaviour>().CurLevel;
        int maxLevel = GlobalTurretData.Instance.curLevel[turretName];

        return isOccupied && curLevel < maxLevel && curGold >= upgradeCost;
    }
}

