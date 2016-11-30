using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MenuOptions : MonoBehaviour {

    public GameObject menu;
    public GameObject options;
    public GameObject areYouSure;

    public GameObject muteSound;
    public GameObject soundVolume;

    // Use this for initialization
    void Start () {

        // Check Volume Slider
        soundVolume.GetComponent<Slider>().value = PlayerPrefs.GetFloat("SoundVolume");

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
        PlayerPrefs.SetFloat("SoundVolume", soundVolume.GetComponent<Slider>().value);
    }

    public void SoundMute()
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            WwiseInterface.Instance.PlayMenuSound(MenuHandle.BackwardsButtonPressed);
            PlayerPrefs.SetInt("Sound", 0);
            Debug.Log("Off");
        }
        else if (PlayerPrefs.GetInt("Sound") == 0)
        {
            WwiseInterface.Instance.PlayMenuSound(MenuHandle.ForwardButtonPressed);
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
