using System.Collections.Generic;
using UnityEngine;

public class ItemsGlobalData : MonoBehaviour {
    public static ItemsGlobalData Instance { get; private set; }

    [SerializeField] private List<IItem> prefs;
    public Dictionary<string, IItem> data = new();

    private List<string> unlockedItems = new();
    public List<string> UnlockedItems => new List<string>(unlockedItems);

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        foreach (IItem item in prefs) {
            data[item.Name] = item;
            unlockedItems.Add(item.Name);
        }
    }
}
