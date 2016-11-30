using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MenuOptions : MonoBehaviour {

    public GameObject menu;
    public GameObject options;
    public GameObject areYouSure;

    public GameObject muteSound;
    public Slider masterVolume;
    public Slider soundVolume;
    public Slider musicVolume;

    // Use this for initialization
    void Start () {

        // Check Volume Sliders
        masterVolume.value = PlayerPrefs.GetFloat("MasterVolume");
        soundVolume.value = PlayerPrefs.GetFloat("SoundVolume");
        musicVolume.value = PlayerPrefs.GetFloat("MusicVolume");

        // Check Mute

        if (PlayerPrefs.GetInt("Sound") == 0)
        {
            muteSound.GetComponent<Image>().color = Color.red;
        }
        else if (PlayerPrefs.GetInt("Sound") == 1)
        {
            muteSound.GetComponent<Image>().color = Color.white;

        }

    }
	
	// Update is called once per frame
	void Update () {

        WwiseInterface.Instance.SetVolume(masterVolume.value * 100, VolumeHandle.Master);
        WwiseInterface.Instance.SetVolume(soundVolume.value * 100, VolumeHandle.SFX);
        WwiseInterface.Instance.SetVolume(musicVolume.value * 100, VolumeHandle.Music);

    }

    public void Dansk()
    {
        PlayerPrefs.SetString("Language", "Dansk");
    }

    public void English()
    {
        PlayerPrefs.SetString("Language", "English");
    }

    public void SoundVolume()
    {
        PlayerPrefs.SetFloat("SoundVolume", soundVolume.value);
    }

    public void MusicVolume()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicVolume.value);
    }

    public void MasterVolume()
    {
        PlayerPrefs.SetFloat("MasterVolume", masterVolume.value);
    }

    public void SoundMute()
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            PlayerPrefs.SetInt("Sound", 0);
            Debug.Log("Off");
        }
        else if (PlayerPrefs.GetInt("Sound") == 0)
        {
            PlayerPrefs.SetInt("Sound", 1);
            Debug.Log("On");
        }

        if (PlayerPrefs.GetInt("Sound") == 0)
        {
            muteSound.GetComponent<Image>().color = Color.red;
        }
        else if (PlayerPrefs.GetInt("Sound") == 1)
        {
            muteSound.GetComponent<Image>().color = Color.white;

        }
    }

    public void Credits()
    {
        
    }

    public void AreYouSure()
    {
        areYouSure.SetActive(true);
    }

    public void IAmNotSure()
    {
        areYouSure.SetActive(false);
    }

    public void ResetData()
    {
        HubDataManager.ResetHubData();
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(0);
        
    }

    public void Back()
    {
        menu.SetActive(true);
        options.SetActive(false);
    }
}
