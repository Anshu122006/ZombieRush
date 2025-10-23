using UnityEngine;

public class TurretStatManager : IStatsManager {
    [SerializeField] private ITurretBehaviour data;
    public override int ExpDrop => 0;

    public override void AddExp(int exp) => GlobalTurretData.Instance.AddExp(data.Name, exp);

    public override void AddLives(int amount = 1) { }

    public override void Heal(int amount) { }

    public override void LevelUp() => GlobalTurretData.Instance.IncrementMaxUpgradeLevel(data.Name);

    public override void TakeDamage(int atk, int accuracy, out int expDrop, Transform target = null) {
        int dmg = Random.Range((int)(atk * 0.5f), (int)(atk * 1.2f)) - Random.Range((int)(data.DEF * 0.5f), (int)(data.DEF * 1.2f));
        dmg = Mathf.Clamp(dmg, 1, (int)(atk * 1.2f));
        expDrop = 0;

        if (data.HP - dmg >= 0) {
            data.HP -= dmg;
        }
        else {
            data.HP = 0;
            expDrop = data.ExpDrop;
        }

        if (!healthbar.gameObject.activeSelf) healthbar.gameObject.SetActive(true);
        healthbar.SetFill((float)data.HP / data.MHP);
        healthbar.Fade();
    }
}
