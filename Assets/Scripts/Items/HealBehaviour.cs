using UnityEngine;

public class HealBehaviour : IItem {
    [SerializeField] private int healAmount = 10;
    protected override void OnCollect(CharacterComponents player) {
        float levelFactor = 1 + (player.stats.CurLevel / 4) * 0.8f;
        int amount = (int)(healAmount * levelFactor);
        player.statsManager.Heal(amount);
    }
}
