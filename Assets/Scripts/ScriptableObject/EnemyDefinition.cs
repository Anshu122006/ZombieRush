using UnityEngine;

[CreateAssetMenu(menuName = "Definitions/Enemy Definition", fileName = "EnemyDefinition")]
public class EnemyDefinition : ScriptableObject {
    [Header("Info")]
    public string enemyName;
    [TextArea] public string enemyDesc;

    [Header("Level Settings")]
    public int startLevel;
    public int maxLevel;

    [Header("Stat Growth Curves")]
    public StatField<int, int> expThreshold;
    public StatField<int, int> expDrop;
    public StatField<int, int> expGain;
    public StatField<int, int> mhp;
    public StatField<int, int> def;
    public StatField<int, int> atk;
    public StatField<int, int> atkRate;
    public StatField<int, int> agi;
    public StatField<int, int> luck;
    public StatField<int, int> baseSpeed;
}
