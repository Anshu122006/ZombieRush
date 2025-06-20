using Game.Utils;
using UnityEngine;

public class Missile : MonoBehaviour {
    private Rigidbody2D rb;
    private Transform target;
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

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.CompareTag("Enemy")) {
            Stats enemy = collision.collider.GetComponent<Stats>();
            enemy.TakeDamage(10, 3);
            Destroy(gameObject);
        }
        else if (collision.collider.CompareTag("BlockBullet")) {
            Destroy(gameObject);
        }
    }

    public void Setup(Transform target, float u, float v, float d) {
        this.target = target;
        this.u = u == 0 ? 0.2f : u;
        this.v = v;
        a = 3 * (v * v - u * u) / d;
        elapsed = 0;
        duration = (v - u) / a + 0.75f * d / v;
        Vector2 dir = (target.position - transform.position).normalized;

        transform.rotation = VectorHandler.RotationFromVector(dir);
        rb.AddForce(dir * u, ForceMode2D.Impulse);
    }

    private void Acclerate() {
        if (rb.linearVelocity.magnitude < v) {
            float forceMag = rb.mass * a;
            rb.AddForce(rb.linearVelocity.normalized * forceMag, ForceMode2D.Force);
        }
        else {
            Vector2 dir = (target.position - transform.position).normalized;
            transform.rotation = VectorHandler.RotationFromVector(dir);
            rb.linearVelocity = dir * v;
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
