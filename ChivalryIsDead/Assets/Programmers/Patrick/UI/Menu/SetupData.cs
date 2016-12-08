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
            PlayerPrefs.SetString("Language", "Dansk"); // Altid start med dansk

            PlayerPrefs.SetInt("Sound", 1);
            PlayerPrefs.SetFloat("SoundVolume", 1);
            PlayerPrefs.SetFloat("MusicVolume", 1);
            //PlayerPrefs.SetFloat("SoundSound", 1);
            PlayerPrefs.SetInt("Swapped", 0);

            // Gameplay
            //PlayerPrefs.SetInt("numDay", 0);
            PlayerPrefs.SetInt("lowCombo", 0);
            PlayerPrefs.SetInt("noGetHit", 1); //yes patrick 1 is the correct one
            PlayerPrefs.SetInt("noSheepKill", 1);
            PlayerPrefs.SetInt("noTaunt", 1);
            PlayerPrefs.SetInt("noOverreact", 1);
            PlayerPrefs.SetInt("poorlyOverreact", 1);

            PlayerPrefs.SetInt("SuicideLevel", 0);
            PlayerPrefs.SetInt("SuicideTut", 0);

            // Player Controls
            PlayerPrefs.SetInt("Attack", 0);
            PlayerPrefs.SetInt("Taunt", 0);
            PlayerPrefs.SetInt("Overreact", 0);

            // Tut Levels
            PlayerPrefs.SetInt("AttackLevel", 0);
            PlayerPrefs.SetInt("TauntLevel", 0);
            PlayerPrefs.SetInt("OverreactLevel", 0);
            

            //PlayerPrefs.SetInt("Level", 1);



            menu.SetActive(true);

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
