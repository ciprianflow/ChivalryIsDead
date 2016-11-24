using UnityEngine;
using System.Collections;

public class Melee_Events : MonoBehaviour {

    public GameObject trail;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void setTrail(int state) {

        if(state == 1) 
            trail.SetActive(true);
        else
            trail.SetActive(false);
    }
}
