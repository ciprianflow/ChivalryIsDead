using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour {

    DialogObject dialogSystem;

    SettingsMngr settingManager;
    public GameObject pause;
    public GameObject muteSound;
    public GameObject soundVolume;

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

    public bool speaking;

    float testRND;

    public GameObject skipBtn;
    GameObject pauseBtn;

    private bool tauntCD = true;

    // Use this for initialization

    void Awake ()
    {
        //speaking = false;
        paused = false;
        endletterActive = false;
        introletterActive = false;

        dialogSystem = GameObject.FindGameObjectWithTag("DialogSystem").GetComponent<DialogObject>();
        settingManager = GameObject.FindGameObjectWithTag("SettingsManager").GetComponent<SettingsMngr>();

        StaticIngameData.gameMenu = this;
    } 

    void Start () {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        // Start the background music.
        WwiseInterface.Instance.SetMusic(MusicHandle.MusicOnePlay);


        endLetter.SetActive(false);
        introLetter.SetActive(false);
        //sword.SetActive(false);
        princess.SetActive(false);
        //if(SceneManager.GetActiveScene().name != "TutHubWorld 1" && SceneManager.GetActiveScene().name != "TutHubWorld 2")
        //    skipBtn.SetActive(false);

        //Princess();
        //Sword();
        //Invoke("Test1", 2);

        //testRND = Random.Range(16, 30);
        //Debug.Log("peasants dialog starts in " + testRND);
        //Invoke("Test", testRND);

        //skipBtn = GameObject.FindGameObjectWithTag("SkipBtn");
        pauseBtn = GameObject.FindGameObjectWithTag("PauseBtn");


        // Check Volume Slider
        soundVolume.GetComponent<Slider>().value = PlayerPrefs.GetFloat("SoundVolume");

        // Check Mute

        if (PlayerPrefs.GetInt("Sound") == 0)
        {
            muteSound.GetComponent<Image>().color = Color.red;
        }
        else if (PlayerPrefs.GetInt("Sound") == 1)
        {
            muteSound.GetComponent<Image>().color = Color.white;

        }


    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Y))
        {
            Princess();
            Sword();
            dialogSystem.StartCoroutine("DialogSystem", 0);
            //Invoke("Test1", 2);
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
        

        if (!paused)
        {
            WwiseInterface.Instance.PlayMenuSound(MenuHandle.ForwardButtonPressed);
            pause.SetActive(true);
            paused = true;
            //pauseBtn.GetComponent<Image>().sprite = pauseBtn.GetComponent<Button>().spriteState.pressedSprite;
            //pauseBtn.SetActive(false);
            Time.timeScale = 0f;
        } else
        {
            WwiseInterface.Instance.PlayMenuSound(MenuHandle.BackwardsButtonPressed);
            pause.SetActive(false);
            paused = false;
            //pauseBtn.SetActive(true);
            //pauseBtn.GetComponent<Image>().sprite = pauseBtn.GetComponent<Button>().spriteState.disabledSprite;
            Time.timeScale = 1f;
        }
    }


    public void TauntCooldown()
    {
        PlayerActionController pAction = GameObject.Find("Player").GetComponent<PlayerActionController>();
        if(!tauntCD)
        {
            tauntCD = true;
            pAction.SetTauntCooldown(5f);
           // pAction.taun
            Debug.Log(" TRUE");

        } 
        else
        {
            tauntCD = false;
            pAction.SetTauntCooldown(0f);
           // pAction.TauntCooldown = 0f;
            Debug.Log(" FALSe");
        }
    }

    public void Quest()
    {
        quest.SetActive(true);
    }

    public void EndLetter()
    {
        /*
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
        }   */  
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
        introLetter.GetComponent<TextGeneration>().CallTxtChooserStartQuest();
        endLetter.GetComponent<TextGeneration>().initTextBags(letter.GetComponent<TextGeneration>().NewBagInitializer);
    }

    public void SkipOneBubble()
    {
        dialogSystem.SkipDialog();
        skipBtn.SetActive(false);
    }


    public void Princess()
    {
        skipBtn.SetActive(true);
        princess.SetActive(true);
    }

    public void Sword()
    {
        skipBtn.SetActive(true);
        sword.SetActive(true);
        
    }




    //Options

    public void SoundVolume()
    {
        //WwiseInterface.Instance.PlayMenuSound(MenuHandle.ForwardButtonPressed);
        PlayerPrefs.SetFloat("SoundVolume", soundVolume.GetComponent<Slider>().value);
    }

    public void SoundMute()
    {
        

        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            PlayerPrefs.SetInt("Sound", 0);
            Debug.Log("Off");
        }
        else if (PlayerPrefs.GetInt("Sound") == 0)
        {
            PlayerPrefs.SetInt("Sound", 1);
            Debug.Log("On");
        }

        if (PlayerPrefs.GetInt("Sound") == 0)
        {
            WwiseInterface.Instance.PlayMenuSound(MenuHandle.BackwardsButtonPressed);
            muteSound.GetComponent<Image>().color = Color.red;
        }
        else if (PlayerPrefs.GetInt("Sound") == 1)
        {
            WwiseInterface.Instance.PlayMenuSound(MenuHandle.ForwardButtonPressed);
            muteSound.GetComponent<Image>().color = Color.white;

        }
    }

    public void SwapControl()
    {
        WwiseInterface.Instance.PlayMenuSound(MenuHandle.ForwardButtonPressed);
        settingManager.swapSides();
    }

    public void MainMenu()
    {
        WwiseInterface.Instance.PlayMenuSound(MenuHandle.ForwardButtonPressed);
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }


}
