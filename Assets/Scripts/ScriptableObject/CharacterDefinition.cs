using UnityEngine;

[CreateAssetMenu(menuName = "Definitions/Character", fileName = "CharacterDefinition")]
public class CharacterDefinition : ScriptableObject {
    [Header("Info")]
    public string charName = "Arin";
    public string charClass = "Warrior";
    [TextArea] public string charDesc = "A brave adventurer with balanced stats and strong combat abilities.";
    public Sprite bust;
    public Sprite side;

    [Header("Level Settings")]
    public int startLevel = 1;
    public int maxLevel = 20;
    public int maxLives = 3;

    [Header("Stat Growth Curves")]
    public StatField<int, int> expThreshold = new(50, 1000, 1);
    public StatField<int, int> mhp = new(100, 500, 1);
    public StatField<int, int> def = new(5, 50, 1);
    public StatField<int, int> agi = new(10, 50, 1);
    public StatField<int, int> luck = new(5, 30, 1);
    public StatField<int, int> baseSpeed = new(10, 25, 1);
}
