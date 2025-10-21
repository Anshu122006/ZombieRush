using UnityEngine;

public class LifeBehaviour : IItem {
    [SerializeField] private int lifeAmount;
    protected override void OnCollect(CharacterComponents player) {
        player.statsManager.AddLives(lifeAmount);
    }
}
