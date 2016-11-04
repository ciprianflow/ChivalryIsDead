using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraMovement))]
public class CameraEditor : Editor
{
    int index = 0;
    public override void OnInspectorGUI()
    {

        CameraMovement camera = (CameraMovement)target;

        base.OnInspectorGUI();

        GUILayout.Space(10);

        GUILayout.Label("Areas", EditorStyles.boldLabel);

        if (GUILayout.Button("Create New Area"))
        {
            camera.AddArea();
            EditorUtility.SetDirty(camera);
        }

        GUILayout.BeginHorizontal();
        index = EditorGUILayout.IntField( index );
        if (GUILayout.Button("Delete Area"))
        {
            camera.RemoveArea(index);
            EditorUtility.SetDirty(camera);
        }

        GUILayout.EndHorizontal();

        if (GUILayout.Button("Reset All Area"))
        {
            camera.ResetAll();
            EditorUtility.SetDirty(camera);
        }
        
    }

    void OnSceneGUI()
    {
        
        CameraMovement camera = (CameraMovement)target;

        for(int i = 0; i < camera.Areas.Count; i++)
        {

            float width = camera.Areas[i].width;
            float height = camera.Areas[i].height;

            GUIStyle style = new GUIStyle();
            style.normal.textColor = camera.AreaColor[i];
            style.fontSize = 24;

            Handles.color = camera.AreaColor[i];
            Vector3 pos = new Vector3(camera.Areas[i].x, 0, camera.Areas[i].y);

            /*Vector3[] verts = new Vector3[] { new Vector3(pos.x, pos.y, pos.z),
                                              new Vector3(pos.x + width, pos.y, pos.z),
                                              new Vector3(pos.x + width, pos.y, pos.z - height),
                                              new Vector3(pos.x, pos.y, pos.z - height)};*/

            Vector3[] verts = new Vector3[] { new Vector3(camera.Areas[i].xMin, pos.y, camera.Areas[i].yMin),
                                              new Vector3(camera.Areas[i].xMax, pos.y, camera.Areas[i].yMin),
                                              new Vector3(camera.Areas[i].xMax, pos.y, camera.Areas[i].yMax),
                                              new Vector3(camera.Areas[i].xMin, pos.y, camera.Areas[i].yMax)};

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
                                 "Width: " + width.ToString() + ", Height: " + height.ToString(), style);

            camera.Areas[i] = new Rect(pos.x, pos.z, scale.x, scale.z);

            Vector3 FP = camera.FocusPoints[i];
            FP = Handles.PositionHandle(FP, Quaternion.identity);
            camera.FocusPoints[i] = new Vector3(FP.x, 0, FP.z);         
            Handles.Label(FP + Vector3.up * 4,
                                 "Focus Point for Area " + i, style);


            camera.CameraPoints[i] = Handles.PositionHandle(camera.CameraPoints[i], Quaternion.identity);
            Handles.Label(camera.CameraPoints[i] + Vector3.up * 4,
                                 "Camera Position For " + i, style);

        }
    }
}