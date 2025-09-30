using UnityEngine;

public class CharacterStats : MonoBehaviour {
    [Header("Definition")]
    public CharacterDefinition definition;

    [Header("Runtime State")]
    public int curLevel;
    public int exp;
    public int hp;

    // Computed stats based on current level
    public int MAXLV => definition.maxLevel;

    public int ExpThreshold => (int)(definition.expThreshold.init +
        (definition.expThreshold.final - definition.expThreshold.init) *
        Mathf.Pow((float)curLevel / definition.maxLevel, definition.expThreshold.pow));

    public int MHP => (int)(definition.mhp.init +
        (definition.mhp.final - definition.mhp.init) *
        Mathf.Pow((float)curLevel / definition.maxLevel, definition.mhp.pow));

    public int DEF => (int)(definition.def.init +
        (definition.def.final - definition.def.init) *
        Mathf.Pow((float)curLevel / definition.maxLevel, definition.def.pow));

    public int BaseSpeed => (int)(definition.baseSpeed.init +
        (definition.baseSpeed.final - definition.baseSpeed.init) *
        Mathf.Pow((float)curLevel / definition.maxLevel, definition.baseSpeed.pow));

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
