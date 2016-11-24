using UnityEngine;
using System.Collections;

public class Sheep_Event : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void flip() {
        transform.eulerAngles = new Vector3(0, 0, 180);
    }
}
