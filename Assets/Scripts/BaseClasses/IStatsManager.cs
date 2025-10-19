using UnityEngine;

public interface IStatsManager {
    public void TakeDamage(int atk, int accuracy, out int expDrop);
    public void AddExp(int exp);
    public void LevelUp();
    public int ExpDrop { get; }
}
