using UnityEngine;
using System.Collections;

public class Melee_Events : MonoBehaviour {

    public GameObject trail;
    public GameObject spearTrail;
    string[] sounds = new string[4] { "melee_death", "melee_fall_thump", "melee_spear_spin", "melee_spear_splat" };

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void setTrail(int state) {

        if(state == 1) 
            trail.SetActive(true);
        else
            trail.SetActive(false);
    }
    void setSpearTrail(int state) {

        if (state == 1)
            spearTrail.SetActive(true);
        else
            spearTrail.SetActive(false);
    }
    public void callSound(int sound) {
        AkSoundEngine.PostEvent(sounds[sound], gameObject);
    }
}
