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
    bool letterActive;

    float testRND;


    // Use this for initialization

    void Awake ()
    {
        paused = false;
        letterActive = false;
        dialogSystem = GameObject.FindGameObjectWithTag("DialogSystem").GetComponent<DialogObject>();
    } 

    void Start () {

        letter.SetActive(false);
        sword.SetActive(false);
        princess.SetActive(false);

        Princess();
        Sword();
        Invoke("Test1", 2);

        testRND = Random.Range(16, 30);
        Debug.Log(testRND);
        Invoke("Test", testRND);

       

    }
	
	// Update is called once per frame
	void Update () {
        
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Princess();
            Sword();
            Invoke("Test1", 2);
        }


        if (Input.GetKeyDown(KeyCode.O))
        {
            Letter();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            LetterUpdate();
        }


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            dialogSystem.StartCoroutine("DialogSystem", 1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            dialogSystem.StartCoroutine("DialogSystem", 2);
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
        if(!letterActive)
        {
            letter.SetActive(true);
            StartCoroutine(TextLoad());
            letterActive = true;
        } else
        {
            letter.SetActive(false);
            letterActive = false;   
        }     
    }

    public void LetterUpdate()
    {
        letter.GetComponent<TextGeneration>().ClearText();
        letter.GetComponent<TextGeneration>().initTextBags(letter.GetComponent<TextGeneration>().NewBagInitializer);
    }

    IEnumerator TextLoad()
    {
        yield return new WaitForSeconds(0.01f);
        LetterUpdate();
    }

    public void Princess()
    {
        princess.SetActive(true);
    }

    public void Sword()
    {
        sword.SetActive(true);
    }


    void Test()
    {
        dialogSystem.StartCoroutine("DialogSystem", 2);
    }

    void Test1()
    {
        dialogSystem.StartCoroutine("DialogSystem", 0);
    }

}
