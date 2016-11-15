using UnityEngine;
using System.Collections;

public class kILLALsCRIPT : MonoBehaviour {

    float t = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        t += Time.deltaTime;
        if(t > 7) {
            Destroy(this.gameObject);
        }
	}
}
