using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public abstract class IStatsManager : MonoBehaviour {
    [SerializeField] protected Healthbar healthbar;
    [SerializeField] protected Transform bloodEffectPoint;
    [SerializeField] protected List<Transform> bloodEffects;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected float flashTime = 0.6f;
    [ColorUsage(true, true)]
    [SerializeField] protected Color flashColor = Color.white;
    [SerializeField] protected AudioClip hitAudio;
    [SerializeField] protected AudioClip bloodAudio;
    [SerializeField] protected AudioClip deathAudio;
    [SerializeField] protected Transform bloodMarkPref;

    protected Coroutine flashCoroutine;
    protected Coroutine onDeathCoroutine;
    protected Coroutine hitAudioCoroutine;

    // Abstract methods
    public abstract void TakeDamage(int atk, int accuracy, out int expDrop);
    public abstract void Heal(int amount);
    public abstract void AddExp(int exp);
    public abstract void LevelUp();

    // Property for experience drop
    public abstract int ExpDrop { get; }

    public virtual void AddLives(int amount = 1) { }
    public virtual void OnDeath() { }

    // Functions for effects
    public void HandleHitEffects() {
        if (onDeathCoroutine != null) return;
        if (hitAudioCoroutine == null) {
            GameAudioManager.Instance.PlaySound(hitAudio, transform.position);
            GameAudioManager.Instance.PlaySound(bloodAudio, transform.position);
            hitAudioCoroutine = StartCoroutine(HitAudioCooldown(Mathf.Max(hitAudio.length, bloodAudio.length)));
        }

        Instantiate(bloodEffects[Random.Range(0, bloodEffects.Count)], bloodEffectPoint.position, Quaternion.identity);

        Transform bloodMark = Instantiate(bloodMarkPref);
        bloodMark.GetComponent<BloodMark>().Setup(transform.position);

        if (flashCoroutine != null) StopCoroutine(flashCoroutine);
        flashCoroutine = StartCoroutine(Flash());
    }

    public IEnumerator Flash() {
        float currentFlashAmount = 0;
        float elapsed = 0;
        while (elapsed <= flashTime) {
            elapsed += Time.deltaTime;
            currentFlashAmount = Mathf.Lerp(0.4f, 0f, elapsed / flashTime);
            spriteRenderer.material.SetFloat("_FlashAmount", currentFlashAmount);
            yield return null;
        }

        spriteRenderer.material.SetFloat("_FlashAmount", 0);
        flashCoroutine = null;
    }

    protected IEnumerator HitAudioCooldown(float time) {
        yield return new WaitForSeconds(time);
        hitAudioCoroutine = null;
    }
}
