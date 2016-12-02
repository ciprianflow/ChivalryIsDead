using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Tutorial_Hub_Dialog : MonoBehaviour {

    public GameObject UI;
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
    float duration;
    public HubDataManager hdManager;

    // Use this for initialization
    void Start()
    {
        //gameMenu = UI.GetComponent<GameMenu>();
        BlackScreenAnimator = blackScreen.GetComponent<Animator>();
        count = 0;
        procceed = false;
        waitforClick = true;
        handAnimator = HandCanvas.GetComponent<Animator>();
        if (SceneManager.GetActiveScene().name == "TutHubWorld 1")
            StartCoroutine("DialogOne");
        else if (SceneManager.GetActiveScene().name == "TutHubWorld 2")
            StartCoroutine("DialogNineAndThreeQuarters");
    }

    // Update is called once per frame
    void Update()
    {
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

    public IEnumerator DialogOne()
    {
        //if(SceneManager.GetActiveScene().name == "TutHubWorld 1")
        //    skipBtn.SetActive(true);
        //yield return new WaitForSeconds(1f);
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 0);

        while (count < 5)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitUntil(SkipAndPlay);

        procceed = false;
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
        yield return new WaitForSeconds(4f);
        StartCoroutine("DialogTwo");

    }

    public void LoadTutorial2()
    {
        handAnimator.SetBool("handHub", false);
        WwiseInterface.Instance.PlayMenuSound(MenuHandle.PlayButtonPressed);
        SceneManager.LoadScene("Tutorial_02");
    }

    public void LoadTutorial3()
    {
        handAnimator.SetBool("handHub", false);
        WwiseInterface.Instance.PlayMenuSound(MenuHandle.PlayButtonPressed);
        SceneManager.LoadScene("Tutorial_03");
    }

    public void removeBubble()
    {
        GameObject swBub = GameObject.FindGameObjectWithTag("SwordBubble");
        if(swBub != null)
        {
            swBub.SetActive(false);
            GameObject.FindGameObjectWithTag("Sword").SetActive(false);
        }

    }


}
