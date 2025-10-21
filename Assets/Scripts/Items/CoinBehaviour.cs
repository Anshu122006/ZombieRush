using UnityEngine;

public class CoinBehaviour : IItem {
    [SerializeField] private int goldAmount = 10;
    protected override void OnCollect(CharacterComponents player) {
        float levelFactor = 1 + (player.stats.CurLevel / 4) * 0.8f;
        int amount = (int)(goldAmount * levelFactor);
        GlobalVariables.Instance.Gold += amount;
    }
}
