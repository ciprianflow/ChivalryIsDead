using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class CinematicTextEvents : MonoBehaviour {

    public Renderer textMat;
    public List<Texture> speechBubbles_English;
    public List<Texture> speechBubbles_Danish;
    public GameObject firstScene;

    int curTex = -1;
	// Use this for initialization
	void Start () {
        //textMat.material.SetTexture("_MainTex", tex);

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void nextText() {
        curTex++;
        if(PlayerPrefs.GetString("Language") == "English")
            textMat.material.SetTexture("_MainTex", speechBubbles_English[curTex]);
        else
            textMat.material.SetTexture("_MainTex", speechBubbles_Danish[curTex]);
    }

    public void nextScene() {
        SceneManager.LoadScene(3);
    }

    public void halfScene() {
        firstScene.SetActive(false);
    }
}
