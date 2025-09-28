using System.Collections.Generic;
using UnityEngine;

public class ContextSolver : MonoBehaviour {
    [SerializeField] private bool showGizmos = false;
    private float[] interestGizmos = new float[8];
    private Vector2 resultDir;
    private float rayLength = 1;

    public Vector2 GetDirToMove(List<SteeringBehaviour> steerings, AIData aiData) {
        float[] danger = new float[8];
        float[] interest = new float[8];

        foreach (SteeringBehaviour steering in steerings) {
            (danger, interest) = steering.GetSteering(danger, interest, aiData);
        }

        for (int i = 0; i < 8; i++) {
            interest[i] = Mathf.Clamp01(interest[i] - danger[i]);
        }

        interestGizmos = interest;
        Vector2 outputDir = Vector2.zero;
        for (int i = 0; i < Directions.eightDirections.Length; i++) {
            outputDir += Directions.eightDirections[i] * interest[i];
        }
        outputDir = outputDir.normalized;
        return outputDir;
    }

    private void OnDrawGizmosSelected() {
        if (showGizmos == false) return;

        if (Application.isPlaying && resultDir != Vector2.zero) {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, resultDir * rayLength);
        }
    }
}
