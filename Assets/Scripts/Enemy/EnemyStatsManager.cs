using UnityEngine;

public class EnemyStatsManager : MonoBehaviour, IStatsManager
{
    [SerializeField] private EnemyStatsData data;
    public int ExpDrop => data.ExpDrop;

    public void AddExp(int exp)
    {
        EnemyGlobalData.Instance.AddExp(data.definition.enemyName, exp);
    }

    public void LevelUp()
    {
        EnemyGlobalData.Instance.LevelUp(data.definition.enemyName);
    }

    public void TakeDamage(int atk, IStatsManager attacker)
    {
        int dmg = Random.Range((int)(atk * 0.5f), (int)(atk * 1.2f)) - Random.Range((int)(data.DEF * 0.5f), (int)(data.DEF * 1.2f));
        dmg = Mathf.Clamp(dmg, 1, (int)(atk * 1.2f));
        if (data.hp - dmg > 0)
        {
            data.hp -= dmg;
        }
        else
        {
            data.hp = 0;
            attacker.AddExp(data.ExpDrop);
            AddExp(data.ExpGain);
            Destroy(gameObject);
        }
        Debug.Log(data.hp);
    }
}
