using System.Collections;
using UnityEngine;

public class CharacterStatsManager : MonoBehaviour, IStatsManager {
    [SerializeField] private CharacterStatsData data;
    [SerializeField] private Healthbar healthbar;
    public int ExpDrop => 0;

    public void TakeDamage(int atk, int accuracy, out int expDrop) {
        int dmg = Random.Range((int)(atk * 0.5f), (int)(atk * 1.2f)) - Random.Range((int)(data.DEF * 0.5f), (int)(data.DEF * 1.2f));
        dmg = Mathf.Clamp(dmg, 1, (int)(atk * 1.2f));
        expDrop = 0;

        int chance = Random.Range(0, accuracy + data.AGI);
        if (chance > accuracy) {
            Debug.Log("Miss");
            return;
        }

        if (data.hp - dmg >= 0) data.hp -= dmg;
        else data.hp = 0;

        if (!healthbar.gameObject.activeSelf) healthbar.gameObject.SetActive(true);
        healthbar.SetFill((float)data.hp / data.MHP);
        healthbar.Fade();
        Debug.Log(data.hp);
    }

    public void AddExp(int exp) {
        exp = Random.Range((int)(exp * 0.5f), exp);
        if (data.exp + exp < data.ExpThreshold) {
            data.exp += exp;
        }
        else {
            int gain = data.exp + exp;
            while (gain >= data.ExpThreshold) {
                gain -= data.ExpThreshold;
                LevelUp();
            }
            data.exp = gain;
        }
    }

    public void LevelUp() {
        if (data.curLevel < data.MAXLV) {
            data.curLevel++;
            data.hp = data.MHP;
        }
    }
}
