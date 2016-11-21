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

        int offset = 20;
        float widthThird = position.width / 3;
        position.width = widthThird - 25;
        EditorGUI.PropertyField(new Rect(position.x - offset, position.y, position.width + offset, position.height), property.FindPropertyRelative("AreaColor"), GUIContent.none);
        position.width = widthThird;
        EditorGUI.PropertyField(new Rect(position.x + position.width - 25 - offset, position.y, position.width, position.height), property.FindPropertyRelative("SpawnType"), GUIContent.none);
        position.width = widthThird;
        EditorGUI.PropertyField(new Rect(position.x + position.width + 25, position.y, position.width, position.height), property.FindPropertyRelative("MaxSpawn"), GUIContent.none);
        position.width = widthThird;
        EditorGUI.PropertyField(new Rect(position.x + position.width * 2, position.y, position.width, position.height), property.FindPropertyRelative("QuestType"), GUIContent.none);
    }
}
