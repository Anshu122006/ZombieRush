using System;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour {
    public static GlobalVariables Instance;

    [SerializeField] private int initGold;
    public List<GameVariable> gameVariables;

    private int gold;
    private int zombieCount;

    private Dictionary<string, int> variables;
    public int Gold {
        get { return gold; }
        set {
            gold = value >= 0 ? value : 0;
            HudManager.Instance.UpdateGold(gold);
        }
    }

    public int ZombieCount {
        get { return zombieCount; }
        set {
            zombieCount = value >= 0 ? value : 0;
            HudManager.Instance.UpdateZombieCount(zombieCount);
        }
    }

    private void Awake() {
        Instance = this;
        gold = initGold;

        foreach (var gameVar in gameVariables) variables[gameVar.name] = gameVar.value;
    }
    private void Start() {
        gold = initGold;
        zombieCount = 0;
        HudManager.Instance.UpdateGold(gold);
        HudManager.Instance.UpdateZombieCount(zombieCount);
    }
}
