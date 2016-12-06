using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Tutorial_03_Dialog : MonoBehaviour
{
    public GameObject mainCam;
    public GameObject animCam;

    public GameObject Player;
    public GameObject[] Sheeps;
    public GameObject enemy;
    public GameObject UI;
    public GameObject loadingScreen;
    public GameObject ControlMove;
    public GameObject ControlHit;
    public GameObject InvisWallOne;
    bool procceed;

    bool learnedToGetHit;
    bool usedOverreact;
    bool learnedToOverreact;
    bool deadSheep;

    public GameObject HandCanvas;
    public GameObject ScreenFreeze;
    Animator handAnimator;
    public Animator swordAnimator;
    public Animator princessAnimator;
    public Animator skipAnimator;
    public GameObject skipBtn;
    public GameObject tutImage;
    public Animator tutImgAnimator;
    int count;
    // Use this for initialization
    void Start()
    {
       
        count = 0;
        procceed = false;
        learnedToGetHit = true;
        learnedToOverreact = true;
        usedOverreact = true;
        deadSheep = false;
        handAnimator = HandCanvas.GetComponent<Animator>();
        enemy.GetComponent<MeleeAI2>().attackRange = 0;

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

        if(!usedOverreact)
        {
            if (Player.GetComponent<PlayerScript>().overreacting)
            {
                gameObject.GetComponent<DialogObject>().StopDialog();
                OverreactEvent();
                StartCoroutine("DialogFour");
                usedOverreact = true;
            }
           
        }

        if (!learnedToOverreact)
        {
            if (Player.GetComponent<PlayerScript>().overreacting && Player.GetComponent<PlayerActionController>().GetPlayerState() == PlayerState.HIT)
            {
                gameObject.GetComponent<DialogObject>().StopDialog();
                StartCoroutine("DialogFive");
                learnedToOverreact = true;
            }
        }


        int countSheep = 0;
        if (deadSheep)
        {
            foreach (GameObject sheep in Sheeps)
            {
                if (sheep.GetComponent<MonsterAI>().getState() == State.Death)
                    countSheep++;
            }
            if (countSheep == Sheeps.Length)
            {
                StartCoroutine("DialogSix");
                deadSheep = false;
            }
        }

        //if (Sheeps[0].GetComponent<MonsterAI>().getState() == State.Death && Sheeps[1].GetComponent<MonsterAI>().getState() == State.Death && Sheeps[2].GetComponent<MonsterAI>().getState() == State.Death
        //    && Sheeps[3].GetComponent<MonsterAI>().getState() == State.Death && Sheeps[4].GetComponent<MonsterAI>().getState() == State.Death && Sheeps[5].GetComponent<MonsterAI>().getState() == State.Death)
        //{
        //    if (deadSheep)
        //    {
        //        StartCoroutine("DialogSix");
        //        deadSheep = false;
        //    }
        //}



    }

    void OverreactEvent()
    {
        handAnimator.SetBool("playOverreact", false);
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
        animCam.SetActive(true);
        mainCam.SetActive(false);
        animCam.GetComponent<Animator>().SetBool("startAnim", true);

        ControlMove.SetActive(false);
        ControlHit.SetActive(false);

        Time.timeScale = 0.1f;      
        swordAnimator.speed = 10f;
        princessAnimator.speed = 10f;
        skipAnimator.speed = 10f;
        handAnimator.speed = 10f;
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 0);
        Invoke("CallableSkip", 3f);
        //yield return new WaitForSeconds(0.2f);
        //UI.GetComponent<GameMenu>().Sword();


        //Invoke("CallableSkip", 5f);
        while (count < 4)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitUntil(SkipAndPlay);

        //UI.GetComponent<GameMenu>().SkipOneBubble();

        procceed = false;
        ControlMove.SetActive(true);
        ControlHit.SetActive(true);

        Time.timeScale = 1f;
        swordAnimator.speed = 1f;
        princessAnimator.speed = 1f;
        skipAnimator.speed = 1f;
        handAnimator.speed = 1f;

        mainCam.SetActive(true);
        animCam.SetActive(false);
        animCam.GetComponent<Animator>().SetBool("startAnim", false);

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
        //ControlMove.SetActive(false);
        //ControlHit.SetActive(false);
        //Time.timeScale = 0.1f;
        //swordAnimator.speed = 10f;
        //princessAnimator.speed = 10f;
        //skipAnimator.speed = 10f;
        //handAnimator.speed = 10f;
        //yield return new WaitForSeconds(2f);
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 1);
        //yield return new WaitForSeconds(0.2f);
        //UI.GetComponent<GameMenu>().Sword();

        enemy.GetComponent<MeleeAI2>().attackRange = 1.5f;
        learnedToGetHit = false;

        yield return null;



        //count = 0;
        //while (count < 1)
        //{
        //    yield return new WaitForEndOfFrame();
        //}
        //yield return new WaitUntil(SkipAndPlay);
        //procceed = false;

        


        //skipAnimator.speed = 1f;
        //swordAnimator.speed = 1f;
        //princessAnimator.speed = 1f;
        //handAnimator.speed = 1f;
        //Time.timeScale = 1f;
        //ControlMove.SetActive(true);
        //ControlHit.SetActive(true);

    }

    public IEnumerator DialogThree()
    {
        yield return new WaitForSeconds(2f);

        procceed = false;
        ControlMove.SetActive(false);

        Time.timeScale = 0.1f;
        swordAnimator.speed = 10f;
        princessAnimator.speed = 10f;
        skipAnimator.speed = 10f;
        handAnimator.speed = 10f;
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 2);
        tutImage.SetActive(true);
        tutImgAnimator.SetBool("playLearnOverreact", true);
        skipBtn.SetActive(false);
        //yield return new WaitForSeconds(0.2f);
        //UI.GetComponent<GameMenu>().Sword();

        count = 0;
        while (count < 1)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitUntil(SkipAndPlay);
        procceed = false;

        yield return new WaitForSeconds(0.1f);
        ScreenFreeze.SetActive(true);
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 6);
        //yield return new WaitForSeconds(0.2f);
        //UI.GetComponent<GameMenu>().Sword();
        handAnimator.SetBool("playOverreact", true);
        PlayerPrefs.SetInt("Overreact", 1);
        skipBtn.SetActive(false);
        ControlHit.SetActive(true);

        usedOverreact = false;
        //learnedToOverreact = false;
    }

    public IEnumerator DialogFour()
    {
        //ControlMove.SetActive(false);
        //ControlHit.SetActive(false);
        //Time.timeScale = 0.1f;
        //swordAnimator.speed = 10f;
        //princessAnimator.speed = 10f;
        //skipAnimator.speed = 10f;
        //handAnimator.speed = 10f;
        yield return new WaitForSeconds(4f);
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 3);
        learnedToOverreact = false;
        //yield return new WaitForSeconds(0.2f);
        //UI.GetComponent<GameMenu>().Sword();

        yield return null;

        //count = 0;
        //while (count < 1)
        //{
        //    yield return new WaitForEndOfFrame();
        //}
        //yield return new WaitUntil(SkipAndPlay);
        //procceed = false;

        //Time.timeScale = 1f;
        //skipAnimator.speed = 1f;
        //swordAnimator.speed = 1f;
        //princessAnimator.speed = 1f;
        //handAnimator.speed = 1f;
        //ControlMove.SetActive(true);
        //ControlHit.SetActive(true);
    }

    public IEnumerator DialogFive()
    {
        //ControlMove.SetActive(false);
        //ControlHit.SetActive(false);

        //Time.timeScale = 0.1f;
        //swordAnimator.speed = 10f;
        //princessAnimator.speed = 10f;
        //skipAnimator.speed = 10f;
        //handAnimator.speed = 10f;
        yield return new WaitForSeconds(2f);
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 4);

        animCam.SetActive(true);
        mainCam.SetActive(false);
        animCam.GetComponent<Animator>().SetTrigger("zoomInCam");
        InvisWallOne.GetComponent<Animator>().SetTrigger("gateOpen");


        yield return new WaitForSeconds(2.5f);
        mainCam.SetActive(true);
        animCam.SetActive(false);

        //yield return new WaitForSeconds(0.2f);
        //UI.GetComponent<GameMenu>().Sword();

        //count = 0;
        //while (count < 2)
        //{
        //    yield return new WaitForEndOfFrame();
        //}
        //yield return new WaitUntil(SkipAndPlay);
        //procceed = false;

        deadSheep = true;
        //yield return null;
        //yield return new WaitForSeconds(0.1f);
        //ScreenFreeze.SetActive(true);
        //this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 7);
        //yield return new WaitForSeconds(0.2f);
        //UI.GetComponent<GameMenu>().Sword();
        //skipBtn.SetActive(false);
        //ControlHit.SetActive(true);

        //foreach (GameObject Sheep in Sheeps)
        //{
        //    Sheep.GetComponent<SheepAI>().enabled = true;
        //}

    }

    public IEnumerator DialogSix()
    {
        //ControlMove.SetActive(false);
        //ControlHit.SetActive(false);
        //Time.timeScale = 0.1f;
        //swordAnimator.speed = 10f;
        //princessAnimator.speed = 10f;
        //skipAnimator.speed = 10f;
        //handAnimator.speed = 10f;
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 5);
        //yield return new WaitForSeconds(0.2f);
        //UI.GetComponent<GameMenu>().Sword();


        //count = 0;
        //while (count < 2)
        //{
        //    yield return new WaitForEndOfFrame();
        //}
        //yield return new WaitUntil(SkipAndPlay);
        //procceed = false;
        //skipAnimator.speed = 1f;
        //swordAnimator.speed = 1f;
        //princessAnimator.speed = 1f;
        //handAnimator.speed = 1f;
        //Time.timeScale = 1f;
        //ControlMove.SetActive(true);
        //ControlHit.SetActive(true);

        yield return new WaitForSeconds(9f);
        PlayerPrefs.SetInt("OverreactLevel", 1);
        PlayerPrefs.SetInt("Attack", 1);
        loadingScreen.SetActive(true);
        SceneManager.LoadScene("ProtoHubWorld 1");

    }

    public void RestoreAllAnimators()
    {
        swordAnimator.speed = 1f;
        skipAnimator.speed = 1f;
        handAnimator.speed = 1f;
        tutImgAnimator.speed = 1f;
    }
}
