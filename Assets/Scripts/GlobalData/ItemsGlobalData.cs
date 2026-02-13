using System.Collections.Generic;
using UnityEngine;

public class ItemsGlobalData : MonoBehaviour {
    public static ItemsGlobalData Instance { get; private set; }

    [SerializeField] private List<IItem> prefs;

    public float totalProb = 0;
    public Dictionary<string, IItem> data = new();

    private List<string> unlockedItems = new();
    public List<string> UnlockedItems => new List<string>(unlockedItems);

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        foreach (IItem item in prefs) {
            data[item.ItemName] = item;
            totalProb += item.chance;
        }
        if (totalProb == 0) totalProb = 1;
        unlockedItems.Add("coin");
        unlockedItems.Add("heal");
        unlockedItems.Add("life");
    }

    public void AddItem(string item) {
        if (data.ContainsKey(item)) unlockedItems.Add(item);
    }
}
