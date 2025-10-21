using System;
using System.Collections;
using UnityEngine;

public class GrenadeExplosion : MonoBehaviour {
    [SerializeField] private AnimationClip clip;
    // [SerializeField] private Transform visual;
    // [SerializeField] private CircleCollider2D explosionCollider;
    // private int damage;
    // private int accuracy;
    // private Action<int> AddExp;

    // private void OnTriggerEnter2D(Collider2D collision) {
    //     EnemyStatsManager enemy = GetComponentInParent<EnemyStatsManager>();
    //     Debug.Log(enemy);
    //     if (enemy != null) {
    //         enemy.TakeDamage(damage, accuracy, out int expDrop);
    //         AddExp.Invoke(expDrop);
    //     }
    // }

    public void Initialize() {
        // this.damage = damage;
        // this.accuracy = accuracy;
        // this.AddExp = AddExp;

        // visual.localScale = Vector2.one * (explosionRadius * 0.7f);
        // StartCoroutine(ExpandCollider(explosionRadius));
        Destroy(gameObject, clip.length - 0.01f);
    }

    // private IEnumerator ExpandCollider(float explosionRadius) {
    //     float elapsed = 0;
    //     float delay = clip.length - 0.01f;
    //     while (elapsed <= delay) {
    //         float t = elapsed / delay;
    //         explosionCollider.radius = 0.2f + explosionRadius * t;
    //         Debug.Log(explosionCollider.radius);
    //         elapsed += Time.deltaTime;
    //         yield return null;
    //     }
    //     explosionCollider.radius = explosionRadius;
    // }
}
