using UnityEngine;
using System.Collections.Generic;

public class CameraMovement : MonoBehaviour {

    void Awake()
    {

        RectsContainingTarget = new List<int>();
        updateAreaCamVaribles();

        if (!target)
            target = GameObject.Find("Player").transform;

        //Setting camera position
        setPos();
        //Setting camera rotation
        setRot();
        distToFP = CP - target.position;

    }

    void LateUpdate()
    {
        if (target)
        {
            if (FixedPosition)
            {
                UpdateCameraMovementTimer();
            
                updateAreaCamVaribles();
                if (!reachedDestination)
                {
                    movePos();
                    moveRot();
                }
            }else
            {
                movePos();
                moveRot();
            }   
        }
    }

    #region Camera Area Control

    [SerializeField]
    public List<Rect> Areas = new List<Rect>();
    [SerializeField]
    public List<Vector3> FocusPoints = new List<Vector3>();
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
        Vector3 oldCP = CP;
        updatePoint(CameraPoints, out CP);
        if(oldCP != CP)
        {
            startTransistion();
        }
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
    [Tooltip("Camera Transition Time")]
    public float cameraTransitionTime = 1f;
    [Tooltip("Determines the amount of damping on the rotation")]
    [Range(0.0f, 3.0f)]
    public float rotationDamping = 1.2f;
    [Tooltip("Determines the amount of damping on the Postion")]
    [Range(0.0f, 3.0f)]
    public float positionDamping = 1.2f;

    [Header("Camera Settings")]
    public bool FixedPosition = true;
    public bool InstantTransisition = false;
    public bool FixedRotation = false;
    public bool FixedZAxis = false;

    [Header("Transistion Curve")]
    [Tooltip("Enabling this will smooth the transition To the curve beneath")]
    public bool EnableTransistionCurve = false;
    public AnimationCurve curve;

    [Header("GameObjects")]
    [Tooltip("Target that the camera is going to focus on ")]
    public Transform target;

    //Area transition
    private float cameraMovementT = 0f;
    private bool reachedDestination = false;
    private Vector3 oldPos = Vector3.zero;
    private Quaternion oldRot = Quaternion.identity;

    void movePos()
    {

        //Set height and distance
        if (!FixedPosition)
        {
            Vector3 pos;
            if (FixedZAxis)
                pos = CP + new Vector3(0,0,target.position.z) + new Vector3(0, target.position.y, 0);
            else
            {
                pos = distToFP + target.position + new Vector3(0, target.position.y, 0);

                if (Vector3.Distance(target.position, transform.position) < distToFP.magnitude)
                    transform.position = Vector3.Slerp(transform.position, pos, Time.deltaTime * positionDamping);
            }
                
            transform.position = Vector3.Slerp(transform.position, pos, Time.deltaTime * positionDamping);

        }
        else // FOR FIXED POSITION
        {
            transform.position = Vector3.Lerp(oldPos, CP, cameraMovementT * positionDamping);
        }
        
    }

    void moveRot()
    {
        if (!FixedPosition)
        {
            if (FixedRotation)
                return;

            Quaternion rotation = Quaternion.LookRotation(target.position - transform.position);
            transform.rotation = rotation;
        }
        else // FOR FIXED POSITION
        {
            Quaternion rotation = Quaternion.LookRotation(FP - transform.position);
            transform.rotation = Quaternion.Lerp(oldRot, rotation, cameraMovementT * rotationDamping);
        }   
    }

    void UpdateCameraMovementTimer()
    {
        if (EnableTransistionCurve)
            cameraMovementT += (Time.deltaTime / cameraTransitionTime) * curve.Evaluate(cameraMovementT);
        else
            cameraMovementT += Time.deltaTime / cameraTransitionTime;

        if(cameraMovementT >= 1)
        {
            //cameraMovementT = 0f;
            reachedDestination = true;
        }
    }

    void setPos()
    {
        Debug.Log("Setting camera position");
        //Set height and distance
        transform.position = CP;

        oldPos = transform.position;
    }

    void setRot()
    {
        Debug.Log("Setting camera rotation");

        Quaternion rotation = Quaternion.LookRotation(FP - transform.position);
        transform.rotation = rotation;

        oldRot = transform.rotation;
        
    }

    void startTransistion()
    {
        if (!InstantTransisition)
            cameraMovementT = 0f;

        oldPos = transform.position;
        oldRot = transform.rotation;
        reachedDestination = false;   
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

    public void enableKillCam()
    {
        this.enabled = false;
        this.GetComponent<KillCam>().enabled = true;
    }

}
