using Game.Utils;
using UnityEngine;

public class Missile : MonoBehaviour {
    private Rigidbody2D rb;
    private Transform target;
    private bool followTarget;
    private float u;
    private float v;
    private float a;
    private float elapsed;
    private float duration;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        DestroySelf();
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if ((LayerMask.GetMask("Enemy") & (1 << collider.gameObject.layer)) != 0) {
            IStatsManager enemy = collider.GetComponent<IStatsManager>();
            // enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if ((LayerMask.GetMask("BlockBullet") & (1 << collider.gameObject.layer)) != 0) {
            Destroy(gameObject);
        }
    }

    public void Setup(Transform target, float u, float v, float d, int damage, int accuracy, bool followTarget) {
        this.followTarget = followTarget;
        this.target = target;
        this.u = u == 0 ? 0.2f : u;
        this.v = v;
        a = 2 * (v * v - u * u) / d;
        elapsed = 0;
        duration = (v - u) / a + 0.75f * d / v;
        Vector2 dir = (target.position - transform.position).normalized;
        if (!followTarget) {
            Destroy(target.gameObject);
            target = null;
        }

        transform.rotation = VectorHandler.RotationFromVector(dir);
        Vector2 force = dir * u * rb.mass;
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    private void Acclerate() {
        if (rb.linearVelocity.magnitude < v) {
            float forceMag = rb.mass * a;
            rb.AddForce(rb.linearVelocity.normalized * forceMag, ForceMode2D.Force);
        }
        if (followTarget) {
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
