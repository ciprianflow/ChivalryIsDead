using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UILanguageSystem : MonoBehaviour {

    public string[] textArray;



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(PlayerPrefs.GetString("Language") == "English")
        {
            gameObject.GetComponent<Text>().text = textArray[0];
        }

        if (PlayerPrefs.GetString("Language") == "Dansk")
        {
            gameObject.GetComponent<Text>().text = textArray[1];
        }
    }
}
