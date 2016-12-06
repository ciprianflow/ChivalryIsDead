using UnityEngine;
using System.Collections;

public class StartCinematicSound : MonoBehaviour {

	// Use this for initialization
	void Start () {
        AkSoundEngine.PostEvent("cinematicIntro", gameObject);
        AkSoundEngine.PostEvent("musicStop", gameObject);
    }

    // Update is called once per frame
    void Update () {
	
	}
}
