using System;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(AreaProperty))]
public class AreaPropertyDrawer : PropertyDrawer {

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {

        SerializedProperty color = property.FindPropertyRelative("AreaColor");
        SerializedProperty spawnType = property.FindPropertyRelative("SpawnType");

        EditorGUI.BeginChangeCheck();

        Rect newPos = position;

        EditorGUI.PropertyField(new Rect(position.x, position.y, position.width /2, position.height), property.FindPropertyRelative("AreaColor"), GUIContent.none);
        position.width = position.width / 2;
        EditorGUI.PropertyField(new Rect(position.x + position.width - 5, position.y, position.width, position.height), property.FindPropertyRelative("SpawnType"), GUIContent.none);

    }
}
