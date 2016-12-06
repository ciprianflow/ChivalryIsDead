using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Tutorial_Hub_Dialog : MonoBehaviour {

    public GameObject UI;
    public GameObject loadingScreen;
    //GameMenu gameMenu;

    bool procceed;

    public GameObject HandCanvas;
    public GameObject ScreenFreeze;
    Animator handAnimator;
    public Animator swordAnimator;
    public Animator skipAnimator;
    public GameObject skipBtn;
    int count;
    bool waitforClick;

    Animator BlackScreenAnimator;
    public GameObject blackScreen;
    public GameObject endCinematic;
    bool hideButton;
    float duration;
    public HubDataManager hdManager;

    public GameObject dampenLightPanel;

    // Use this for initialization
    void Start()
    {
        //gameMenu = UI.GetComponent<GameMenu>();
        BlackScreenAnimator = blackScreen.GetComponent<Animator>();
        BlackScreenAnimator.SetTrigger("fadeOut");
        count = 0;
        procceed = false;
        hideButton = false;
        waitforClick = true;
        handAnimator = HandCanvas.GetComponent<Animator>();
        if (SceneManager.GetActiveScene().name == "TutHubWorld 1")
            StartCoroutine("DialogOne");
        else if (SceneManager.GetActiveScene().name == "TutHubWorld 2")
        {
            endCinematic.SetActive(false);
            StartCoroutine("DialogNineAndThreeQuarters");
        }
            
    }

    // Update is called once per frame
    void Update()
    {

        if(hideButton)
        {
            skipBtn.SetActive(false);
        }
        //if (!waitforClick)
        //{
        //    if (hdManager.isClicked)
        //    {
        //        gameObject.GetComponent<DialogObject>().StopDialog();
        //        StartCoroutine("DialogThree");
        //        waitforClick = true;
        //        hdManager.isClicked = false;
        //    }
        //}
    }

    public IEnumerator DialogIntro()
    {
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 5);
        yield return new WaitForSeconds(5.5f);
        StartCoroutine("DialogOne");
    }


    public IEnumerator DialogOne()
    {
        //if(SceneManager.GetActiveScene().name == "TutHubWorld 1")
        //    skipBtn.SetActive(true);
        yield return new WaitForSeconds(5.5f);
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 0);
        skipBtn.SetActive(false);

        Invoke("CallableSkip", 21f);
        hideButton = true;

        //while (count < 5)
        //{
        //    skipBtn.SetActive(false);
        //    yield return new WaitForEndOfFrame();
        //}
        yield return new WaitUntil(SkipAndPlay);

        hideButton = false;
        procceed = false;

        BlackScreenAnimator.SetTrigger("fadeIn");
        yield return new WaitForSeconds(5f);
        blackScreen.SetActive(false);
        endCinematic.SetActive(false);

        blackScreen.SetActive(true);


        BlackScreenAnimator.SetTrigger("fadeOut");
        //duration = BlackScreenAnimator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(4f);
        StartCoroutine("DialogTwo");

    }

    public bool SkipAndPlay()
    {
        return procceed;
    }

    public void CallableSkip()
    {
        procceed = true;
        count++;
    }

    public IEnumerator DialogTwo()
    {
        //yield return new WaitForSeconds(1f);
        blackScreen.SetActive(false);
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 1);
        handAnimator.SetBool("playPeasant", true);
        count = 0;
        while (count < 3)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitUntil(SkipAndPlay);

        procceed = false;
        //waitforClick = false;
        //StartCoroutine("DialogThree");
    }

    public IEnumerator DialogThree()
    {
        yield return new WaitForSeconds(1f);
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 2);
        skipBtn.SetActive(false);

        //count = 0;
        //while (count < 1)
        //{
        //    yield return new WaitForEndOfFrame();
        //}
        //yield return new WaitUntil(SkipAndPlay);
        procceed = false;
        //handAnimator.SetBool("handHub", true);
  
    }

    public IEnumerator DialogNineAndThreeQuarters()
    {
        //if (SceneManager.GetActiveScene().name == "TutHubWorld 2")
        //{
        //    //UI.GetComponent<GameMenu>().Sword();
        //    skipBtn.SetActive(true);
        //}
          
        //yield return new WaitForSeconds(1f);
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 3);

        while (count < 3)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitUntil(SkipAndPlay);

        procceed = false;
        BlackScreenAnimator.SetTrigger("fadeOut");
        //duration = BlackScreenAnimator.GetCurrentAnimatorStateInfo(0).length;
        //yield return new WaitForSeconds(4f);
        //StartCoroutine("DialogTwo");
        yield return new WaitForSeconds(4f);
        blackScreen.SetActive(false);
    }

    public void LoadTutorial2()
    {
        handAnimator.SetBool("handHub", false);
        WwiseInterface.Instance.PlayMenuSound(MenuHandle.PlayButtonPressed);
        loadingScreen.SetActive(true);
        SceneManager.LoadScene("Tutorial_02");
    }

    public void LoadTutorial3()
    {
        handAnimator.SetBool("handHub", false);
        WwiseInterface.Instance.PlayMenuSound(MenuHandle.PlayButtonPressed);
        loadingScreen.SetActive(true);
        SceneManager.LoadScene("Tutorial_03");
    }

    public void removeBubble()
    {
        gameObject.GetComponent<DialogObject>().StopDialog();
        //GameObject swBub = GameObject.FindGameObjectWithTag("SwordBubble");
        //if(swBub != null)
        //{
        //    swBub.SetActive(false);
        //    GameObject.FindGameObjectWithTag("Sword").SetActive(false);
        //}

    }

    public void CoolDampenLight()
    {
        skipBtn.SetActive(false);
        dampenLightPanel.SetActive(true);
    }


}
