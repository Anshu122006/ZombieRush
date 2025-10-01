using UnityEngine;

public class EnemyStatsData : MonoBehaviour {
    [Header("Definition")]
    public EnemyDefinition definition;

    [Header("Runtime State")]
    public int curLevel;
    public int hp;

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
    public void Setup(int curLevel, int exp) {
        this.curLevel = curLevel;
        this.hp = MHP;
    }

    private void Start() {
        Setup(1, 0);
    }
}
