using UnityEngine;
using System.Collections;

public class killSheepSound : MonoBehaviour {

	// Use this for initialization
	void OnDestroy () {
        AkSoundEngine.PostEvent("sheep_death", gameObject);
    }
	
	// Update is called once per frame
	void OnDisable () {
        AkSoundEngine.PostEvent("sheep_death", gameObject);
    }
}
