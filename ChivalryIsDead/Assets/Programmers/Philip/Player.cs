using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {


    public float maxSpeed = 0.5f;
    public GameObject PlayerObj;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void move(float x, float y) {
        
        float worldX = (x * Mathf.Cos(-Mathf.PI/4)) - (y * Mathf.Sin(-Mathf.PI / 4));
        float worldY = (x * Mathf.Sin(-Mathf.PI/4)) + (y * Mathf.Cos(-Mathf.PI / 4));

        //Debug.Log(Mathf.Atan2(worldY, worldX));
        PlayerObj.transform.eulerAngles = new Vector3(0, -Mathf.Rad2Deg * Mathf.Atan2(worldY, worldX), 0);
        transform.Translate(worldX * maxSpeed, 0, worldY * maxSpeed);
    }
}
