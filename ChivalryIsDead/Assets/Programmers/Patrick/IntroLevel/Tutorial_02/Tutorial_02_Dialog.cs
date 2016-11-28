using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Tutorial_02_Dialog : MonoBehaviour {

    public GameObject Player;
    public GameObject enemyBillboard;
    public GameObject TrollA;
    public GameObject TrollB;
    public GameObject[] Sheeps; 

    public GameObject UI;
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
        count = 0;
        procceed = false;
        learnedToGetHit = true;
        learnedToTaunt = true;
        learnedToUseTaunt = true;
        deadSheeps = true;
        handAnimator = HandCanvas.GetComponent<Animator>();

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

                
                TrollA.GetComponent<RangedAI>().softAttackRangeBreak = 0;
                TrollA.GetComponent<RangedAI>().attackRange = 0;

                TrollB.GetComponent<RangedAI>().softAttackRangeBreak = 0;
                TrollB.GetComponent<RangedAI>().attackRange = 0;

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

                TrollB.GetComponent<RangedAI>().softAttackRangeBreak = 12;
                TrollB.GetComponent<RangedAI>().attackRange = 10;
            }
        }

        

        if (!learnedToUseTaunt)
        {
            if (Player.GetComponent<PlayerActionController>().GetPlayerState() == PlayerState.HIT)
            {
                
                learnedToUseTaunt = true;
            }
        }

        if (Sheeps[0].GetComponent<MonsterAI>().getState() == State.Death && Sheeps[1].GetComponent<MonsterAI>().getState() == State.Death && Sheeps[2].GetComponent<MonsterAI>().getState() == State.Death)
        {
            if(deadSheeps)
            {
                StartCoroutine("DialogSeven");
                deadSheeps = false;
            }
        }


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
        StartCoroutine("DialogSix");
    }

    public IEnumerator DialogOne()
    {
        ControlMove.SetActive(false);
        ControlHit.SetActive(false);       
        yield return new WaitForSeconds(1f);
        
        Time.timeScale = 0.1f;
        swordAnimator.speed = 10f;
        skipAnimator.speed = 10f;
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 0);
        //yield return new WaitForSeconds(0.2f);
        //UI.GetComponent<GameMenu>().Sword();


        //Invoke("CallableSkip", 5f);
        //count = 0;
        //while (count < 1)
        //{
        //    yield return new WaitForEndOfFrame();
        //}
        //yield return new WaitUntil(SkipAndPlay);

        procceed = false;
        ControlMove.SetActive(true);
        Debug.Log("DRINK MORE NIKOLINE");
        Time.timeScale = 1f;
        swordAnimator.speed = 1f;
        skipAnimator.speed = 1f;


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
        Time.timeScale = 0.1f;
        swordAnimator.speed = 10f;
        skipAnimator.speed = 10f;
        enemyBillboard.GetComponent<CameraBillboard>().speaker = TrollA;
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
        yield return new WaitForSeconds(3f);
        //ControlMove.SetActive(false);

        //Time.timeScale = 0.1f;
        //swordAnimator.speed = 10f;
        //skipAnimator.speed = 10f;
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 2);
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
    }


    public IEnumerator DialogFour()
    {
        ControlMove.SetActive(false);

        //Time.timeScale = 0.1f;
        //swordAnimator.speed = 10f;
        //skipAnimator.speed = 10f;
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 3);
        //yield return new WaitForSeconds(0.2f);
        //UI.GetComponent<GameMenu>().Sword();
        
        // GATE!
        InvisWallOne.GetComponent<Animator>().SetTrigger("gateOpen");
        ControlMove.SetActive(true);

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
        enemyBillboard.GetComponent<CameraBillboard>().speaker = TrollB;
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 4);
        tutImage.SetActive(true);
        tutImgAnimator.SetBool("playLearnTaunt", true);
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
        yield return new WaitForSeconds(0.1f);
        ScreenFreeze.SetActive(true);
        handAnimator.speed = 10f;
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 7);
        //yield return new WaitForSeconds(0.2f);
        //UI.GetComponent<GameMenu>().Sword();
        skipBtn.SetActive(false);
        handAnimator.SetBool("playTaunt", true);
        ControlHit.SetActive(true);

        learnedToTaunt = false;
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


        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(5);

    }
   

}
