using UnityEngine;

public interface IStatsManager {
    public void TakeDamage(int atk, int accuracy, IStatsManager attacker);
    public void TakeDamage(int atk, int accuracy);
    public void AddExp(int exp);
    public void LevelUp();
    public int ExpDrop { get; }
}
