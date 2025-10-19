using System;
using UnityEngine;

public class SonicRing : MonoBehaviour {
    private int damage;
    private int accuracy;
    private float pushBackForce;
    private Action<int> AddExp;

    private void OnTriggerEnter2D(Collider2D collider) {
        Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
        IStatsManager enemy = collider.GetComponent<IStatsManager>();
        if (enemy != null) {
            enemy.TakeDamage(damage, accuracy, out int expDrop);
            AddExp?.Invoke(expDrop);
            Vector2 dir = collider.transform.position - transform.position;
            rb.AddForce(dir * pushBackForce, ForceMode2D.Impulse);
        }
    }

    public void Setup(int damage, int accuracy, float pushBackForce, Action<int> AddExp) {
        this.damage = damage;
        this.accuracy = accuracy;
        this.pushBackForce = pushBackForce;
        this.AddExp = AddExp;
    }
}
