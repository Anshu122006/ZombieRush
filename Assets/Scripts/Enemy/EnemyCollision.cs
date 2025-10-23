using System.Collections;
using UnityEngine;

public class EnemyCollision : MonoBehaviour {
    [SerializeField] private float maxMass = 2;
    [SerializeField] private float collisionCheckRate = 0.1f;
    [SerializeField] private LayerMask targetLayers;
    private Coroutine collisionDelayCoroutine;

    private void OnCollisionStay2D(Collision2D collision) {
        if (collisionDelayCoroutine == null) {
            Collider2D collider = collision.collider;
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if ((targetLayers & (1 << collider.gameObject.layer)) != 0) {
                float mass = Mathf.Clamp(rb.mass, 1, 100);
                float dampingFactor = Mathf.Clamp(maxMass / mass, 0.1f, 0.4f);
                rb.linearVelocity *= 0.3f;
                collisionDelayCoroutine = StartCoroutine(DelayCollisionCheck());
            }
        }
    }

    private IEnumerator DelayCollisionCheck() {
        yield return new WaitForSeconds(collisionCheckRate);
        collisionDelayCoroutine = null;
    }
}
