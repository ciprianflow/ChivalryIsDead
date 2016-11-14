using UnityEngine;
using System.Collections;

public class TestWin : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDisable()
    {
        Application.LoadLevel(0);
    }
}
