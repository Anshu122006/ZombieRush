using Game.Utils;
using UnityEngine;

public class Laser : MonoBehaviour {
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
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
        if (collider.CompareTag("Enemy")) {
            IStats enemy = collider.GetComponent<IStats>();
            enemy.TakeDamage(damage, accuracy);
        }
    }

    public void Setup(Vector3 dir, int damage, int accuracy) {
        this.damage = damage;
        this.accuracy = accuracy;
        elapsed = 0;
        duration = 3;

        transform.rotation = VectorHandler.RotationFromVector(dir);
        Vector2 force = dir * 20 * rb.mass;
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    private void DestroySelf() {
        if (elapsed >= duration) {
            Destroy(gameObject);
        }
        else {
            elapsed += Time.fixedDeltaTime;
        }
    }
}
