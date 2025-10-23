using System;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour {
    public static GlobalVariables Instance;

    [SerializeField] private int initGold;
    public List<GameVariable> gameVariables;

    private int gold;
    private int zombieCount;
    private int zombiesKilled;

    private Dictionary<string, int> variables;
    public int Gold {
        get { return gold; }
        set {
            gold = value >= 0 ? value : 0;
            HudManager.Instance?.UpdateGold(gold);
            GlobalGameManager.Instance?.SaveCurGold(gold);
        }
    }

    public int ZombieCount {
        get { return zombieCount; }
        set {
            zombieCount = value >= 0 ? value : 0;
            HudManager.Instance?.UpdateZombieCount(zombieCount);
        }
    }

    public int ZombiesKilled {
        get { return zombiesKilled; }
        set {
            zombiesKilled = value >= 0 ? value : 0;
            HudManager.Instance?.UpdateZombiesKilled(zombiesKilled);
            GlobalGameManager.Instance.SaveCurKills(zombiesKilled);
        }
    }

    private void Awake() {
        Instance = this;
        gold = initGold;

        foreach (var gameVar in gameVariables) variables[gameVar.name] = gameVar.value;
    }
    private void Start() {
        Gold = initGold;
        ZombieCount = 0;
        ZombiesKilled = 0;
    }
}
