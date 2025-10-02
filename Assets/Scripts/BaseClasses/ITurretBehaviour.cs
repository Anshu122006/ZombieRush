using UnityEngine;

public interface ITurretBehaviour {
    public string Name { get; }
    public int CurLevel { get; }
    public int UpgradeCost { get; }
    public int Damage { get; }
    public int Accuracy { get; }
    public float Range { get; }

    public void LevelUp() { }
}
