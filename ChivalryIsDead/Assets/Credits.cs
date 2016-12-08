using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Invoke("Menu", 28); // Angel said this :)
       
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    
    public void Menu()
    {
        SceneManager.LoadScene(0);
    }
}
