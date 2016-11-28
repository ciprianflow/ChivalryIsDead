using UnityEngine;
using System.Collections;

public class AnimTimeScale : MonoBehaviour {
    Animator anim;
    // Use this for initialization
    void Awake () {
        anim = GetComponent<Animator>();
        anim.speed = 10f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void CloseWindow()
    {
        anim.SetBool("playGetHit", false);
        gameObject.SetActive(false);
    }
}
