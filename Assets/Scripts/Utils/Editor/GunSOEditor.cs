using UnityEditor;

[CustomEditor(typeof(GunSO))]
public class GunSOEditor : Editor {
    public override void OnInspectorGUI() {
        GunSO gunSO = (GunSO)target;
        GunType gunType = gunSO.gunType;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("gunType"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("visual"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("effectPref"));
        if (gunType != GunType.Pistol && gunType != GunType.SMG && gunType != GunType.Shotgun)
            EditorGUILayout.PropertyField(serializedObject.FindProperty("particlePref"));

        DrawCommonGUI();
        switch (gunType) {
            case GunType.LaserGun:
                DrawLaserGunGUI();
                break;
            case GunType.MiniGun:
                DrawMiniGunGUI();
                break;
            case GunType.Bazuca:
                DrawBazucaGUI();
                break;
            default:
                DrawRaycastGUI();
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawCommonGUI() {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("curLevel"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("maxLevel"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("damage"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("accuracy"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("reloadTime"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("weight"));
    }

    private void DrawRaycastGUI() {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("maxAmmo"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("particlesPerShot"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("range"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("deviation"));
    }

    private void DrawBazucaGUI() {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("maxAmmo"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("range"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("particleSpeed"));
    }

    private void DrawMiniGunGUI() {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("maxAmmo"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("range"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("deviation"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("particleSpeed"));
    }

    private void DrawLaserGunGUI() {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("dischargeRate"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("rechargeRate"));
    }
}