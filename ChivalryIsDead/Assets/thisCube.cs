using UnityEngine;
using System.Collections;

public class thisCube : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(noesplode());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    IEnumerator noesplode() {
        yield return new WaitForSeconds(3);
        GetComponent<Rigidbody>().AddExplosionForce(5000, transform.position, 20);
        
    }
}
