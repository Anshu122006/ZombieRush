using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatsData : MonoBehaviour {
    [Header("Definition")]
    public EnemyDefinition definition;

    [Header("Runtime State")]
    public int curLevel;
    public int hp;

    public string Name => definition.enemyName;
    public int MaxCountPerSpawnPoint => definition.maxCountPerSpawnPoint;
    public List<Action> onDestroy = new();

    // Computed stats based on current level
    public int ExpDrop => definition.expDrop.EvaluateStat(curLevel, definition.maxLevel);
    public int ExpGain => definition.expGain.EvaluateStat(curLevel, definition.maxLevel);
    public int ExpThreshold => definition.expThreshold.EvaluateStat(curLevel, definition.maxLevel);
    public int MHP => definition.mhp.EvaluateStat(curLevel, definition.maxLevel);
    public int DEF => definition.def.EvaluateStat(curLevel, definition.maxLevel);
    public int ATK => definition.atk.EvaluateStat(curLevel, definition.maxLevel);
    public int ATKRATE => definition.atkRate.EvaluateStat(curLevel, definition.maxLevel);
    public int AGI => definition.agi.EvaluateStat(curLevel, definition.maxLevel);
    public int LUCK => definition.luck.EvaluateStat(curLevel, definition.maxLevel);
    public int BASESPEED => definition.baseSpeed.EvaluateStat(curLevel, definition.maxLevel);


    // Setup from outside
    public void Setup(int curLevel, List<Action> onDestroy = null) {
        this.curLevel = curLevel;
        this.hp = MHP;
        if (onDestroy != null) this.onDestroy.AddRange(onDestroy);
    }


    private void Start() {
        Setup(1);
    }

    public void DestroySelf(float delay = 0) {
        foreach (var d in onDestroy) d();
        Destroy(gameObject, delay);
    }
}
