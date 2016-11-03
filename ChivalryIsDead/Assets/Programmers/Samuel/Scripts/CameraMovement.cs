using UnityEngine;
using System.Collections.Generic;

public class CameraMovement : MonoBehaviour {

    void Awake()
    {

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
            movePos();
            moveRot();
        }
    }

    #region Editor Variables

    [SerializeField]
    public List<Rect> Areas = new List<Rect>();
    [SerializeField]
    public List<Vector3> FocusPoints = new List<Vector3>();
    [SerializeField]
    public List<Vector3> CamPoints = new List<Vector3>();

    public void AddArea()
    {

        Areas.Add(new Rect(0, 0, 2f, 5f));
        FocusPoints.Add(new Vector3(0, 0, 0));
        CamPoints.Add(new Vector3(0, 0, 0));

    }

    public void ResetAll()
    {
        Areas = new List<Rect>();
        FocusPoints = new List<Vector3>();
        CamPoints = new List<Vector3>();
    }

    #endregion

    #region movement

    [Header("Camera Variables")]
    public float distance = 1f;
    public float height = 1f;
    [Tooltip("Determines the amount of damping on the rotation")]
    [Range(0.0f, 3.0f)]
    public float rotationDamping = 1.2f;
    [Tooltip("Determines the amount of damping on the Postion")]
    [Range(0.0f, 3.0f)]
    public float postionDamping = 1.2f;

    [Header("Camera Settings")]
    public bool fixedRotation = false;
    public bool reverseRotation = false;

    [Header("GameObjects")]
    [Tooltip("Target that the camera is going to focus on ")]
    public Transform target;

    void movePos()
    {
        int multiplier = -1;
        if (reverseRotation)
            multiplier = 1;

        //Set height and distance
        if (fixedRotation)
        {
            Vector3 pos = (Vector3.forward * multiplier * distance) + target.position + new Vector3(0, height + target.position.y, 0);
            transform.position = Vector3.Slerp(transform.position, pos, Time.deltaTime * postionDamping);
        }
        else
        {
            Vector3 pos = (target.forward * multiplier * distance) + target.position + new Vector3(0, height + target.position.y, 0);
            transform.position = Vector3.Slerp(transform.position, pos, Time.deltaTime * postionDamping);
        }
        
    }

    void moveRot()
    {
        if (fixedRotation)
        {
            Vector3 D = target.position - transform.position;
            Quaternion rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(D), rotationDamping * Time.deltaTime);
            transform.rotation = rot;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, 0);
            return;
        }

        //Look at and dampen the rotation
        Quaternion rotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationDamping);
    }

    void setPos()
    {
        int multiplier = -1;
        if (reverseRotation)
            multiplier = 1;

        //Set height and distance
        if (fixedRotation)
        {
            Vector3 pos = (Vector3.forward * multiplier * distance) + target.position + new Vector3(0, height + target.position.y, 0);
            transform.position = pos;
        }
        else
        {
            Vector3 pos = (target.forward * multiplier * distance) + target.position + new Vector3(0, height + target.position.y, 0);
            transform.position = pos;
        }
    }

    void setRot()
    {
        if (fixedRotation)
        {
            Vector3 D = target.position - transform.position;
            Quaternion rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(D), rotationDamping * Time.deltaTime);
            transform.rotation = rot;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, 0);
            return;
        }

        Quaternion rotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = rotation;
    }

    Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles) {
       Vector3 dir = point - pivot; // get point direction relative to pivot
       dir = Quaternion.Euler(angles) * dir; // rotate it
       point = dir + pivot; // calculate rotated point
       return point; // return it
    }

    #endregion
}
