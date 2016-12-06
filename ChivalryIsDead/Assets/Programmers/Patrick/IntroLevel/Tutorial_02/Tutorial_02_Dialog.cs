using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Tutorial_02_Dialog : MonoBehaviour {

    public GameObject animCam;
    public GameObject mainCam;

    public GameObject Player;
    public GameObject enemyBillboard;
    public GameObject TrollA;
    public GameObject TrollB;
    public GameObject[] Sheeps; 

    public GameObject UI;
    public GameObject loadingScreen;

    public GameObject ControlMove;
    public GameObject ControlHit;

    public GameObject InvisWallOne;
    bool procceed;

    bool learnedToGetHit;
    bool learnedToTaunt;
    bool learnedToUseTaunt;
    bool deadSheeps;

    public GameObject HandCanvas;
    public GameObject ScreenFreeze;
    Animator handAnimator;
    public Animator swordAnimator;
    public Animator skipAnimator;
    public GameObject skipBtn;
    int count;

    public GameObject tutImage;
    public Animator tutImgAnimator;

    // Use this for initialization
    void Start()
    {
        PlayerPrefs.SetInt("Attack", 0);
        count = 0;
        procceed = false;
        learnedToGetHit = true;
        learnedToTaunt = true;
        learnedToUseTaunt = true;
        deadSheeps = true;
        handAnimator = HandCanvas.GetComponent<Animator>();

        TrollA.GetComponent<RangedAI>().softAttackRangeBreak = 0;
        TrollA.GetComponent<RangedAI>().attackRange = 0;

        TrollB.GetComponent<RangedAI>().softAttackRangeBreak = 0;
        TrollB.GetComponent<RangedAI>().attackRange = 0;

        foreach (GameObject Sheep in Sheeps)
        {
           // Sheep.GetComponent<SheepAI>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!learnedToGetHit)
        {
            if(Player.GetComponent<PlayerActionController>().GetPlayerState() == PlayerState.HIT)
            {
                gameObject.GetComponent<DialogObject>().StopDialog();
                StartCoroutine("DialogFour");

                
                

                learnedToGetHit = true;
            }
            
        }

        if (!learnedToTaunt)
        {
            if (Player.GetComponent<PlayerScript>().taunting)
            {
                gameObject.GetComponent<DialogObject>().StopDialog();
                TauntEvent();
                learnedToTaunt = true;
                count = 1;
                TrollB.GetComponent<RangedAI>().softAttackRangeBreak = 12;
                TrollB.GetComponent<RangedAI>().attackRange = 10;
            }
        }

        

        if (!learnedToUseTaunt)
        {
            if (Player.GetComponent<PlayerActionController>().GetPlayerState() == PlayerState.HIT)
            {
                
                learnedToUseTaunt = true;
                handAnimator.speed = 1f;
                swordAnimator.speed = 1f;
                skipAnimator.speed = 1f;
                Time.timeScale = 1f;
                StartCoroutine("DialogSix");
            }
        }
        int countSheep = 0;
        if (deadSheeps)
        {
            foreach (GameObject sheep in Sheeps)
            {
                if (sheep.GetComponent<MonsterAI>().getState() == State.Death)
                    countSheep++;
            }
            if(countSheep == Sheeps.Length)
            {
                StartCoroutine("DialogSeven");
                deadSheeps = false;
            }
        }
        //if (Sheeps[0].GetComponent<MonsterAI>().getState() == State.Death && Sheeps[1].GetComponent<MonsterAI>().getState() == State.Death && Sheeps[2].GetComponent<MonsterAI>().getState() == State.Death)
        //{
        //    if(deadSheeps)
        //    {
        //        StartCoroutine("DialogSeven");
        //        deadSheeps = false;
        //    }
        //}


    }

    void TauntEvent()
    {
        handAnimator.SetBool("playTaunt", false);
        handAnimator.speed = 100f;
        swordAnimator.speed = 1f;
        skipAnimator.speed = 1f;
        Time.timeScale = 1f;
        ScreenFreeze.SetActive(false);
        ControlMove.SetActive(true);
        //StartCoroutine("DialogSix");
    }

    public IEnumerator DialogOne()
    {
        //yield return new WaitForSeconds(animCam.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        animCam.GetComponent<Animator>().SetBool("startAnim", true);
        animCam.SetActive(true);
        mainCam.SetActive(false);
        ControlMove.SetActive(false);
        ControlHit.SetActive(false);       
        //yield return new WaitForSeconds(1f);
        Invoke("SkipCam", 13f);

        //Time.timeScale = 0.1f;
        //swordAnimator.speed = 10f;
        //skipAnimator.speed = 10f;
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 0);
        //yield return new WaitForSeconds(0.2f);
        //UI.GetComponent<GameMenu>().Sword();


        count = 0;
        Invoke("SkipCam", 7.25f);
        
        while (count < 1)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitUntil(SkipAndPlay);

        procceed = false;
        ControlMove.SetActive(true);
        mainCam.SetActive(true);
        animCam.SetActive(false);
        animCam.GetComponent<Animator>().SetBool("startAnim", false);



        Time.timeScale = 1f;
        swordAnimator.speed = 1f;
        skipAnimator.speed = 1f;


    }

    void SkipCam ()
    {
        count = 1;
        CallableSkip();
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

    public IEnumerator DialogEight()
    {
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 8);
        //yield return new WaitForSeconds(0.2f);
        //UI.GetComponent<GameMenu>().Sword();
  
        count = 0;
        while (count < 1)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitUntil(SkipAndPlay);
        procceed = false;
    }

    public IEnumerator DialogTwo()
    {
        ControlMove.SetActive(false);
        Time.timeScale = 0.1f;
        swordAnimator.speed = 10f;
        skipAnimator.speed = 10f;
        //enemyBillboard.GetComponent<CameraBillboard>().speaker = TrollA;
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 1);
        //yield return new WaitForSeconds(0.2f);
        //UI.GetComponent<GameMenu>().Sword();
        tutImage.SetActive(true);
        tutImgAnimator.SetBool("playGetHit", true);
        skipBtn.SetActive(false);

        count = 0;
        while (count < 1)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitUntil(SkipAndPlay);
        procceed = false;
        skipAnimator.speed = 1f;
        swordAnimator.speed = 1f;
        Time.timeScale = 1f;
        ControlMove.SetActive(true);

        StartCoroutine("DialogThree");
    }

    public IEnumerator DialogThree()
    {
        tutImage.SetActive(false);
        yield return new WaitForSeconds(2f);
        //ControlMove.SetActive(false);

        //Time.timeScale = 0.1f;
        //swordAnimator.speed = 10f;
        //skipAnimator.speed = 10f;
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 2);

        TrollA.GetComponent<RangedAI>().softAttackRangeBreak = 12;
        TrollA.GetComponent<RangedAI>().attackRange = 10;

        //yield return new WaitForSeconds(0.2f);
        //UI.GetComponent<GameMenu>().Sword();
        //count = 0;
        //while (count < 1)
        //{
        //    yield return new WaitForEndOfFrame();
        //}
        //yield return new WaitUntil(SkipAndPlay);
        //procceed = false;
        //skipAnimator.speed = 1f;
        //swordAnimator.speed = 1f;
        //Time.timeScale = 1f;
        //ControlMove.SetActive(true);
        learnedToGetHit = false;
        yield return null;
    }


    public IEnumerator DialogFour()
    {
        ControlMove.SetActive(false);
        ControlHit.SetActive(false);

        //Time.timeScale = 0.1f;
        //swordAnimator.speed = 10f;
        //skipAnimator.speed = 10f;
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 3);
        //yield return new WaitForSeconds(0.2f);
        //UI.GetComponent<GameMenu>().Sword();

        // GATE!
        yield return new WaitForSeconds(1f);
        animCam.SetActive(true);
        mainCam.SetActive(false);
        animCam.GetComponent<Animator>().SetTrigger("zoomInCam");
        InvisWallOne.GetComponent<Animator>().SetTrigger("gateOpen");
        

        yield return new WaitForSeconds(2.5f);
        mainCam.SetActive(true);
        animCam.SetActive(false);
        ControlMove.SetActive(true);
        ControlHit.SetActive(true);

        /*
        count = 0;
        while (count < 1)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitUntil(SkipAndPlay);
        procceed = false;
        //skipAnimator.speed = 1f;
        //swordAnimator.speed = 1f;
        //Time.timeScale = 1f;
        */

        yield return null;
        
    }

    public IEnumerator DialogFive()
    {
        ControlMove.SetActive(false);

        Time.timeScale = 0.1f;
        swordAnimator.speed = 10f;
        skipAnimator.speed = 10f;
        handAnimator.speed = 10f;

        yield return new WaitForSeconds(0.1f);
        ScreenFreeze.SetActive(true);
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 7);
        //yield return new WaitForSeconds(0.2f);
        //UI.GetComponent<GameMenu>().Sword();
        skipBtn.SetActive(false);
        handAnimator.SetBool("playTaunt", true);
        PlayerPrefs.SetInt("Taunt", 1);
        ControlHit.SetActive(true);
        count = 0;
        learnedToTaunt = false;
        while (count < 1)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitUntil(SkipAndPlay);
        procceed = false;



        //enemyBillboard.GetComponent<CameraBillboard>().speaker = TrollB;
        yield return new WaitForSeconds(1f);

        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 4);
        InvisWallOne.GetComponent<Animator>().SetTrigger("gateClose");

        Time.timeScale = 0.1f;
        swordAnimator.speed = 10f;
        skipAnimator.speed = 10f;
        handAnimator.speed = 10f;

        tutImage.SetActive(true);
        tutImgAnimator.SetBool("playLearnTaunt", true);
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

        learnedToUseTaunt = false;
        //yield return new WaitForSeconds(0.1f);
        //ScreenFreeze.SetActive(true);
        //handAnimator.speed = 10f;
        //this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 7);
        ////yield return new WaitForSeconds(0.2f);
        ////UI.GetComponent<GameMenu>().Sword();
        //skipBtn.SetActive(false);
        //handAnimator.SetBool("playTaunt", true);
        //PlayerPrefs.SetInt("Taunt", 1);
        //ControlHit.SetActive(true);

        //learnedToTaunt = false;
    }

    public IEnumerator DialogSix()
    {
        yield return new WaitForSeconds(1f);
        //ControlMove.SetActive(false);

        //Time.timeScale = 0.1f;
        //swordAnimator.speed = 10f;
        //skipAnimator.speed = 10f;
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
        //skipAnimator.speed = 1f;
        //swordAnimator.speed = 1f;
        //Time.timeScale = 1f;
        //ControlMove.SetActive(true);


        //foreach (GameObject Sheep in Sheeps)
        //{
        //    Sheep.GetComponent<SheepAI>().enabled = true;
        //}

        //StartCoroutine("DialogSeven");
        
    }

   


    public IEnumerator DialogSeven()
    {

        //yield return new WaitForSeconds(10f);

        //ControlMove.SetActive(false);

        //Time.timeScale = 0.1f;
        //swordAnimator.speed = 10f;
        //skipAnimator.speed = 10f;
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 6);
        //yield return new WaitForSeconds(0.2f);
        //UI.GetComponent<GameMenu>().Sword();

        //count = 0;
        //while (count < 1)
        //{
        //    yield return new WaitForEndOfFrame();
        //}
        //yield return new WaitUntil(SkipAndPlay);
        //procceed = false;
        //skipAnimator.speed = 1f;
        //swordAnimator.speed = 1f;
        //Time.timeScale = 1f;


        yield return new WaitForSeconds(6f);
        PlayerPrefs.SetInt("TauntLevel", 1);
        loadingScreen.SetActive(true);
        SceneManager.LoadScene("TutHubWorld 2");

    }

    public void RestoreAllAnimators()
    {
        swordAnimator.speed = 1f;
        skipAnimator.speed = 1f;
        handAnimator.speed = 1f;
        tutImgAnimator.speed = 1f;
    }
   

}
