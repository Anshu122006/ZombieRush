using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomPropertyDrawer(typeof(GameVariable))]
public class GameVariableDrawer : PropertyDrawer {
    // private static Dictionary<string, bool> foldouts = new Dictionary<string, bool>();

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        float lineHeight = EditorGUIUtility.singleLineHeight;
        float spacing = EditorGUIUtility.standardVerticalSpacing;

        var name = property.FindPropertyRelative("name");
        var value = property.FindPropertyRelative("value");

        // Calculate layout
        float fieldWidth = (position.width - spacing) / 2f;
        float labelY = position.y;
        float fieldY = labelY + lineHeight + spacing / 2;

        // Draw labels
        EditorGUI.LabelField(new Rect(position.x, labelY, fieldWidth, lineHeight), "Name");
        EditorGUI.LabelField(new Rect(position.x + fieldWidth + spacing, labelY, fieldWidth, lineHeight), "Value");

        // Draw fields
        EditorGUI.PropertyField(
            new Rect(position.x, fieldY, fieldWidth, lineHeight), name, GUIContent.none);
        EditorGUI.PropertyField(
            new Rect(position.x + fieldWidth + spacing, fieldY, fieldWidth, lineHeight), value, GUIContent.none);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        float lineHeight = EditorGUIUtility.singleLineHeight;
        float spacing = EditorGUIUtility.standardVerticalSpacing;
        return lineHeight + spacing + lineHeight;
    }
}
