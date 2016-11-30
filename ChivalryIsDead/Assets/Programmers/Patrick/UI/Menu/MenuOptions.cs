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

    private bool IsMuted
    {
        get { return PlayerPrefs.GetInt("Sound") == 0; }
    }

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
            WwiseInterface.Instance.SetVolume(0, VolumeHandle.Master);
        }
        else if (PlayerPrefs.GetInt("Sound") == 1)
        {
            muteSound.GetComponent<Image>().color = Color.white;
            // Volume is set correctly in update loop, and doesn't need to be set here.
        }

    }
	
	// Update is called once per frame
	void Update () {
        if (!IsMuted) { 
            WwiseInterface.Instance.SetVolume(masterVolume.value * 100, VolumeHandle.Master);
            WwiseInterface.Instance.SetVolume(soundVolume.value * 100, VolumeHandle.SFX);
            WwiseInterface.Instance.SetVolume(musicVolume.value * 100, VolumeHandle.Music);
        }
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
            WwiseInterface.Instance.SetVolume(0, VolumeHandle.Master);
            muteSound.GetComponent<Image>().color = Color.red;
        }
        else if (PlayerPrefs.GetInt("Sound") == 0)
        {
            PlayerPrefs.SetInt("Sound", 1);
            muteSound.GetComponent<Image>().color = Color.white;
            // Volume is set correctly in update loop, and doesn't need to be set here.
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
