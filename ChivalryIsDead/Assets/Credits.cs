﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Invoke("Menu", 25);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    void Menu()
    {
        SceneManager.LoadScene(0);
    }
}
