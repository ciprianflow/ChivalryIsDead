using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class loadScene : MonoBehaviour {

	// Use this for initialization
	void Start () {

        GetComponent<Text>().text = "Current scene - " + SceneManager.GetActiveScene().name;

	}
	
}
