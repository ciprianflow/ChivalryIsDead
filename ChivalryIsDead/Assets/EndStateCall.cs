using UnityEngine;
using System.Collections;

public class EndStateCall : MonoBehaviour {

    public PlayerScript ps;

	// Use this for initialization
	void Start () {
        //Time.timeScale = 0.1f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void EndState(int value) {
        Debug.Log(value);
        ps.EndState(value);
    }

    void step() {

    }
}
