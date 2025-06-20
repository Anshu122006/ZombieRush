using Game.Utils;
using UnityEngine;

public class Bullet : MonoBehaviour {
    private Rigidbody2D rb;
    private Vector2 dir;
    private int damage;
    private int accuracy;
    private float u;
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
            enemy.TakeDamage(damage, accuracy);
            Destroy(gameObject);
        }
        else if (collision.collider.CompareTag("BlockBullet")) {
            Destroy(gameObject);
        }
    }

    public void Setup(Vector3 dir, float u, float d, int damage, int accuracy) {
        this.dir = dir;
        this.u = u;
        this.damage = damage;
        this.accuracy = accuracy;

        elapsed = 0;
        duration = d / u;

        transform.rotation = VectorHandler.RotationFromVector(dir);
        rb.AddForce(dir * u, ForceMode2D.Impulse);
    }

    private void DestroySelf() {
        if (elapsed >= duration) Destroy(gameObject);
        else elapsed += Time.fixedDeltaTime;
    }
}
