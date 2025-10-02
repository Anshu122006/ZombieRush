using TMPro;
using UnityEngine;

public class GoldUIManager : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI goldText;
    private string label = "Gold";
    private string separator = " : ";

    private void Start() {
        GlobalVariables.Instance.OnGoldChanged += UpdateText;
    }

    private void UpdateText(int gold) {
        goldText.text = label + separator + gold.ToString();
    }
}
