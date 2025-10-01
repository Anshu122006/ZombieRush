using UnityEngine;

public class CharacterStatsData : MonoBehaviour {
    [Header("Definition")]
    public CharacterDefinition definition;

    [Header("Runtime State")]
    public int curLevel;
    public int exp;
    public int hp;

    // Computed stats based on current level
    public int MAXLV => definition.maxLevel;
    public int ExpThreshold => definition.expThreshold.EvaluateStat(curLevel, definition.maxLevel);
    public int MHP => definition.mhp.EvaluateStat(curLevel, definition.maxLevel);
    public int DEF => definition.def.EvaluateStat(curLevel, definition.maxLevel);
    public int AGI => definition.agi.EvaluateStat(curLevel, definition.maxLevel);
    public int LUCK => definition.luck.EvaluateStat(curLevel, definition.maxLevel);
    public int BaseSpeed => definition.baseSpeed.EvaluateStat(curLevel, definition.maxLevel);


    // Setup for outside
    public void Setup(int curLevel, int exp) {
        this.curLevel = curLevel;
        this.exp = exp;
        this.hp = MHP;
    }

    private void Start() {
        Setup(1, 0);
    }
}
