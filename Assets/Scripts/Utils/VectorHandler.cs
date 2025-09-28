using UnityEditor.Rendering;
using UnityEngine;

namespace Game.Utils {
    static class VectorHandler {
        public static float AngleFromVector(Vector3 v) {
            float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
            angle = angle < 0 ? 360 + angle : angle;
            return angle;
        }

        public static Quaternion RotationFromVector(Vector3 v) {
            float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
            return Quaternion.Euler(0, 0, angle);
        }

        public static Vector3 GenerateRandomDir(Vector3 baseDir, float del) {
            float a = Random.Range(-del, del);
            Quaternion rot = Quaternion.Euler(0, 0, a);
            Vector3 randomVector = rot * baseDir;

            return randomVector;
        }

        public static Vector2 ClampVector(Vector2 baseDir, Vector2 clampDir, float angularRange) {
            Vector2 anticlockwiseOffset = Quaternion.Euler(0, 0, -angularRange) * baseDir;
            Vector2 clockwiseOffset = Quaternion.Euler(0, 0, angularRange) * baseDir;

            float angularSep = Vector2.Angle(baseDir, clampDir);
            if (angularSep < angularRange) {
                return clampDir;
            }
            else {
                float cross = baseDir.x * clampDir.y - baseDir.y * clampDir.x;
                if (cross > 0) return clockwiseOffset;
                else return anticlockwiseOffset;
            }
        }
    }
}