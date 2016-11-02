using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour {

    public GameObject menu;
    public GameObject options;
    public GameObject loading;

    Scene testGame;

	// Use this for initialization
	void Start () {
	
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
        //StartCoroutine("LoadLevel");
        Application.OpenURL("https://www.youtube.com/watch?v=AuRXVMSG3po");
    }


    public void Play()
    {
        //StartCoroutine("LoadLevel");
        Application.OpenURL("https://www.youtube.com/watch?v=AuRXVMSG3po");
    }

    public void Options()
    {
        options.SetActive(true);
        menu.SetActive(false);
    }


    IEnumerator LoadLevel()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync("TestLoad");
        loading.SetActive(true);
        yield return async;
        Debug.Log("Loading complete");
    }

}
