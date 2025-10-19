using System;
using System.Collections;
using UnityEditor.Analytics;
using UnityEngine;

public class Emitter : MonoBehaviour {
    [SerializeField] private ParticleSystem flameParticles;
    [SerializeField] private ParticleSystem smokeParticles;
    [SerializeField] private CapsuleCollider2D collider2d;
    private bool emitting;
    private int damage;
    private int accuracy;
    private float fireRate;
    private Coroutine delayCoroutine;
    private Action<int> AddExp;


    private void OnTriggerStay2D(Collider2D collider) {
        if (emitting && delayCoroutine == null) {
            IStatsManager target = collider.GetComponent<IStatsManager>();
            if (target != null && (LayerMask.GetMask("Enemy") & (1 << collider.gameObject.layer)) != 0) {
                target.TakeDamage(damage, accuracy, out int expDrop);
                AddExp?.Invoke(expDrop);
                delayCoroutine = StartCoroutine(Delay());
            }
        }
    }

    private IEnumerator Delay() {
        yield return new WaitForSeconds(fireRate);
        delayCoroutine = null;
    }

    public void UpdateStats(int damage, int accuracy, float fireRate, float range, Action<int> AddExp) {
        this.damage = damage;
        this.accuracy = accuracy;
        this.fireRate = fireRate;
        this.AddExp = AddExp;

        float startLifetime = range * 0.2f;
        var main = flameParticles.main;
        var emission = flameParticles.emission;
        main.startLifetime = new ParticleSystem.MinMaxCurve(startLifetime, startLifetime * 2.5f);
        emission.rateOverTime = startLifetime * 800;
        main = smokeParticles.main;
        emission = smokeParticles.emission;
        main.startLifetime = startLifetime * 2.5f + 0.05f;
        emission.rateOverTime = startLifetime * 600;

        Vector2 size = collider2d.size;
        Vector2 offset = collider2d.offset;
        size.x = range;
        offset.x = range * 0.5f;
    }

    public void StartEmitting(Vector2 start, Vector2 dir) {
        if (!emitting) {
            transform.position = start;
            transform.right = dir;
            flameParticles.Play();
            smokeParticles.Play();

            emitting = true;
        }
    }

    public void StopEmitting() {
        if (emitting) {
            flameParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            smokeParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);

            emitting = false;
        }
    }
}
