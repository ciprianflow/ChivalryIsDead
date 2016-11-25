using UnityEngine;
using System.Collections;

public class AnimTimeScale : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        Animator anim = GetComponent<Animator>();
        anim.speed = 10f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void CloseWindow()
    {
        gameObject.SetActive(false);
    }
}
