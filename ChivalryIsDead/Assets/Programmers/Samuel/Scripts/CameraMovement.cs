using UnityEngine;
using System.Collections.Generic;

public class CameraMovement : MonoBehaviour {

    void Awake()
    {

        RectsContainingTarget = new List<int>();
        updateAreaCamVaribles();

        if (!target)
            target = GameObject.Find("Player").transform;

        setPos();
        setRot();

        if (target)
            movePos();
    }

    void LateUpdate()
    {
        if (target)
        {
            updateAreaCamVaribles();
            movePos();
            moveRot();
        }
    }

    #region Camera Area Control

    [HideInInspector]
    [SerializeField]
    public List<Rect> Areas = new List<Rect>();
    [HideInInspector]
    [SerializeField]
    public List<Vector3> FocusPoints = new List<Vector3>();
    [HideInInspector]
    [SerializeField]
    public List<Vector3> CameraPoints = new List<Vector3>();
    [SerializeField]
    public List<Color> AreaColor = new List<Color>();

    List<int> RectsContainingTarget;

    public void AddArea()
    {

        Areas.Add(new Rect(0, 0, 2f, 5f));
        FocusPoints.Add(new Vector3(0, 0, 0));
        CameraPoints.Add(new Vector3(0, 0, 0));
        AreaColor.Add(new Color(0.2f, 0.2f, 0.9f, 1f));

    }

    public void RemoveArea(int index)
    {
        if (index > Areas.Count - 1)
            return;

        Areas.RemoveAt(index);
        FocusPoints.RemoveAt(index);
        CameraPoints.RemoveAt(index);
        AreaColor.RemoveAt(index);

    }

    public void ResetAll()
    {
        Areas = new List<Rect>();
        FocusPoints = new List<Vector3>();
        CameraPoints = new List<Vector3>();
        AreaColor = new List<Color>();
    }

    void updateAreaCamVaribles()
    {
        UpdateRectsContainingPlayer();
        updatePoint(FocusPoints, out FP);
        updatePoint(CameraPoints, out CP);
    }

    void UpdateRectsContainingPlayer()
    {  
        RectsContainingTarget.Clear();
        for (int i = 0; i < Areas.Count; i++)
        {
            //Debug.Log(target.position.x + ", " + target.position.z + " " + Areas[i].ToString() + " - " + Areas[i].Contains(new Vector3(target.position.x, target.position.z), true));
            if (Areas[i].Contains(new Vector3(target.position.x, target.position.z), true)){

                RectsContainingTarget.Add(i);
            }
        }
    }

    void updatePoint(List<Vector3> pList, out Vector3 p)
    {
        //Debug.Log(RectsContainingTarget.Count);
        if (RectsContainingTarget.Count == 0)
            p = Vector3.zero;
        else if (RectsContainingTarget.Count == 1)
            p = pList[RectsContainingTarget[0]];
        else
        {
            p = Interpolate(pList);
        }
    }

    #endregion

    #region movement

    //Distance to focus point
    private Vector3 FP; //FocusPoint
    private Vector3 CP; // Camera Point
    private Vector3 distToFP;

    [Header("Camera Variables")]
    public float height = 1f;
    [Tooltip("Determines the amount of damping on the rotation")]
    [Range(0.0f, 3.0f)]
    public float rotationDamping = 1.2f;
    [Tooltip("Determines the amount of damping on the Postion")]
    [Range(0.0f, 3.0f)]
    public float postionDamping = 1.2f;

    [Header("Camera Settings")]
    public bool FixedPostion = true;
    public bool FixedHeight = false;

    [Header("GameObjects")]
    [Tooltip("Target that the camera is going to focus on ")]
    public Transform target;

    void movePos()
    {

        //Set height and distance
        if (!FixedPostion)
        {
            Vector3 pos = distToFP + target.position + new Vector3(0, height + target.position.y, 0);
            transform.position = Vector3.Slerp(transform.position, pos, Time.deltaTime * postionDamping);
        }
        else
        {
            transform.position = Vector3.Slerp(transform.position, CP, Time.deltaTime * postionDamping);
        }
        
    }

    void moveRot()
    {
        if (!FixedPostion)
        {
            Vector3 D = target.position - transform.position;
            Quaternion rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(D), rotationDamping * Time.deltaTime);
            transform.rotation = rot;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, 0);
            return;
        }else
        {
            //Look at and dampen the rotation
            Quaternion rotation = Quaternion.LookRotation(FP - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationDamping);
        }   
    }

    void setPos()
    {
        //Set height and distance
        if (!FixedPostion)
        {
            Vector3 pos = distToFP + target.position + new Vector3(0, height + target.position.y, 0);
            transform.position = pos;
        }
        else
        {
            Vector3 pos = CP;
        }
    }

    void setRot()
    {
        if (!FixedPostion)
        {
            Vector3 D = target.position - transform.position;
            Quaternion rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(D), rotationDamping * Time.deltaTime);
            transform.rotation = rot;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, 0);
        }else
        {
            Quaternion rotation = Quaternion.LookRotation(FP - transform.position);
            transform.rotation = rotation;
        }

        
    }

    #endregion

    #region Helper functions

    Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles) {
       Vector3 dir = point - pivot; // get point direction relative to pivot
       dir = Quaternion.Euler(angles) * dir; // rotate it
       point = dir + pivot; // calculate rotated point
       return point; // return it
    }

    Vector3 Interpolate(List<Vector3> list)
    {
        Vector3 p = Vector3.zero;
        for(int i = 0; i < RectsContainingTarget.Count; i++)
        {
            p += list[RectsContainingTarget[i]];
        }
        p /= RectsContainingTarget.Count;
        return p;
    }

    #endregion

}
