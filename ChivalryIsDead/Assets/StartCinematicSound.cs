using UnityEngine;
using System.Collections;

public class StartCinematicSound : MonoBehaviour {

	// Use this for initialization
	void Start () {
<<<<<<< HEAD
        WwiseInterface.Instance.StopEvent("music1Play");
        WwiseInterface.Instance.StopEvent("musicquest");
        WwiseInterface.Instance.StopEvent("start_world_1_ambience");
        WwiseInterface.Instance.StopEvent("reward_combo_start");
=======
>>>>>>> 4001b32c51a2b315532867989a91cb998200ecd1

        WwiseInterface.Instance.SetMusic(MusicHandle.MusicStop);
        AkSoundEngine.PostEvent("cinematicIntro", gameObject);

    }

    // Update is called once per frame
    void Update () {
	
	}
}
