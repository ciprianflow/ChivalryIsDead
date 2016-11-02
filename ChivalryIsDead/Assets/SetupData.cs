using UnityEngine;
using System.Collections;

public class SetupData : MonoBehaviour {

    public GameObject menu;
    public GameObject language;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (!PlayerPrefs.HasKey("Setup"))
        {
            PlayerPrefs.SetInt("Sound", 1);
            PlayerPrefs.SetFloat("SoundVolume", 1);
            PlayerPrefs.SetFloat("SoundMusic", 1);
            PlayerPrefs.SetFloat("SoundSound", 1);

            //PlayerPrefs.SetInt("Level", 1);

            

            language.SetActive(true);

            PlayerPrefs.SetString("Setup", "Done");
            Debug.Log("Data Saved");
            gameObject.SetActive(false);

        } else
        {
            menu.SetActive(true);
            gameObject.SetActive(false);
        }

    }
}
