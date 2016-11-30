using UnityEngine;
using System.Collections;

public class Language : MonoBehaviour {

    public GameObject menu;
    public GameObject language;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void English()
    {
        WwiseInterface.Instance.PlayMenuSound(MenuHandle.ForwardButtonPressed);
        PlayerPrefs.SetString("Language", "English");

        menu.SetActive(true);
        language.SetActive(false);

    }

    public void Dansk()
    {
        WwiseInterface.Instance.PlayMenuSound(MenuHandle.ForwardButtonPressed);
        PlayerPrefs.SetString("Language", "Dansk");

        menu.SetActive(true);
        language.SetActive(false);
    }
}
