using UnityEngine;
using System.Collections;

public class rotateTest : MonoBehaviour {
    public SpeechBubbles bubbles;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(!bubbles.isPaused)
            transform.Rotate(0, 0.5f, 0);
	}
}
