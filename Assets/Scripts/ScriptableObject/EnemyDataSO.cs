using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDataSO", menuName = "Scriptable Objects/EnemyDataSO")]
public class EnemyDataSO : ScriptableObject {
    public int id;
    public string enemyName;
    [TextArea] public string enemyDesc;
    public int startLevel;
    public int maxLevel;
    public StatField<int, int> mhp;
    public StatField<int, int> def;
    public StatField<int, int> atk;
    public StatField<int, int> atkRate;
    public StatField<int, int> luck;
    public StatField<int, int> speed;
}
