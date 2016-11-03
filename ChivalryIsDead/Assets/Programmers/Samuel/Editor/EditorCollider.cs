using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraMovement))]
public class CameraEditor : Editor
{

    public override void OnInspectorGUI()
    {

        CameraMovement camera = (CameraMovement)target;

        base.OnInspectorGUI();

        if (GUILayout.Button("Create New Area"))
        {
            camera.AddArea();
        }

        if (GUILayout.Button("Reset All Area"))
        {
            camera.ResetAll();
        }

    }

    void OnSceneGUI()
    {
        
        CameraMovement camera = (CameraMovement)target;

        for(int i = 0; i < camera.Areas.Count; i++)
        {

            float width = camera.Areas[i].width;
            float height = camera.Areas[i].height;

            Handles.color = Color.blue;
            /*Handles.Label(camera.transform.position + Vector3.up * 2,
                                 camera.transform.position.ToString() + "\nRange: " +
                                 range.ToString());*/

            Vector3 pos = new Vector3(camera.Areas[i].position.x, 0, camera.Areas[i].position.y);

            Vector3[] verts = new Vector3[] { new Vector3(pos.x, pos.y, pos.z),
                                              new Vector3(pos.x + width, pos.y, pos.z),
                                              new Vector3(pos.x + width, pos.y, pos.z - height),
                                              new Vector3(pos.x, pos.y, pos.z - height)};

            Handles.DrawSolidRectangleWithOutline(verts, new Color(1, 1, 1, 0.2f), new Color(0, 0, 0, 1));
            Handles.color = Color.white;

            float x = width;
            float y = height;


            Vector3 scale = new Vector3(width, 0, height);
            Vector3 handlePos = pos + new Vector3(width / 2, 0, height / 2);
            scale = Handles.ScaleHandle(scale, handlePos, Quaternion.identity, HandleUtility.GetHandleSize(handlePos));

            pos = Handles.PositionHandle(pos, Quaternion.identity);

            //x = Handles.ScaleValueHandle(x, verts[0], Quaternion.identity, 1, Handles.DoPositionHandle, 1);
            //x = Handles.ScaleValueHandle(x, verts[1], Quaternion.identity, 1, Handles.CubeCap, 1);
            //x = Handles.ScaleValueHandle(x, verts[2], Quaternion.identity, 1, Handles.CubeCap, 1);
            //x = Handles.ScaleValueHandle(x, verts[3], Quaternion.identity, 1, Handles.CubeCap, 1);

            camera.Areas[i] = new Rect(pos.x, pos.z, scale.x, scale.z);

        }
    }
}