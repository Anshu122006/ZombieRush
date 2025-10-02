using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionSelector : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI upgradeCost;
    [SerializeField] private TextMeshProUGUI demolishReturn;

    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button demolishButton;

    private void Awake() {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }
    public void UpdateData(int upgradeCost, int demolishReturn, Action onUpgrade, Action onDemolish) {
        this.upgradeCost.text = upgradeCost.ToString();
        this.demolishReturn.text = demolishReturn.ToString();

        upgradeButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.AddListener(() => onUpgrade());
        demolishButton.onClick.RemoveAllListeners();
        demolishButton.onClick.AddListener(() => onDemolish());
    }
}
