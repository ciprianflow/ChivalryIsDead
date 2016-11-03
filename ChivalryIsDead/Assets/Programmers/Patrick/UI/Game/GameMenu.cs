using UnityEngine;
using System.Collections;

public class GameMenu : MonoBehaviour {

    DialogObject dialogSystem;

    public GameObject pause;
    public GameObject quest;
    public GameObject letter;

    public GameObject sword;
    public GameObject swordBubble;

    public GameObject princess;
    public GameObject princessBubble;

    bool paused;

    // Use this for initialization

    void Awake ()
    {
        paused = false;
        dialogSystem = GameObject.FindGameObjectWithTag("DialogSystem").GetComponent<DialogObject>();
    } 

    void Start () {

        Invoke("Princess", 2);

    }
	
	// Update is called once per frame
	void Update () {
	
        if(Input.GetKeyDown(KeyCode.Y))
        {
            Invoke("Princess", 2);
        }

	}

    public void Pause()
    {
        if(!paused)
        {
            pause.SetActive(true);
            paused = true;
            Time.timeScale = 0f;
        } else
        {
            pause.SetActive(false);
            paused = false;
            Time.timeScale = 1f;
        }
    }

    public void Quest()
    {
        quest.SetActive(true);
    }

    public void Letter()
    {
        letter.SetActive(true);
    }

    public void Princess()
    {
        princess.SetActive(true);
        dialogSystem.StartCoroutine("DialogSystem", 0);
    }

    public void Sword()
    {
        sword.SetActive(true);
    }


}
