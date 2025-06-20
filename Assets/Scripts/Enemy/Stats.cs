using UnityEngine;

public class Stats : MonoBehaviour {
    [SerializeField] public int hp;
    [SerializeField] private int mhp;
    [SerializeField] private float atk;
    [SerializeField] private float def;
    [SerializeField] private float agi;
    [SerializeField] private float accuracy;
    [SerializeField] private Healthbar healthbar;

    /// <summary>
    /// This is to be called whenever you want the target to take damage.
    /// </summary>
    /// <param name="atk">atk of the attacker.</param>
    /// <param name="accuracy">accuracy of the attacker.</param>
    public void TakeDamage(float atk, float accuracy) {
        float damage = atk - def;
        damage = damage <= 0 ? 1 : damage;

        float chanceRange = agi - accuracy;
        chanceRange = chanceRange < 1 ? 1 : chanceRange;
        chanceRange = Mathf.Log(chanceRange, 2);
        float chance = Random.Range(0, chanceRange + 8);

        if (chance > chanceRange) {
            hp -= (int)damage;
            hp = hp < 0 ? 0 : hp;
        }

        float val = hp * 1.0f / mhp;
        healthbar.SetFill(val);
    }

    public void HP() {
        Debug.Log(hp + "/" + mhp);
    }
}
