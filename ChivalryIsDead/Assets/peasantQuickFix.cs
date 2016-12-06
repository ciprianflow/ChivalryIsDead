using UnityEngine;
using System.Collections;

public class peasantQuickFix : MonoBehaviour {
    public GameObject questLetter;
    public Animator handAnim;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void letterAppear()
    {
        questLetter.SetActive(true);
    }

    public void StopHand()
    {
        handAnim.SetBool("playPeasant", false);
    }

}
