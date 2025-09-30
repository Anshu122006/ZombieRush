using UnityEngine;
using UnityEngine.UIElements;

public static class WeightCalculator {
    public static float[] GetObstacleWeights(float[] danger, AIData aiData, Vector2 pos, float collisionRadius, float radius) {
        foreach (Collider2D obstacleCollider in aiData.obstacles) {
            Vector2 directionToObstacle = obstacleCollider.ClosestPoint(pos) - pos;
            float distanceToObstacle = directionToObstacle.magnitude;
            Vector2 directionToObstacleNormalised = directionToObstacle.normalized;

            float weight = distanceToObstacle <= collisionRadius ? 1 : (radius - distanceToObstacle) / radius;

            for (int i = 0; i < 8; i++) {
                float result = Vector2.Dot(Directions.directions[i], directionToObstacleNormalised);
                float valueToPutIn = result * weight;

                if (valueToPutIn > danger[i]) {
                    danger[i] = valueToPutIn;
                }
            }
        }
        return danger;
    }

    public static float[] GetDirWeights(float[] interest, Vector2 targetDir) {
        targetDir = targetDir.normalized;
        for (int i = 0; i < 8; i++) {
            float result = Vector2.Dot(Directions.directions[i], targetDir);
            result = Mathf.Clamp01(result);
            interest[i] = result;
        }
        return interest;
    }

    public static float[] GetDirWeightInRange(float[] interest, Vector2 targetDir, Vector2 rangeDir, float range, float dist) {
        targetDir = targetDir.normalized;
        rangeDir = rangeDir.normalized;
        for (int i = 0; i < 8; i++) {
            float w1 = (Vector2.Dot(Directions.directions[i], targetDir) + 1f) * 0.5f;
            float w2 = Mathf.Clamp01(Vector2.Dot(Directions.directions[i], rangeDir)) * (dist / range);
            interest[i] = Mathf.Clamp01(w1 * (1 - w2));
        }
        return interest;
    }
}

public static class Directions {
    public static Vector2[] directions = {
        new Vector2(1,0).normalized,
        new Vector2(-1,0).normalized,
        new Vector2(0,1).normalized,
        new Vector2(0,-1).normalized,
        new Vector2(1,1).normalized,
        new Vector2(-1,-1).normalized,
        new Vector2(1,-1).normalized,
        new Vector2(-1,1).normalized,
     };
}
