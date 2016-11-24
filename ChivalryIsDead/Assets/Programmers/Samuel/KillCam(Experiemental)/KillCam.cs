using UnityEngine;
using System.Collections;

public class KillCam : MonoBehaviour {

    public Transform target;
    public float speed = 5f;
    public float distance = 2f;
    public float height = 4f;
	
    private float timer = 0f;
    public float duration = 2f;

    void OnEnable()
    {

        this.transform.position = new Vector3(distance, height, 0) + target.position;
        timer = 0;

    }

	// Update is called once per frame
	void Update () {

        timer += Time.deltaTime;

        if (timer >= duration)
        {
            this.enabled = false;
            this.GetComponent<CameraMovement>().enabled = true;
        }

        Quaternion rotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = rotation;
        transform.position = Vector3.Lerp(transform.position, transform.position + transform.right, Time.fixedDeltaTime * speed);

    }
}
