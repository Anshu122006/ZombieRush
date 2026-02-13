using System;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour {
    public enum ControlType { mouse, keyboard }
    public static GlobalVariables Instance;

    [SerializeField] private int initGold;
    public List<GameVariable> gameVariables;

    private int gold;
    private int zombieCount;
    private int zombiesKilled;

    [SerializeField] private ControlType controlType = ControlType.mouse;
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
        bool mouseAim = PlayerPrefs.GetInt(PrefKeys.mouseAim) == 1;
        if (mouseAim) controlType = ControlType.mouse;
        else controlType = ControlType.keyboard;
    }

    public void SwitchControlType() {
        if (controlType == ControlType.mouse) controlType = ControlType.keyboard;
        else controlType = ControlType.mouse;
    }

    public bool isMouseBased() {
        return controlType == ControlType.mouse;
    }
}
