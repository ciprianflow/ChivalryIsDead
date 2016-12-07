using UnityEngine;
using System.Collections;

public class StartCinematicSound : MonoBehaviour {

	// Use this for initialization
	void Start () {

        WwiseInterface.Instance.SetMusic(MusicHandle.MusicStop);
        AkSoundEngine.PostEvent("cinematicIntro", gameObject);

    }

    // Update is called once per frame
    void Update () {
	
	}
}
