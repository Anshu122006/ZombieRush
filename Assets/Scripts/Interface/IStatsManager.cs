using UnityEngine;

public interface IStatsManager {
    public void TakeDamage(int atk,IStatsManager attacker);
    public void AddExp(int exp);
    public void LevelUp();
    public int ExpDrop{ get; }
}
