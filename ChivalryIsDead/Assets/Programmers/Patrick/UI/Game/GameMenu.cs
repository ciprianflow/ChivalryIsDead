using UnityEngine;
using System.Collections;

public class GameMenu : MonoBehaviour {

    DialogObject dialogSystem;

    public GameObject pause;
    public GameObject quest;
    public GameObject endLetter;
    public GameObject introLetter;


    public GameObject sword;
    public GameObject swordBubble;

    public GameObject princess;
    public GameObject princessBubble;

    bool paused;
    bool endletterActive;
    bool introletterActive;

    float testRND;


    // Use this for initialization

    void Awake ()
    {
        paused = false;
        endletterActive = false;
        introletterActive = false;

        dialogSystem = GameObject.FindGameObjectWithTag("DialogSystem").GetComponent<DialogObject>();
    } 

    void Start () {

        endLetter.SetActive(false);
        introLetter.SetActive(false);
        sword.SetActive(false);
        princess.SetActive(false);

        Princess();
        Sword();
        Invoke("Test1", 2);

        testRND = Random.Range(16, 30);
        Debug.Log("peasants dialog starts in " + testRND);
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

        // this means quest is over
        if (Input.GetKeyDown(KeyCode.O))
        {
            EndLetter();
          
        }

        // this means start of quest
        if (Input.GetKeyDown(KeyCode.N))
        {
            IntroLetter();
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

    public void EndLetter()
    {
        
        if(!endletterActive)
        {
            endLetter.SetActive(true);
            StartCoroutine(TextLoad(endLetter));
            endletterActive = true;
        }
        else
        {
            endLetter.SetActive(false);
            endletterActive = false;
        }     
    }

    public void IntroLetter()
    {

        if (!introletterActive)
        {
            introLetter.SetActive(true);
            StartCoroutine(TextLoad(introLetter));
            introletterActive = true;
        }
        else
        {
            introLetter.SetActive(false);
            introletterActive = false;
        }
    }

    IEnumerator TextLoad(GameObject letter)
    {
        yield return new WaitForSeconds(0.01f);
        LetterUpdate(letter);
    }

    public void LetterUpdate(GameObject letter)
    {
        letter.GetComponent<TextGeneration>().ClearText();
        letter.GetComponent<TextGeneration>().initTextBags(letter.GetComponent<TextGeneration>().NewBagInitializer);
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
