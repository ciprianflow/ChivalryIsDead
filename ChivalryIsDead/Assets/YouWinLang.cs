using UnityEngine;
using System.Collections;

public class YouWinLang : MonoBehaviour {

    public GameObject winDanish;
    public GameObject winEnglish;

	// Use this for initialization
	void Start () {

        if (PlayerPrefs.GetString("Language") == "Dansk")
        {
            winDanish.SetActive(true);
        }
        else if (PlayerPrefs.GetString("Language") == "English")
        {
            winEnglish.SetActive(true);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

  
}
