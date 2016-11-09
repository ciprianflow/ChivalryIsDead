using UnityEngine;
using UnityEditor;

public static class EditorList {

	public static void Show (SerializedProperty list)
    {
        
        EditorGUILayout.PropertyField(list);
        EditorGUI.indentLevel += 1;
        if (list.isExpanded)
        {
            for (int i = 0; i < list.arraySize; i++)
            {
                EditorGUILayout.LabelField("Area " + i);
                EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i));
            }
        }
        EditorGUI.indentLevel -= 1;
    }
}
