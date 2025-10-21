using UnityEngine;

[CreateAssetMenu(menuName = "Definitions/Enemy", fileName = "EnemyDefinition")]
public class EnemyDefinition : ScriptableObject {
    [Header("Info")]
    public string enemyName = "Zombie";
    [TextArea] public string enemyDesc = "Some zombie description";

    [Header("Level Settings")]
    public int startLevel = 1;
    public int maxLevel = 99;
    public int maxCountPerSpawnPoint = 6;

    [Header("Stat Growth Curves")]
    public StatField<int, int> expThreshold = new StatField<int, int>(10, 30, 1);
    public StatField<int, int> expDrop = new StatField<int, int>(5, 15, 1);
    public StatField<int, int> expGain = new StatField<int, int>(8, 20, 1);
    public StatField<int, int> mhp = new StatField<int, int>(50, 150, 5);
    public StatField<int, int> def = new StatField<int, int>(2, 10, 1);
    public StatField<int, int> atk = new StatField<int, int>(3, 12, 1);
    public StatField<int, int> atkRate = new StatField<int, int>(1, 5, 1);
    public StatField<int, int> agi = new StatField<int, int>(4, 14, 1);
    public StatField<int, int> luck = new StatField<int, int>(1, 8, 1);
    public StatField<int, int> baseSpeed = new StatField<int, int>(6, 10, 1);

}
