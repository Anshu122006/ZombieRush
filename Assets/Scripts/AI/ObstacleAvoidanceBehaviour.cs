using UnityEngine;

public class ObstacleAvoidanceBehaviour : SteeringBehaviour {
    [SerializeField] private float radius = 2, agentColliderSize = 0.6f;

    // for gizmos purpose
    [SerializeField] private bool showGizmos;
    private float[] dangerResultTemp;

    public override (float[] danger, float[] interest) GetSteering(float[] danger, float[] interest, AIData aiData) {
        foreach (Collider2D obstacleCollider in aiData.obstacles) {
            Vector2 directionToObstacle = obstacleCollider.ClosestPoint(transform.position) - (Vector2)transform.position;
            float distanceToObstacle = directionToObstacle.magnitude;
            Vector2 directionToObstacleNormalised = directionToObstacle.normalized;

            float weight = distanceToObstacle <= agentColliderSize ? 1 : (radius - distanceToObstacle) / radius;

            for (int i = 0; i < Directions.eightDirections.Length; i++) {
                float result = Vector2.Dot(Directions.eightDirections[i], directionToObstacleNormalised);
                float valueToPutIn = result * weight;

                if (valueToPutIn > danger[i]) {
                    danger[i] = valueToPutIn;
                }
            }
        }

        dangerResultTemp = danger;
        return (danger, interest);
    }

    private void OnDrawGizmosSelected() {
        if (showGizmos == false) return;

        if (Application.isPlaying && dangerResultTemp != null) {
            Gizmos.color = Color.red;
            for (int i = 0; i < dangerResultTemp.Length; i++) {
                Gizmos.DrawRay(transform.position, Directions.eightDirections[i] * dangerResultTemp[i]);
            }
        }
        else {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
