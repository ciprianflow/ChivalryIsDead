using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class endDialog : MonoBehaviour {

    public GameObject UI;
    public GameObject DialogSystem;

	// Use this for initialization
	void Start () {

    }

    // Update is called once per frame
    void Update () {
	
	}

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (UI.GetComponent<GameMenu>().speaking == true)
            {

                UI.GetComponent<GameMenu>().SkipOneBubble();
                if (SceneManager.GetActiveScene().name == "IntroLevel")
                    DialogSystem.GetComponent<IntroLevelDialog>().CallableSkip();
                else if (SceneManager.GetActiveScene().name == "Tutorial_02")
                    DialogSystem.GetComponent<Tutorial_02_Dialog>().CallableSkip();
                else if (SceneManager.GetActiveScene().name == "Tutorial_03")
                    DialogSystem.GetComponent<Tutorial_03_Dialog>().CallableSkip();

            }

            DestoryThis();
        }
        
    }

    void DestoryThis()
    {
        Destroy(gameObject);
    }

}
