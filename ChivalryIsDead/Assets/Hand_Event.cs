using UnityEngine;
using System.Collections;

public class Hand_Event : MonoBehaviour {

    public Texture handBlood;
    public Renderer textMat;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void nextImage() {
        textMat.material.SetTexture("_MainTex", handBlood);
    }
}
