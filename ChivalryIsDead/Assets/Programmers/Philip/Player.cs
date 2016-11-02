using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    [Header("Variables")]
    public float maxSpeed = 0.5f;

    [Header("GameObjects")]
    public GameObject PlayerObj;

    public void move(float x, float y) {

        FixedPosition(x, y);

    }

    void FixedPosition(float x, float y)
    {

        float camRot = Mathf.Deg2Rad * Camera.main.transform.eulerAngles.y;

        float worldX = (x * Mathf.Cos(camRot)) - (y * Mathf.Sin(camRot));
        float worldY = (x * Mathf.Sin(camRot)) + (y * Mathf.Cos(camRot));

        //Debug.Log(Mathf.Atan2(worldY, worldX));
        PlayerObj.transform.eulerAngles = new Vector3(0, -Mathf.Rad2Deg * Mathf.Atan2(worldY, worldX), 0);
        transform.Translate(worldX * maxSpeed, 0, worldY * maxSpeed);

    }
}
