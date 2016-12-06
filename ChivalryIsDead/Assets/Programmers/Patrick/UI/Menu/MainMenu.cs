using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public GameObject menu;
    public GameObject options;
    public GameObject loading;

    public GameObject ContinueButton;

    Scene testGame;

	// Use this for initialization
	void Start ()
    {
        SetVolumes();

        // Start the background music.
        WwiseInterface.Instance.SetMusic(MusicHandle.MusicOnePlay);

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        if (PlayerPrefs.GetInt("AttackLevel") == 0)
        {
            ContinueButton.GetComponent<Image>().color = Color.red;
        }
        
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }


    public void DevPlay()
    {
        WwiseInterface.Instance.PlayMenuSound(MenuHandle.PlayButtonPressed);
        //Application.OpenURL("https://www.youtube.com/watch?v=AuRXVMSG3po");

        PlayerPrefs.SetInt("Attack", 1);
        PlayerPrefs.SetInt("Taunt", 1);
        PlayerPrefs.SetInt("Overreact", 1);

        SceneManager.LoadScene("ProtoHubWorld 1");
    }


    public void Continue()
    {
        StaticData.pressedContinue = true;

        WwiseInterface.Instance.PlayMenuSound(MenuHandle.PlayButtonPressed);

        if (PlayerPrefs.GetInt("TauntLevel") == 0 && PlayerPrefs.GetInt("AttackLevel") == 1)
        {
            Debug.Log("Level2");
            PlayerPrefs.SetInt("Attack", 0);
            PlayerPrefs.SetInt("Taunt", 0);
            PlayerPrefs.SetInt("Overreact", 0);
            StartCoroutine("LoadLevel", "TutHubWorld 1");

        }
        else if (PlayerPrefs.GetInt("OverreactLevel") == 0 && PlayerPrefs.GetInt("AttackLevel") == 1)
        {
            Debug.Log("Level3");
            PlayerPrefs.SetInt("Attack", 0);
            PlayerPrefs.SetInt("Taunt", 1);
            PlayerPrefs.SetInt("Overreact", 0);
            StartCoroutine("LoadLevel", "Tutorial_03");

        } else if(PlayerPrefs.GetInt("AttackLevel") == 1 && PlayerPrefs.GetInt("TauntLevel") == 1 && PlayerPrefs.GetInt("OverreactLevel") == 1)
        {
            SceneManager.LoadScene("ProtoHubWorld 1");
        }
    }


    public void Play()
    {
        WwiseInterface.Instance.PlayMenuSound(MenuHandle.PlayButtonPressed);
        HubDataManager.ResetHubData();
    
        PlayerPrefs.SetInt("AttackLevel", 0);
        PlayerPrefs.SetInt("TauntLevel", 0);
        PlayerPrefs.SetInt("OverreactLevel", 0);

        StartCoroutine("LoadLevel", "Introlevel");


    }

    public void Options()
    {
        WwiseInterface.Instance.PlayMenuSound(MenuHandle.ForwardButtonPressed);
        options.SetActive(true);
        menu.SetActive(false);
    }


    IEnumerator LoadLevel(string level)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(level);
        loading.SetActive(true);
        yield return async;
        Debug.Log("Loading complete");
    }

    private void SetVolumes()
    {
        var hasAudio = System.Convert.ToBoolean(PlayerPrefs.GetInt("Sound"));
        float sfxVol, musicVol;
        if (hasAudio) {
            sfxVol = PlayerPrefs.GetFloat("SoundVolume") * 100;
            musicVol = PlayerPrefs.GetFloat("MusicVolume") * 100;
        }
        else {
            sfxVol = 0;
            musicVol = 0;
        }

        WwiseInterface.Instance.SetVolume(sfxVol, VolumeHandle.SFX);
        WwiseInterface.Instance.SetVolume(musicVol, VolumeHandle.Music);
    }

}
