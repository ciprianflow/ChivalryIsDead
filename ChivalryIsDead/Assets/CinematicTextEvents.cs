using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CinematicTextEvents : MonoBehaviour {

    public Renderer textMat;
    public Texture tex;
    public List<Texture> speechBubbles;

    int curTex = -1;
	// Use this for initialization
	void Start () {
        //textMat.material.SetTexture("_MainTex", tex);

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void nextText() {
        curTex++;
        textMat.material.SetTexture("_MainTex", speechBubbles[curTex]);
    }
}
