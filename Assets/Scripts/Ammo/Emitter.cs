using System;
using System.Collections;
using UnityEditor.Analytics;
using UnityEngine;

public class Emitter : MonoBehaviour {
    [SerializeField] private ParticleSystem flameParticles;
    [SerializeField] private ParticleSystem smokeParticles;
    [SerializeField] private AudioSource startAudioSource;
    [SerializeField] private AudioSource loopAudioSource;
    [SerializeField] private CapsuleCollider2D collider2d;
    [SerializeField] private AudioClip flameStart;
    [SerializeField] private AudioClip flameLoop;
    private bool emitting;
    private int damage;
    private int accuracy;
    private float fireRate;
    private Coroutine delayCoroutine;
    private Action<int> AddExp;

    private void OnTriggerStay2D(Collider2D collider) {
        if (emitting && delayCoroutine == null) {
            IStatsManager enemy = collider.GetComponent<IStatsManager>();
            if (enemy != null && (LayerMask.GetMask("Enemy") & (1 << collider.gameObject.layer)) != 0) {
                enemy.HandleHitEffects();
                enemy.TakeDamage(damage, accuracy, out int expDrop);
                AddExp?.Invoke(expDrop);
                delayCoroutine = StartCoroutine(Delay());
            }
        }
    }

    private IEnumerator Delay() {
        yield return new WaitForSeconds(fireRate);
        delayCoroutine = null;
    }

    // public void UpdateStats(int damage, int accuracy, float fireRate, float range, Action<int> AddExp) {
    //     this.damage = damage;
    //     this.accuracy = accuracy;
    //     this.fireRate = fireRate;
    //     this.AddExp = AddExp;

    // float startLifetime = range * 0.2f;
    // var main = flameParticles.main;
    // var emission = flameParticles.emission;
    // main.startLifetime = new ParticleSystem.MinMaxCurve(startLifetime, startLifetime * 1.5f);
    // emission.rateOverTime = startLifetime * 10;
    // main = smokeParticles.main;
    // emission = smokeParticles.emission;
    // main.startLifetime = startLifetime * 1.5f + 0.05f;
    // emission.rateOverTime = startLifetime * 6;

    //     Vector2 size = collider2d.size;
    //     Vector2 offset = collider2d.offset;
    //     size.x = range;
    //     offset.x = range * 0.5f;
    // }

    public void UpdateStats(int damage, int accuracy, float fireRate, float range, Action<int> AddExp) {
        this.damage = damage;
        this.accuracy = accuracy;
        this.fireRate = fireRate;
        this.AddExp = AddExp;

        float startLifetime = range * 0.06f;

        var main = flameParticles.main;
        var emission = flameParticles.emission;
        main.startLifetime = new ParticleSystem.MinMaxCurve(startLifetime, startLifetime);
        emission.rateOverTime = startLifetime * 400;

        main = smokeParticles.main;
        emission = smokeParticles.emission;
        main.startLifetime = startLifetime + 0.05f;
        emission.rateOverTime = startLifetime * 300;

        Vector2 size = collider2d.size;
        Vector2 offset = collider2d.offset;
        size.x = range;
        offset.x = range * 0.5f;
        collider2d.size = size;
        collider2d.offset = offset;
    }


    public void UpdateDirecttion(Vector2 start, Vector2 dir) {
        if ((Vector2)transform.position != start)
            transform.position = start;
        if ((Vector2)transform.right != dir)
            transform.right = dir;
    }

    public void StartEmitting(Vector2 start, Vector2 dir) {
        if (!emitting) {
            flameParticles.Play();
            smokeParticles.Play();
            startAudioSource.PlayOneShot(flameStart);
            loopAudioSource.clip = flameLoop;
            loopAudioSource.loop = true;
            loopAudioSource.Play();

            emitting = true;
        }
    }

    public void StopEmitting() {
        if (emitting) {
            flameParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            smokeParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            loopAudioSource.Stop();

            emitting = false;
        }
    }
}
