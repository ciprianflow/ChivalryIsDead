using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Tutorial_03_Dialog : MonoBehaviour
{
    public GameObject Player;
    public GameObject[] Sheeps;
    public GameObject UI;
    public GameObject ControlMove;
    public GameObject ControlHit;
    public GameObject InvisWallOne;
    public GameObject InvisWallTwo;
    bool procceed;

    bool learnedToGetHit;
    bool learnedToPlaceYourself;
    bool learnedToOverreact;
    bool deadSheep;

    public GameObject HandCanvas;
    public GameObject ScreenFreeze;
    Animator handAnimator;
    public Animator swordAnimator;
    public Animator princessAnimator;
    public Animator skipAnimator;
    public GameObject skipBtn;
    int count;
    // Use this for initialization
    void Start()
    {
        count = 0;
        procceed = false;
        learnedToGetHit = true;
        learnedToPlaceYourself = true;
        learnedToOverreact = true;
        deadSheep = false;
        handAnimator = HandCanvas.GetComponent<Animator>();

        foreach (GameObject Sheep in Sheeps)
        {
            //Sheep.GetComponent<SheepAI>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!learnedToGetHit)
        {
            if (Player.GetComponent<PlayerActionController>().GetPlayerState() == PlayerState.HIT)
            {
                learnedToGetHit = true;
                StartCoroutine("DialogThree");
            }

        }

        if (!learnedToOverreact)
        {
            if (Player.GetComponent<PlayerScript>().overreacting)
            {
                gameObject.GetComponent<DialogObject>().StopDialog();
                OverreactEvent();
                learnedToOverreact = true;
                StartCoroutine("DialogFour");
            }
        }
        if (!learnedToPlaceYourself)
        {
            if (Player.GetComponent<PlayerScript>().taunting)
            {
                gameObject.GetComponent<DialogObject>().StopDialog();
                OverreactEvent();
                learnedToPlaceYourself = true;
            }

        }

        
        if (Sheeps[0].GetComponent<MonsterAI>().getState() == State.Death && Sheeps[1].GetComponent<MonsterAI>().getState() == State.Death)
        {
            if (deadSheep)
            {
                StartCoroutine("DialogSix");
                deadSheep = false;
            }
        }

        

    }

    void OverreactEvent()
    {
        handAnimator.SetBool("playOverreact", false);
        handAnimator.SetBool("playTaunt", false);
        Time.timeScale = 1f;
        handAnimator.speed = 1f;
        swordAnimator.speed = 1f;
        princessAnimator.speed = 1f;
        skipAnimator.speed = 1f;
        ScreenFreeze.SetActive(false);
        ControlMove.SetActive(true);
    }

    public IEnumerator DialogOne()
    {
        ControlMove.SetActive(false);
        ControlHit.SetActive(false);

        Time.timeScale = 0.1f;
        swordAnimator.speed = 10f;
        princessAnimator.speed = 10f;
        skipAnimator.speed = 10f;
        handAnimator.speed = 10f;
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 0);
        //yield return new WaitForSeconds(0.2f);
        //UI.GetComponent<GameMenu>().Sword();


        //Invoke("CallableSkip", 5f);
        while (count < 1)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitUntil(SkipAndPlay);

        procceed = false;
        ControlMove.SetActive(true);
        ControlHit.SetActive(true);

        Time.timeScale = 1f;
        swordAnimator.speed = 1f;
        princessAnimator.speed = 1f;
        skipAnimator.speed = 1f;
        handAnimator.speed = 1f;

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
        ControlMove.SetActive(false);
        ControlHit.SetActive(false);
        Time.timeScale = 0.1f;
        swordAnimator.speed = 10f;
        princessAnimator.speed = 10f;
        skipAnimator.speed = 10f;
        handAnimator.speed = 10f;
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 1);
        //yield return new WaitForSeconds(0.2f);
        //UI.GetComponent<GameMenu>().Sword();


        count = 0;
        while (count < 1)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitUntil(SkipAndPlay);
        procceed = false;
        skipAnimator.speed = 1f;
        swordAnimator.speed = 1f;
        princessAnimator.speed = 1f;
        handAnimator.speed = 1f;
        Time.timeScale = 1f;
        ControlMove.SetActive(true);
        ControlHit.SetActive(true);
        learnedToGetHit = false;
    }

    public IEnumerator DialogThree()
    {
        ControlMove.SetActive(false);

        Time.timeScale = 0.1f;
        swordAnimator.speed = 10f;
        princessAnimator.speed = 10f;
        skipAnimator.speed = 10f;
        handAnimator.speed = 10f;
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 2);
        //yield return new WaitForSeconds(0.2f);
        //UI.GetComponent<GameMenu>().Sword();

        count = 0;
        while (count < 1)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitUntil(SkipAndPlay);
        procceed = false;

        ScreenFreeze.SetActive(true);
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 6);
        //yield return new WaitForSeconds(0.2f);
        //UI.GetComponent<GameMenu>().Sword();
        handAnimator.SetBool("playOverreact", true);
        skipBtn.SetActive(false);
        ControlHit.SetActive(true);

        learnedToOverreact = false;
    }

    public IEnumerator DialogFour()
    {
        ControlMove.SetActive(false);
        ControlHit.SetActive(false);
        Time.timeScale = 0.1f;
        swordAnimator.speed = 10f;
        princessAnimator.speed = 10f;
        skipAnimator.speed = 10f;
        handAnimator.speed = 10f;
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 3);
        //yield return new WaitForSeconds(0.2f);
        //UI.GetComponent<GameMenu>().Sword();


        count = 0;
        while (count < 1)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitUntil(SkipAndPlay);
        procceed = false;
       
        Time.timeScale = 1f;
        skipAnimator.speed = 1f;
        swordAnimator.speed = 1f;
        princessAnimator.speed = 1f;
        handAnimator.speed = 1f;
        ControlMove.SetActive(true);
        ControlHit.SetActive(true);

    }

    public IEnumerator DialogFive()
    {
        ControlMove.SetActive(false);
        ControlHit.SetActive(false);

        Time.timeScale = 0.1f;
        swordAnimator.speed = 10f;
        princessAnimator.speed = 10f;
        skipAnimator.speed = 10f;
        handAnimator.speed = 10f;
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 4);
        //yield return new WaitForSeconds(0.2f);
        //UI.GetComponent<GameMenu>().Sword();

        count = 0;
        while (count < 1)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitUntil(SkipAndPlay);
        procceed = false;

        ScreenFreeze.SetActive(true);
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 7);
        //yield return new WaitForSeconds(0.2f);
        //UI.GetComponent<GameMenu>().Sword();
        handAnimator.SetBool("playTaunt", true);
        skipBtn.SetActive(false);
        ControlHit.SetActive(true);

        foreach (GameObject Sheep in Sheeps)
        {
            Sheep.GetComponent<SheepAI>().enabled = true;
        }
        deadSheep = true;
        learnedToPlaceYourself = false;
    }

    public IEnumerator DialogSix()
    {
        ControlMove.SetActive(false);
        ControlHit.SetActive(false);
        Time.timeScale = 0.1f;
        swordAnimator.speed = 10f;
        princessAnimator.speed = 10f;
        skipAnimator.speed = 10f;
        handAnimator.speed = 10f;
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 5);
        //yield return new WaitForSeconds(0.2f);
        //UI.GetComponent<GameMenu>().Sword();


        count = 0;
        while (count < 1)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitUntil(SkipAndPlay);
        procceed = false;
        skipAnimator.speed = 1f;
        swordAnimator.speed = 1f;
        princessAnimator.speed = 1f;
        handAnimator.speed = 1f;
        Time.timeScale = 1f;
        ControlMove.SetActive(true);
        ControlHit.SetActive(true);

        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(6);

    }
}
