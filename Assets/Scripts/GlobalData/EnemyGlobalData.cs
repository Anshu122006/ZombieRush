using System.Collections.Generic;
using UnityEngine;

public class EnemyGlobalData : MonoBehaviour {
    public static EnemyGlobalData Instance { get; private set; }

    [SerializeField] private List<EnemyDefinition> definitions;
    public Dictionary<string, EnemyDefinition> data = new Dictionary<string, EnemyDefinition>();
    public Dictionary<string, int> exp = new Dictionary<string, int>();
    public Dictionary<string, int> curLevel = new Dictionary<string, int>();

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        foreach (EnemyDefinition def in definitions) {
            data[def.enemyName] = def;
            exp[def.enemyName] = 0;
            curLevel[def.enemyName] = def.startLevel;
        }
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
        Debug.Log(exp[enemyName]);
    }

    public void LevelUp(string enemyName) {
        if (curLevel[enemyName] < data[enemyName].maxLevel) {
            curLevel[enemyName]++;
        }
        Debug.Log(curLevel[enemyName]);
    }

    private int GetThreshold(string enemyName) {
        int cl = curLevel[enemyName];
        int ml = data[enemyName].maxLevel;
        int threshold = (int)(data[enemyName].expThreshold.init +
                (data[enemyName].expThreshold.final - data[enemyName].expThreshold.init) *
                Mathf.Pow((float)cl / ml, data[enemyName].expThreshold.pow));
        return threshold;
    }
}
