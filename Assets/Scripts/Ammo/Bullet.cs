using Game.Utils;
using UnityEngine;

public class Bullet : MonoBehaviour {
    private Rigidbody2D rb;
    private int damage;
    private int accuracy;
    private float elapsed;
    private float duration;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        DestroySelf();
    }

    void OnTriggerEnter2D(Collider2D collider) {
        IStatsManager enemy = collider.GetComponent<IStatsManager>();
        if (enemy != null) enemy.TakeDamage(damage, accuracy);
        Destroy(gameObject);
    }

    public void Setup(Vector3 dir, float u, float d, int damage, int accuracy) {
        this.damage = damage;
        this.accuracy = accuracy;

        elapsed = 0;
        duration = d / u;

        transform.rotation = VectorHandler.RotationFromVector(dir);
        Vector2 force = dir * u * rb.mass;
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    private void DestroySelf() {
        if (elapsed >= duration) Destroy(gameObject);
        else elapsed += Time.fixedDeltaTime;
    }
}
