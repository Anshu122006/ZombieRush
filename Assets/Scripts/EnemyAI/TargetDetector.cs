using System.Collections.Generic;
using UnityEngine;

public class TargetDetector : Detector {
    [SerializeField] private float detectionRadius = 6;
    [SerializeField] private LayerMask losLayer, playerLayer;


    // for gizmos purpose
    [SerializeField] private bool showGizmos = false;
    private List<Transform> colliders;

    public override void Detect(AIData aiData) {
        Collider2D[] targetColliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, playerLayer);
        colliders = new();
        foreach (Collider2D col in targetColliders) {
            if (col != null) {
                Vector2 dir = (col.transform.position - transform.position).normalized;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, detectionRadius, losLayer);
                if (hit.collider != null && (playerLayer & (1 << hit.collider.gameObject.layer)) != 0)
                    colliders.Add(col.transform);
            }
            else {
                colliders = null;
            }
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
