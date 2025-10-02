using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurretSlot : MonoBehaviour {
    [SerializeField] private Button button;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI label;

    public void UpdateData(Sprite sprite, string label) {
        this.image.sprite = sprite;
        this.label.text = label;
    }

    public void UpdateListner(Action onclick) {
        button.onClick.RemoveAllListeners();
        if (onclick != null) {
            button.onClick.AddListener(() => {
                Debug.Log("Button clicked");
                onclick();
            });
        }
    }

}
