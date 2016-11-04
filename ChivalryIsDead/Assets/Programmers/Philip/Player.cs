using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    [Header("Variables")]
    public float maxSpeed = 0.5f;

    public void move(float x, float y) {

        FixedPosition(x, y);

    }

    void FixedPosition(float x, float y)
    {

        Vector3 dir = new Vector3(x, 0, y);

        Vector3 fwd = Camera.main.transform.forward;
        fwd.y = transform.position.y;

        //float angle = Vector3.Angle(fwd, Vector3.forward) * Mathf.Deg2Rad;
        float angle = CalculateAngle(fwd, Vector3.forward) * Mathf.Deg2Rad;

        //angle = Camera.main.transform.rotation.y * Mathf.Deg2Rad;

        float worldX = (x * Mathf.Cos(angle)) - (y * Mathf.Sin(angle));
        float worldY = (x * Mathf.Sin(angle)) + (y * Mathf.Cos(angle));

        //dir = Quaternion.AngleAxis(angle, Vector3.up) * dir;

        //Debug.Log(Mathf.Atan2(worldY, worldX));
        //transform.eulerAngles = new Vector3(0, -Mathf.Rad2Deg * Mathf.Atan2(worldY, worldX), 0);

        transform.LookAt(transform.position - new Vector3(worldX * maxSpeed, 0, worldY * maxSpeed));

        transform.position -= new Vector3(worldX * maxSpeed, 0, worldY * maxSpeed);

        //transform.position += dir * maxSpeed;
    }

    public static float CalculateAngle(Vector3 from, Vector3 to)
    {
        return Quaternion.FromToRotation(Vector3.up, to - from).eulerAngles.z;
    }
}
