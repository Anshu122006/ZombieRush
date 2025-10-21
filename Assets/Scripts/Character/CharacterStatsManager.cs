using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class CharacterStatsManager : IStatsManager {
    [SerializeField] private CharacterStatsData data;
    [SerializeField] private float blinkPeriod = 0.3f;
    [SerializeField] private float blinkDuration = 4f;
    [SerializeField] private List<Transform> spawnPoints;

    private bool inReviveState = false;
    public bool InReviveStage => inReviveState;
    public override int ExpDrop => 0;

    private void Start() {
        spriteRenderer.material = new Material(spriteRenderer.material);
        spriteRenderer.material.SetColor("_FlashColor", flashColor);
        spriteRenderer.material.SetFloat("_FlashAmount", 0);

        HudManager.Instance?.UpdateName(data.definition.charName);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.K)) {
            // TakeDamage(5, 3, out int e);
            // OnDeath();
        }
    }

    public override void TakeDamage(int atk, int accuracy, out int expDrop) {
        expDrop = 0;
        if (onDeathCoroutine != null) return;

        int dmg = Random.Range((int)(atk * 0.5f), (int)(atk * 1.2f)) - Random.Range((int)(data.DEF * 0.5f), (int)(data.DEF * 1.2f));
        dmg = Mathf.Clamp(dmg, 1, (int)(atk * 1.2f));

        int chance = Random.Range(0, accuracy + data.AGI);
        if (chance > accuracy) {
            Debug.Log("Miss");
            return;
        }

        data.HP -= dmg;
        if (!healthbar.gameObject.activeSelf) healthbar.gameObject.SetActive(true);
        healthbar.SetFill((float)data.HP / data.MHP);
        healthbar.Fade();

        if (data.HP <= 0) {
            if (data.CURLIVES > 1) {
                data.CURLIVES--;
                OnDeath();
                inReviveState = true;
            }
            else {
                Debug.Log("GameOver");
            }
        }
    }

    public override void Heal(int amount) {
        amount = Random.Range((int)(amount * 0.7f), (int)(amount * 1.4f));
        data.HP += amount;

        if (!healthbar.gameObject.activeSelf) healthbar.gameObject.SetActive(true);
        healthbar.SetFill((float)data.HP / data.MHP);
        healthbar.Fade();
    }

    public override void AddLives(int amount = 1) {
        data.CURLIVES += amount;
    }

    public override void AddExp(int exp) {
        exp = Random.Range((int)(exp * 0.5f), exp);
        if (data.EXP + exp < data.ExpThreshold) {
            data.EXP += exp;
        }
        else {
            int gain = data.EXP + exp;
            while (gain >= data.ExpThreshold) {
                gain -= data.ExpThreshold;
                LevelUp();
            }
            data.EXP = gain;
        }
    }

    public override void LevelUp() {
        if (data.CurLevel < data.MAXLV) {
            data.CurLevel++;
            data.HP = data.MHP;
        }
    }

    public override void OnDeath() {
        if (onDeathCoroutine == null) {
            if (hitAudioCoroutine != null) StopCoroutine(hitAudioCoroutine);
            if (flashCoroutine != null) StopCoroutine(flashCoroutine);
            hitAudioCoroutine = StartCoroutine(HitAudioCooldown(20));
            onDeathCoroutine = StartCoroutine(ReviveEffect());
        }
    }

    private IEnumerator ReviveEffect() {
        GameAudioManager.Instance.PlaySound(deathAudio, transform.position);
        yield return new WaitForSeconds(deathAudio.length);

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        transform.position = spawnPoint.position;
        data.HP = data.MHP;
        inReviveState = false;

        if (!healthbar.gameObject.activeSelf) healthbar.gameObject.SetActive(true);
        healthbar.SetFill((float)data.HP / data.MHP);
        healthbar.Fade();

        float currentFlashAmount = 0;
        float elapsed = 0;
        while (elapsed <= blinkDuration) {
            elapsed += Time.deltaTime;
            float t = elapsed / blinkDuration;
            float a = Mathf.PI / blinkPeriod;
            currentFlashAmount = Mathf.Abs(Mathf.Sin(a * t)) * 0.3f;
            spriteRenderer.material.SetFloat("_FlashAmount", currentFlashAmount);
            yield return null;
        }

        spriteRenderer.material.SetFloat("_FlashAmount", 0);

        if (hitAudioCoroutine != null) StopCoroutine(hitAudioCoroutine);
        hitAudioCoroutine = null;
        onDeathCoroutine = null;
    }
}
