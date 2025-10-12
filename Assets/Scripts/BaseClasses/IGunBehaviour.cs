using System.Collections.Generic;
using UnityEngine;

public interface IGunBehaviour {
    public void Shoot(Vector2 dir);
    public void AbortShoot();
    public void AddExp(int exp);
    public void LevelUp();
    public CharacterStatsData CharStatData { get; set; }
    public CharacterStatsManager CharStatManager { get; set; }
    public string Name { get; }
    public bool Shooting { get; }

    public int Damage { get; }
    public int Accuracy { get; }
    public float Range { get; }
    public float Weight { get; }
}
