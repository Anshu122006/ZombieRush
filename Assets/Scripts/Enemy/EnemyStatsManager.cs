using UnityEngine;

public class EnemyStatsManager : MonoBehaviour, IStatsManager {
    [SerializeField] private EnemyStatsData data;
    [SerializeField] private Healthbar healthbar;
    public int ExpDrop => data.ExpDrop;

    public void TakeDamage(int atk, int accuracy, IStatsManager attacker) {
        int dmg = Random.Range((int)(atk * 0.5f), (int)(atk * 1.2f)) - Random.Range((int)(data.DEF * 0.5f), (int)(data.DEF * 1.2f));
        dmg = Mathf.Clamp(dmg, 1, (int)(atk * 1.2f));

        int chance = Random.Range(0, accuracy + data.AGI);
        if (chance > accuracy) {
            Debug.Log("Miss");
            return;
        }

        if (data.hp - dmg > 0) {
            data.hp -= dmg;
        }
        else {
            data.hp = 0;
            attacker.AddExp(data.ExpDrop);
            AddExp(data.ExpGain);
            Destroy(gameObject);
        }

        if (!healthbar.gameObject.activeSelf) healthbar.gameObject.SetActive(true);
        healthbar.SetFill((float)data.hp / data.MHP);
        healthbar.Fade();
        Debug.Log(data.hp);
    }

    public void TakeDamage(int atk, int accuracy) {
        int dmg = Random.Range((int)(atk * 0.5f), (int)(atk * 1.2f)) - Random.Range((int)(data.DEF * 0.5f), (int)(data.DEF * 1.2f));
        dmg = Mathf.Clamp(dmg, 1, (int)(atk * 1.2f));

        int chance = Random.Range(0, accuracy + data.AGI);
        if (chance > accuracy) {
            Debug.Log("Miss");
            return;
        }

        if (data.hp - dmg > 0) {
            data.hp -= dmg;
        }
        else {
            data.hp = 0;
            AddExp(data.ExpGain);
            Destroy(gameObject);
        }

        if (!healthbar.gameObject.activeSelf) healthbar.gameObject.SetActive(true);
        healthbar.SetFill((float)data.hp / data.MHP);
        healthbar.Fade();
        Debug.Log(data.hp);
    }

    public void AddExp(int exp) {
        EnemyGlobalData.Instance.AddExp(data.definition.enemyName, exp);
    }

    public void LevelUp() {
        EnemyGlobalData.Instance.LevelUp(data.definition.enemyName);
    }
}
