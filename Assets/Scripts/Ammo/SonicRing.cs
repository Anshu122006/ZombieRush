using UnityEngine;

public class SonicRing : MonoBehaviour {
    private int damage;
    private int accuracy;
    private float pushBackForce;
    private void OnTriggerEnter2D(Collider2D collider) {
        IStatsManager enemy = collider.GetComponent<IStatsManager>();
        Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
        Debug.Log("Hit");
        Vector2 dir = collider.transform.position - transform.position;
        if (enemy == null || rb == null) {
            Debug.LogError("No stats or rigidbody component attached");
            return;
        }

        // enemy.TakeDamage(damage);
        rb.AddForce(dir * pushBackForce, ForceMode2D.Impulse);
    }

    public void Setup(int damage, int accuracy, float pushBackForce) {
        this.damage = damage;
        this.accuracy = accuracy;
        this.pushBackForce = pushBackForce;
    }
}
