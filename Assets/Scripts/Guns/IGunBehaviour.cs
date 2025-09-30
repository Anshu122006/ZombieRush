using UnityEngine;

public interface IGunBehaviour {
    public void Shoot(Vector2 dir, IStatsManager attacker);
    public void AddExp(int exp);
    public void LevelUp();
    public Transform FirePoint { get; }
    public int Damage { get; }
    public int Accuracy { get; }
    public float Range { get; }
    public float Weight { get; }
    public bool CanShoot { get; }
}
