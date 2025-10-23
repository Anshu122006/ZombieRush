using System.Collections.Generic;
using UnityEngine;

public class EnemyGlobalData : MonoBehaviour {
    public static EnemyGlobalData Instance { get; private set; }

    [SerializeField] private List<EnemyDefinition> definitions;
    public Dictionary<string, EnemyDefinition> data = new Dictionary<string, EnemyDefinition>();
    public Dictionary<string, int> exp = new Dictionary<string, int>();
    public Dictionary<string, int> curLevel = new Dictionary<string, int>();

    private List<string> unlockedZombies = new();
    public List<string> UnlockedZombies => new List<string>(unlockedZombies);

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        foreach (EnemyDefinition def in definitions) {
            data[def.enemyName] = def;
            exp[def.enemyName] = 0;
            curLevel[def.enemyName] = def.startLevel;
        }
        unlockedZombies.Add(definitions[0].enemyName);
    }

    public void AddExp(string enemyName, int amount) {
        int threshold = GetThreshold(enemyName);
        if (exp[enemyName] + amount < threshold) {
            exp[enemyName] += amount;
        }
        else {
            int gain = exp[enemyName] + amount - threshold;
            while (gain >= threshold) {
                gain -= GetThreshold(enemyName);
                LevelUp(enemyName);
            }
            exp[enemyName] = gain;
        }
    }

    public void LevelUp(string enemyName) {
        if (curLevel[enemyName] < data[enemyName].maxLevel) {
            curLevel[enemyName]++;
            HudManager.Instance?.ShowLog(enemyName + " reached Lv" + curLevel[enemyName]);
        }
    }

    private int GetThreshold(string enemyName) {
        int cl = curLevel[enemyName];
        int ml = data[enemyName].maxLevel;
        int threshold = (int)(data[enemyName].expThreshold.init +
                (data[enemyName].expThreshold.final - data[enemyName].expThreshold.init) *
                Mathf.Pow((float)cl / ml, data[enemyName].expThreshold.pow));
        return threshold;
    }

    public void AddNewZombie(int curLevel) {
        switch (curLevel) {
            case 3:
                string zombieName = definitions[1].enemyName;
                unlockedZombies.Add(zombieName);
                HudManager.Instance?.ShowLog(zombieName + " has become active");
                break;
            case 7:
                zombieName = definitions[2].enemyName;
                unlockedZombies.Add(zombieName);
                HudManager.Instance?.ShowLog(zombieName + " has become active");
                break;
        }
    }
}
