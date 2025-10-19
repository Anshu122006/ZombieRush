using System;
using Game.Utils;
using Unity.VisualScripting;
using UnityEngine;

public class Missile : MonoBehaviour {
    private Rigidbody2D rb;
    private Transform target;
    private int damage;
    private int accuracy;
    private float v;
    private float a;
    private float elapsed;
    private float duration;
    private Action<int> AddExp;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        DestroySelf();
    }

    void OnTriggerEnter2D(Collider2D collider) {
        IStatsManager enemy = collider.GetComponent<IStatsManager>();
        if (enemy != null) {
            enemy.TakeDamage(damage, accuracy, out int expDrop);
            AddExp?.Invoke(expDrop);
        }
        Destroy(gameObject);
    }

    public void Setup(Transform target, float u, float v, float d, int damage, int accuracy, bool followTarget, Action<int> AddExp) {
        this.damage = damage;
        this.accuracy = accuracy;
        this.target = followTarget ? target : null;
        this.v = v;
        this.AddExp = AddExp;

        float s = d / 4;
        a = 2 * (v * v - u * u) / (2 * s);
        duration = (v - u) / a + 0.75f * d / v;
        elapsed = 0;
        Vector2 dir = (target.position - transform.position).normalized;

        transform.rotation = VectorHandler.RotationFromVector(dir);
        Vector2 force = dir * u * rb.mass;
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    private void Acclerate() {
        if (rb.linearVelocity.magnitude < v) {
            float forceMag = rb.mass * a;
            rb.AddForce(rb.linearVelocity.normalized * forceMag, ForceMode2D.Force);
        }
        if (target != null) {
            Vector2 dir = (target.position - transform.position).normalized;
            transform.rotation = VectorHandler.RotationFromVector(dir);
            rb.linearVelocity = dir * rb.linearVelocity.magnitude;
        }
    }

    private void DestroySelf() {
        if (elapsed >= duration) {
            Destroy(gameObject);
        }
        else {
            elapsed += Time.fixedDeltaTime;
            Acclerate();
        }
    }
}
