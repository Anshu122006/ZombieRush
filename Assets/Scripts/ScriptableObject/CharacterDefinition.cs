using UnityEngine;

[CreateAssetMenu(menuName = "Definitions/Character", fileName = "CharacterDefinition")]
public class CharacterDefinition : ScriptableObject {
    [Header("Info")]
    public string charName;
    public string charClass;
    [TextArea] public string charDesc;

    [Header("Level Settings")]
    public int startLevel;
    public int maxLevel;

    [Header("Stat Growth Curves")]
    public StatField<int, int> expThreshold;
    public StatField<int, int> mhp;
    public StatField<int, int> def;
    public StatField<int, int> agi;
    public StatField<int, int> luck;
    public StatField<int, int> baseSpeed;
}
