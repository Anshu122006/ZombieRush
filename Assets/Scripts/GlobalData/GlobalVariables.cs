using System;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour {
    public event Action<int> OnGoldChanged;

    public static GlobalVariables Instance;
    [SerializeField] private int initGold;
    public List<GameVariable> gameVariables;

    private int gold;
    private Dictionary<string, int> variables;
    public int Gold {
        get { return gold; }
        set {
            gold = value >= 0 ? value : 0;
            OnGoldChanged?.Invoke(gold);
        }
    }

    private void Awake() {
        Instance = this;
        gold = initGold;

        foreach (var gameVar in gameVariables) variables[gameVar.name] = gameVar.value;
    }
    private void Start() {
        OnGoldChanged?.Invoke(gold);
    }
}
