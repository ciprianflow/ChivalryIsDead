using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(MapManager))]
public class MapManagerEditor : Editor {

    public override void OnInspectorGUI()
    {

        MapManager mm = (MapManager)target;

        base.DrawDefaultInspector();

        if(GUILayout.Button("Create Quest Type Objects"))
        {
            mm.CreateQuestTypeObjects();
        }
    }
}
