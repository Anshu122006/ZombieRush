using UnityEngine;

public class CharacterStatsData : MonoBehaviour {
    [Header("Definition")]
    public CharacterDefinition definition;

    [Header("Runtime State")]
    private int curLevel;
    private int curLives;
    private int exp;
    private int hp;

    public string Name => definition.charName;

    // Computed stats based on current level
    public int CurLevel {
        get {
            return curLevel;
        }
        set {
            curLevel = Mathf.Clamp(value, 1, MAXLV);
            HudManager.Instance?.UpdatePlayerLevel(curLevel);
        }
    }

    public int HP {
        get {
            return hp;
        }
        set {
            hp = Mathf.Clamp(value, 0, MHP);
            HudManager.Instance?.UpdateHealth((float)hp / MHP);
        }
    }

    public int EXP {
        get {
            return exp;
        }
        set {
            exp = Mathf.Clamp(value, 0, ExpThreshold);
            HudManager.Instance?.UpdateExp((float)exp / ExpThreshold);
        }
    }

    public int CURLIVES {
        get {
            return curLives;
        }
        set {
            curLives = Mathf.Clamp(value, 0, MAXLIVES);
            HudManager.Instance?.UpdateLives(curLives);
        }
    }

    public int MAXLV => definition.maxLevel;
    public int MAXLIVES => definition.maxLives;
    public int ExpThreshold => definition.expThreshold.EvaluateStat(curLevel, definition.maxLevel);
    public int MHP => definition.mhp.EvaluateStat(curLevel, definition.maxLevel);
    public int DEF => definition.def.EvaluateStat(curLevel, definition.maxLevel);
    public int AGI => definition.agi.EvaluateStat(curLevel, definition.maxLevel);
    public int LUCK => definition.luck.EvaluateStat(curLevel, definition.maxLevel);
    public int BaseSpeed => definition.baseSpeed.EvaluateStat(curLevel, definition.maxLevel);


    // Setup for outside
    public void Setup(int curLevel, int exp = 0) {
        this.CurLevel = curLevel;
        this.EXP = exp;
        this.HP = MHP;
        this.CURLIVES = MAXLIVES;
    }

    private void Start() {
        Setup(1, 0);
    }
}
