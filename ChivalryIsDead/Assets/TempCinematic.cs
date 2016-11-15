using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TempCinematic : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Invoke("nextScene", 3);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void nextScene()
    {
        SceneManager.LoadScene(3);
    }

}
