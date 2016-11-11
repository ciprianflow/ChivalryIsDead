using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(AreaScript))]
public class AreaEditor : Editor {

    int index = 0;
    public override void OnInspectorGUI()
    {

        AreaScript areaScript = (AreaScript)target;

        //base.OnInspectorGUI();

        GUIStyle style = new GUIStyle(GUI.skin.box);

        GUILayout.BeginVertical(style);

        GUILayout.Space(5);

        EditorGUI.indentLevel += 1;

        serializedObject.Update();

        EditorList.Show(serializedObject.FindProperty("properties"));

        serializedObject.ApplyModifiedProperties();

        EditorGUI.indentLevel -= 1;

        GUILayout.Space(10);

        GUILayout.EndVertical();

        GUILayout.Space(5);

        GUILayout.BeginVertical(style);

        GUILayout.Label("Options", EditorStyles.boldLabel);

        GUILayout.Space(5);

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Create New Spawn Area"))
        {
            areaScript.AddArea();
            EditorUtility.SetDirty(areaScript);
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        index = EditorGUILayout.IntField(index);
        if (GUILayout.Button("Delete Spawn Area"))
        {
            areaScript.RemoveArea(index);
            EditorUtility.SetDirty(areaScript);
        }

        GUILayout.EndHorizontal();

        if (GUILayout.Button("Clear All Spawn Area"))
        {
            areaScript.ResetAll();
            EditorUtility.SetDirty(areaScript);
        }

        GUILayout.Space(5);

        GUILayout.EndVertical();
    }

    void OnSceneGUI()
    {
        AreaScript areaScript = (AreaScript)target;

        for (int i = 0; i < areaScript.Areas.Count; i++)
        {

            float width = areaScript.Areas[i].width;
            float height = areaScript.Areas[i].height;

            GUIStyle style = new GUIStyle();
            style.normal.textColor = areaScript.properties[i].AreaColor;
            style.fontSize = 20;

            Handles.color = areaScript.properties[i].AreaColor;
            Vector3 pos = new Vector3(areaScript.Areas[i].x, 0, areaScript.Areas[i].y);

            Vector3[] verts = new Vector3[] { new Vector3(areaScript.Areas[i].xMin, pos.y, areaScript.Areas[i].yMin),
                                              new Vector3(areaScript.Areas[i].xMax, pos.y, areaScript.Areas[i].yMin),
                                              new Vector3(areaScript.Areas[i].xMax, pos.y, areaScript.Areas[i].yMax),
                                              new Vector3(areaScript.Areas[i].xMin, pos.y, areaScript.Areas[i].yMax)};

            Handles.DrawSolidRectangleWithOutline(verts, new Color(1, 1, 1, 0.2f), new Color(0, 0, 0, 1));
            Handles.color = Color.white;

            float x = width;
            float y = height;

            Vector3 scale = new Vector3(width, 0, height);
            Vector3 handlePos = pos + scale / 2;
            scale = Handles.ScaleHandle(scale, handlePos, Quaternion.AngleAxis(360, Vector3.left), HandleUtility.GetHandleSize(handlePos));

            Handles.color = Color.green;
            pos = Handles.PositionHandle(pos, Quaternion.identity);

            Handles.color = Color.blue;
            Handles.Label(pos + Vector3.up * 4 + new Vector3(1, 0, 1),
                                 i + " : " + areaScript.properties[i].SpawnType.ToString(), style);

            areaScript.Areas[i] = new Rect(pos.x, pos.z, scale.x, scale.z);

        }
    }
}
