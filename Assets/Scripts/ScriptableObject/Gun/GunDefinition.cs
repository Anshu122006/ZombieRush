using UnityEngine;

public abstract class GunDefinition : ScriptableObject {
    [Header("Base Properties")]
    public string gunName = "Unnamed Gun";
    public Sprite gunSprite;

    [Header("Progression")]
    public int startLevel = 1;
    public int maxLevel = 10;

    [Header("Core Stats")]
    public StatField<int, int> expThreshold = new(100, 2000, 1);
    public StatField<int, int> damage = new(10, 100, 1);
    public StatField<int, int> accuracy = new(60, 95, 1);
    public StatField<int, int> range = new(20, 100, 1);
    public StatField<float, int> fireDelay = new(0.6f, 0.2f, 1);
    public StatField<float, int> weight = new(5f, 3f, 1);
}












