using UnityEngine;
using System.Collections;

public class CameraBillboard : MonoBehaviour
{
    public GameObject speaker;
    public Camera m_Camera;
    //public Transform target;
    //public float speed;
    Vector3 worldPos;
    Vector3 screenPos;

    void Start()
    {
        //transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward, m_Camera.transform.rotation * Vector3.up);
        //rotation = Quaternion.LookRotation(Vector3.up, Vector3.forward);


    }

    void Update()
    {

       
        worldPos = new Vector3(speaker.transform.position.x, speaker.transform.position.y, speaker.transform.position.z);
        screenPos = m_Camera.WorldToScreenPoint(worldPos);

        transform.position = new Vector3(screenPos.x, screenPos.y, screenPos.z);

        //transform.localRotation = Quaternion.Euler(-transform.parent.rotation.eulerAngles);
        //transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward, m_Camera.transform.rotation * Vector3.up);

        //float step = speed * Time.deltaTime;
        //transform.position = Vector3.MoveTowards(transform.position, target.position, step);

        //transform.rotation = rotation;

    }


}