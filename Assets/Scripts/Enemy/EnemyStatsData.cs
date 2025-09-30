using UnityEngine;

public class EnemyStatsData : MonoBehaviour {
    [Header("Definition")]
    public EnemyDefinition definition;

    [Header("Runtime State")]
    public int curLevel;
    public int hp;

    // Computed stats based on current level
    public int ExpDrop => (int)(definition.expDrop.init +
       (definition.expDrop.final - definition.expDrop.init) *
       Mathf.Pow((float)curLevel / definition.maxLevel, definition.expDrop.pow));

    public int ExpGain => (int)(definition.expGain.init +
    (definition.expGain.final - definition.expGain.init) *
    Mathf.Pow((float)curLevel / definition.maxLevel, definition.expGain.pow));

    public int ExpThreshold => (int)(definition.expThreshold.init +
    (definition.expThreshold.final - definition.expThreshold.init) *
    Mathf.Pow((float)curLevel / definition.maxLevel, definition.expThreshold.pow));
    public int MHP => (int)(definition.mhp.init +
        (definition.mhp.final - definition.mhp.init) *
        Mathf.Pow((float)curLevel / definition.maxLevel, definition.mhp.pow));

    public int DEF => (int)(definition.def.init +
        (definition.def.final - definition.def.init) *
        Mathf.Pow((float)curLevel / definition.maxLevel, definition.def.pow));

    public int ATK => (int)(definition.atk.init +
        (definition.atk.final - definition.atk.init) *
        Mathf.Pow((float)curLevel / definition.maxLevel, definition.atk.pow));

    public int ATKRATE => (int)(definition.atkRate.init +
        (definition.atkRate.final - definition.atkRate.init) *
        Mathf.Pow((float)curLevel / definition.maxLevel, definition.atkRate.pow));

    public int LUCK => (int)(definition.luck.init +
        (definition.luck.final - definition.luck.init) *
        Mathf.Pow((float)curLevel / definition.maxLevel, definition.luck.pow));

    public int BASESPEED => (int)(definition.baseSpeed.init +
        (definition.baseSpeed.final - definition.baseSpeed.init) *
        Mathf.Pow((float)curLevel / definition.maxLevel, definition.baseSpeed.pow));

    // Setup from outside
    public void Setup(int curLevel, int exp) {
        this.curLevel = curLevel;
        this.hp = MHP;
    }

    private void Start() {
        Setup(1, 0);
    }
}
