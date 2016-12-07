using UnityEngine;
using System.Collections;

public class StartCinematicSound : MonoBehaviour {

	// Use this for initialization
	void Start () {
        WwiseInterface.Instance.StopEvent("music1Play");
        WwiseInterface.Instance.StopEvent("musicquest");
        WwiseInterface.Instance.StopEvent("start_world_1_ambience");
        WwiseInterface.Instance.StopEvent("reward_combo_start");


        WwiseInterface.Instance.SetMusic(MusicHandle.MusicStop);
        AkSoundEngine.PostEvent("cinematicIntro", gameObject);

    }

    // Update is called once per frame
    void Update () {
	
	}
}
