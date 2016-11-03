using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(TextGeneration))]
public class TextGenEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        TextGeneration myTarget = (TextGeneration)target;

        //EditorGUILayout.Toggle("test", myTarget.isTest);

        if (GUILayout.Button("Generate Text"))
        {
            myTarget.TextGenerator(myTarget.shuffleBagHello);
        }

        // Setup Editor layout.
        EditorGUILayout.LabelField("press what you want");
        for (int i = 0; i < myTarget.shuffleBagHello.Size; i++)
        {
            // Create row of toggle controls.
            EditorGUILayout.BeginHorizontal(GUILayout.MaxHeight(10), GUILayout.MaxWidth(7 * 12));
          
        
            if(EditorGUILayout.Toggle(myTarget.isTest))
                myTarget.TextGenerator(myTarget.shuffleBagHello);
            

            EditorGUILayout.EndHorizontal();
        }
    }
}