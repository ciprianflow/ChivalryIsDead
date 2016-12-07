using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MenuOptions : MonoBehaviour {

    public GameObject menu;
    public GameObject options;
    public GameObject areYouSure;

    public GameObject muteSound;
    public Slider soundVolume;
    public Slider musicVolume;

    private bool IsMuted
    {
        get { return PlayerPrefs.GetInt("Sound") == 0; }
    }

    // Use this for initialization
    void Start () {

        // Check Volume Sliders
        soundVolume.value = PlayerPrefs.GetFloat("SoundVolume");
        musicVolume.value = PlayerPrefs.GetFloat("MusicVolume");

        // Check Mute
        if (IsMuted)
        {
            muteSound.GetComponent<Image>().color = Color.red;
            WwiseInterface.Instance.SetVolume(0, VolumeHandle.Master);
        }
        else
        {
            muteSound.GetComponent<Image>().color = Color.white;
            WwiseInterface.Instance.SetVolume(100, VolumeHandle.Master);
            // Volume is set correctly in update loop, and doesn't need to be set here.
            // EDIT: MASTER VOLUME IS INACCESSIBLE EXCEPT THROUGH BUTTONS, AND IS NOT UPDATED IN UPDATE LOOP!
        }

    }
	
	// Update is called once per frame
	void Update () {
        if (!IsMuted) { 
            //WwiseInterface.Instance.SetVolume(masterVolume.value * 100, VolumeHandle.Master);
            WwiseInterface.Instance.SetVolume(soundVolume.value * 100, VolumeHandle.SFX);
            WwiseInterface.Instance.SetVolume(musicVolume.value * 100, VolumeHandle.Music);
        }
    }

    public void Dansk()
    {
        WwiseInterface.Instance.PlayMenuSound(MenuHandle.ForwardButtonPressed);
        PlayerPrefs.SetString("Language", "Dansk");
    }

    public void English()
    {
        WwiseInterface.Instance.PlayMenuSound(MenuHandle.ForwardButtonPressed);
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
        //PlayerPrefs.SetFloat("MasterVolume", masterVolume.value);
    }

    public void SoundMute()
    {
        // Check Mute
        if (!IsMuted) {
            WwiseInterface.Instance.PlayMenuSound(MenuHandle.BackwardsButtonPressed);
            PlayerPrefs.SetInt("Sound", 0);
            muteSound.GetComponent<Image>().color = Color.red;
            WwiseInterface.Instance.SetVolume(0, VolumeHandle.Master);
        }
        else {
            WwiseInterface.Instance.PlayMenuSound(MenuHandle.ForwardButtonPressed);
            PlayerPrefs.SetInt("Sound", 1);
            muteSound.GetComponent<Image>().color = Color.white;
            WwiseInterface.Instance.SetVolume(100, VolumeHandle.Master);
            // Volume is set correctly in update loop, and doesn't need to be set here.
            // EDIT: MASTER VOLUME IS INACCESSIBLE EXCEPT THROUGH BUTTONS, AND IS NOT UPDATED IN UPDATE LOOP!
        }
    }

    public void Credits()
    {
        
    }

    public void AreYouSure()
    {
        WwiseInterface.Instance.PlayMenuSound(MenuHandle.ForwardButtonPressed);
        areYouSure.SetActive(true);
    }

    public void IAmNotSure()
    {
        WwiseInterface.Instance.PlayMenuSound(MenuHandle.BackwardsButtonPressed);
        areYouSure.SetActive(false);
    }

    public void ResetData()
    {
        WwiseInterface.Instance.PlayMenuSound(MenuHandle.ForwardButtonPressed);
        HubDataManager.ResetHubData();
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(0);
        
    }

    public void Back()
    {
        WwiseInterface.Instance.PlayMenuSound(MenuHandle.BackwardsButtonPressed);
        menu.SetActive(true);
        options.SetActive(false);
    }
}
