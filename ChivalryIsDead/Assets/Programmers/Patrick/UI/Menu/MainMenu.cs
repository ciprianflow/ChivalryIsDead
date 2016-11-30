using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour {

    public GameObject menu;
    public GameObject options;
    public GameObject loading;

    Scene testGame;

	// Use this for initialization
	void Start ()
    {
        WwiseInterface.Instance.SetMusic(MusicHandle.MusicStop);
        WwiseInterface.Instance.SetMusic(MusicHandle.MusicOnePlay);
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }


    public void Continue()
    {
        WwiseInterface.Instance.PlayMenuSound(MenuHandle.PlayButtonPressed);
        //Application.OpenURL("https://www.youtube.com/watch?v=AuRXVMSG3po");

        SceneManager.LoadScene("ProtoHubWorld 1");
    }


    public void Play()
    {
        WwiseInterface.Instance.PlayMenuSound(MenuHandle.PlayButtonPressed);
        HubDataManager.ResetHubData();
        StartCoroutine("LoadLevel");
        
    }

    public void Options()
    {
        WwiseInterface.Instance.PlayMenuSound(MenuHandle.ForwardButtonPressed);
        options.SetActive(true);
        menu.SetActive(false);
    }


    IEnumerator LoadLevel()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(1);
        loading.SetActive(true);
        yield return async;
        Debug.Log("Loading complete");
    }

}
