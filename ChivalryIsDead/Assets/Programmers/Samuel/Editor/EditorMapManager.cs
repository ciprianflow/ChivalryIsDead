using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(MapManager))]
public class EditorMapManager : Editor {

    int index = 0;
    public override void OnInspectorGUI()
    {

        MapManager mapManager = (MapManager)target;

        base.OnInspectorGUI();

        GUILayout.Space(10);

        GUILayout.Label("Spawn Areas", EditorStyles.boldLabel);

        if (GUILayout.Button("Create New Spawn Area"))
        {
            mapManager.AddArea();
            EditorUtility.SetDirty(mapManager);
        }

        GUILayout.BeginHorizontal();
        index = EditorGUILayout.IntField(index);
        if (GUILayout.Button("Delete Spawn Area"))
        {
            mapManager.RemoveArea(index);
            EditorUtility.SetDirty(mapManager);
        }

        GUILayout.EndHorizontal();

        if (GUILayout.Button("Clear All Spawn Area"))
        {
            mapManager.ResetAll();
            EditorUtility.SetDirty(mapManager);
        }
    }

    void OnSceneGUI()
    {

        MapManager mapManager = (MapManager)target;

        for (int i = 0; i < mapManager.SpawnAreas.Count; i++)
        {

            float width = mapManager.SpawnAreas[i].width;
            float height = mapManager.SpawnAreas[i].height;

            GUIStyle style = new GUIStyle();
            style.normal.textColor = mapManager.AreaColor[i];
            style.fontSize = 24;

            Handles.color = mapManager.AreaColor[i];
            Vector3 pos = new Vector3(mapManager.SpawnAreas[i].x, 0, mapManager.SpawnAreas[i].y);

            Vector3[] verts = new Vector3[] { new Vector3(mapManager.SpawnAreas[i].xMin, pos.y, mapManager.SpawnAreas[i].yMin),
                                              new Vector3(mapManager.SpawnAreas[i].xMax, pos.y, mapManager.SpawnAreas[i].yMin),
                                              new Vector3(mapManager.SpawnAreas[i].xMax, pos.y, mapManager.SpawnAreas[i].yMax),
                                              new Vector3(mapManager.SpawnAreas[i].xMin, pos.y, mapManager.SpawnAreas[i].yMax)};

            Handles.DrawSolidRectangleWithOutline(verts, new Color(1, 1, 1, 0.2f), new Color(0, 0, 0, 1));
            Handles.color = Color.white;

            float x = width;
            float y = height;

            Vector3 scale = new Vector3(width, 0, height);
            Vector3 handlePos = pos + scale / 2;
            scale = Handles.ScaleHandle(scale, handlePos, Quaternion.AngleAxis(180, Vector3.left), HandleUtility.GetHandleSize(handlePos));

            Handles.color = Color.green;
            pos = Handles.PositionHandle(pos, Quaternion.identity);

            Handles.color = Color.blue;
            Handles.Label(handlePos + Vector3.up * 4,
                                 "Spawn Area " + i + " : Width: " + width.ToString() + ", Height: " + height.ToString(), style);

            mapManager.SpawnAreas[i] = new Rect(pos.x, pos.z, scale.x, scale.z);

        }
    }
}
