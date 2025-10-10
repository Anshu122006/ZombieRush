using System.Collections.Generic;
using UnityEngine;

public class TargetDetector : Detector {
    [SerializeField] private float detectionRadius = 6;
    [SerializeField] private LayerMask losLayer, playerLayer;


    // for gizmos purpose
    [SerializeField] private bool showGizmos = false;
    private List<Transform> colliders;

    public override void Detect(AIData aiData) {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);

        if (playerCollider != null) {
            Vector2 dir = (playerCollider.transform.position - transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, detectionRadius, losLayer);
            Debug.Log(hit.collider);

            if (hit.collider != null && (playerLayer & (1 << hit.collider.gameObject.layer)) != 0) {
                Debug.Log(dir);
                colliders = new List<Transform> { hit.collider.transform };
            }
            else {
                colliders = null;
            }
        }
        else {
            colliders = null;
        }
        aiData.targets = colliders;
    }

    private void OnDrawGizmosSelected() {
        if (showGizmos == false) return;

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        if (colliders != null) {
            Gizmos.color = Color.magenta;
            foreach (Transform target in colliders) {
                Gizmos.DrawSphere(target.position, 0.2f);
            }
        }
    }
}
