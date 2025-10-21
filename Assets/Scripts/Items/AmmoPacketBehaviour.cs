using UnityEngine;

public class AmmoPacketBehaviour : IItem {
    [SerializeField] private string gunName;
    [SerializeField] private int ammoAmount;
    protected override void OnCollect(CharacterComponents player) {
        float levelFactor = 1 + (player.stats.CurLevel / 4) * 0.8f;
        int amount = (int)(ammoAmount * levelFactor);
        player.gunHandler.Refill(gunName, amount);
    }
}
