using UnityEngine;
using System.Collections;

public class Sheep_flying : MonoBehaviour {

    public bool flying;
    public GameObject deathParticles;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnCollisionStay(Collision coll) {
    
        if (coll.transform.CompareTag("Ground") && flying) {
            GameObject dP = Instantiate(deathParticles) as GameObject;
            dP.transform.position = transform.position;
            this.gameObject.SetActive(false);
        }
    }
}
