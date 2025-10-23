using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatsManager : IStatsManager {
    [SerializeField] private EnemyStatsData data;
    [SerializeField] private EnemyAnimation anim;
    [SerializeField] private AnimationClip deathAnimation;
    public override int ExpDrop => data.ExpDrop;

    private void Start() {
        spriteRenderer.material = new Material(spriteRenderer.material);
        spriteRenderer.material.SetColor("_FlashColor", flashColor);
        spriteRenderer.material.SetFloat("_FlashAmount", 0);
    }
    public override void TakeDamage(int atk, int accuracy, out int expDrop, Transform target = null) {
        expDrop = 0;
        if (onDeathCoroutine != null) return;

        int dmg = Random.Range((int)(atk * 0.5f), (int)(atk * 1.2f)) - Random.Range((int)(data.DEF * 0.5f), (int)(data.DEF * 1.2f));
        dmg = Mathf.Clamp(dmg, 1, (int)(atk * 1.2f));

        int chance = Random.Range(0, accuracy + data.AGI);
        if (chance > accuracy) {
            return;
        }

        if (data.hp - dmg > 0) {
            data.hp -= dmg;
            GetComponent<EnemyAI>().SetIsHit(target);
        }
        else {
            data.hp = 0;
            expDrop = data.ExpDrop;
            AddExp(data.ExpGain);
            OnDeath();
        }

        if (!healthbar.gameObject.activeSelf) healthbar.gameObject.SetActive(true);
        healthbar.SetFill((float)data.hp / data.MHP);
        healthbar.Fade();
    }

    public override void Heal(int amount) {
        amount = Random.Range((int)(amount * 0.7f), (int)(amount * 1.4f));
        data.hp = Mathf.Clamp(data.hp + amount, 0, data.MHP);

        if (!healthbar.gameObject.activeSelf) healthbar.gameObject.SetActive(true);
        healthbar.SetFill((float)data.hp / data.MHP);
        healthbar.Fade();
    }

    public override void AddExp(int exp) {
        EnemyGlobalData.Instance.AddExp(data.definition.enemyName, exp);
    }

    public override void LevelUp() {
        EnemyGlobalData.Instance.LevelUp(data.definition.enemyName);
    }

    public override void OnDeath() {
        if (onDeathCoroutine == null) {
            if (hitAudioCoroutine != null) StopCoroutine(hitAudioCoroutine);
            if (flashCoroutine != null) StopCoroutine(flashCoroutine);
            hitAudioCoroutine = StartCoroutine(HitAudioCooldown(20));
            onDeathCoroutine = StartCoroutine(OnDeathEffect());
        }
    }

    private IEnumerator OnDeathEffect() {
        spriteRenderer.material.SetFloat("_FlashAmount", 0);

        GameAudioManager.Instance.PlaySound(deathAudio, transform.position);
        anim.PlayDeathAnimation();
        yield return new WaitForSeconds(deathAnimation.length - 0.01f);

        if (hitAudioCoroutine != null) StopCoroutine(hitAudioCoroutine);
        hitAudioCoroutine = null;
        onDeathCoroutine = null;

        data.DestroySelf();
    }
}
