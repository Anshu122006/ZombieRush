using System.Collections.Generic;
using UnityEngine;

public class GlobalTurretData : MonoBehaviour {
    [SerializeField] private List<TurretDefinition> definitions;
    public static GlobalTurretData Instance { get; private set; }
    private Dictionary<string, TurretDefinition> data = new();
    public Dictionary<string, int> exp = new();
    public Dictionary<string, int> curLevel = new();
    public Dictionary<string, bool> isUnlocked = new();

    private void Awake() {
        Instance = this;

        foreach (var def in definitions) {
            data[def.turretName] = def;
            exp[def.turretName] = 0;
            curLevel[def.turretName] = 1;
            isUnlocked[def.turretName] = true;
        }
        isUnlocked[definitions[0].turretName] = true;
    }

    public void AddExp(string turretName, int e) {
        e = Random.Range((int)(e * 0.1f), (int)(e * 0.3f));
        int gain = exp[turretName] + e;
        int threshold = GetThreshold(turretName);
        while (gain < threshold) {
            gain -= threshold;
            IncrementMaxUpgradeLevel(turretName);
            threshold = GetThreshold(turretName);
        }
        exp[turretName] = gain;
    }

    public void IncrementMaxUpgradeLevel(string turretName) {
        if (curLevel[turretName] < data[turretName].maxLevel) {
            curLevel[turretName]++;
        }
        Debug.Log(curLevel[turretName]);
    }

    public void UnlockTurret(string turretName) {
        isUnlocked[turretName] = true;
    }

    private int GetThreshold(string turretName) {
        return data[turretName].expThreshold.EvaluateStat(curLevel[turretName], data[turretName].maxLevel);
    }
}
