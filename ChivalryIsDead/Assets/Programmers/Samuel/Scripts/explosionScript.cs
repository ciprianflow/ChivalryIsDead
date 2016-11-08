using UnityEngine;
using System.Collections;

public class explosionScript : MonoBehaviour {

    float t1 = 0;
    float timer = 1f;
	
	// Update is called once per frame
	void Update () {
        t1 += Time.deltaTime;
        if(t1 > timer)
        {
            Destroy(this.gameObject);
        }
	}
}
