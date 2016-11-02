using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

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

    

	void Awake ()
    {
       
        if(target)
            setPos();

    }

    void LateUpdate()
    {
        if (target)
        {
            setPos();
            setRot();
        }     
        
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
            transform.position = Vector3.Slerp(transform.position, pos, Time.deltaTime * postionDamping);
        }
        else
        {
            Vector3 pos = (target.forward * multiplier * distance) + target.position + new Vector3(0, height + target.position.y, 0);
            transform.position = Vector3.Slerp(transform.position, pos, Time.deltaTime * postionDamping);
        }
        
    }

    void setRot()
    {
        if (fixedRotation)
            return;

        //Look at and dampen the rotation
        Quaternion rotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationDamping);
    }
}
