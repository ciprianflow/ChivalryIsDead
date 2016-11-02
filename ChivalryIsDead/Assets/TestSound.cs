using UnityEngine;
using System.Collections;

public class TestSound : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if(PlayerPrefs.GetInt("Sound") == 1)
        {
            gameObject.GetComponent<AudioSource>().mute = false;
        }

        if (PlayerPrefs.GetInt("Sound") == 0)
        {
            gameObject.GetComponent<AudioSource>().mute = true;
        }

        gameObject.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SoundVolume");

    }
}
