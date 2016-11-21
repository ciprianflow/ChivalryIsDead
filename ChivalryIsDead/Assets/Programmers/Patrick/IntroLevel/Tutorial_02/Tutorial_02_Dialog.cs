using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Tutorial_02_Dialog : MonoBehaviour {

    public GameObject Player;
    public GameObject enemyBillboard;
    public GameObject TrollA;
    public GameObject TrollB;

    public GameObject UI;
    public GameObject ControlMove;
    public GameObject ControlHit;

    public GameObject InvisWallOne;
    public GameObject InvisWallTwo;
    bool procceed;

    bool learnedToGetHit;
    bool learnedToTaunt;
    bool learnedToOverreact;

    public GameObject HandCanvas;
    Animator handAnimator;
    public Animator swordAnimator;
    public Animator skipAnimator;
    int count;

    // Use this for initialization
    void Start()
    {
        count = 0;
        procceed = false;
        learnedToGetHit = true;
        learnedToTaunt = true;
        learnedToOverreact = true;
        handAnimator = HandCanvas.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!learnedToGetHit)
        {
            if(Player.GetComponent<PlayerActionController>().GetPlayerState() == PlayerState.HIT)
            {
                StartCoroutine("DialogFour");
                learnedToGetHit = true;
            }
            
        }

        if (!learnedToTaunt)
        {
           
        }

        if (!learnedToOverreact)
        {

        }



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
        yield return new WaitForSeconds(0.2f);
        UI.GetComponent<GameMenu>().Sword();
        

        //Invoke("CallableSkip", 5f);
        while(count < 10)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitUntil(SkipAndPlay);

        procceed = false;
        ControlMove.SetActive(true);
        swordAnimator.speed = 1f;
        skipAnimator.speed = 1f;
        Time.timeScale = 1f;
        InvisWallOne.SetActive(false);

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
        Time.timeScale = 0.1f;
        swordAnimator.speed = 10f;
        skipAnimator.speed = 10f;
        enemyBillboard.GetComponent<CameraBillboard>().speaker = TrollA;
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 1);
        yield return new WaitForSeconds(0.2f);
        UI.GetComponent<GameMenu>().Sword();


        count = 0;
        while (count < 7)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitUntil(SkipAndPlay);
        procceed = false;
        skipAnimator.speed = 1f;
        swordAnimator.speed = 1f;
        Time.timeScale = 1f;


        StartCoroutine("DialogThree");
    }

    public IEnumerator DialogThree()
    {
        yield return new WaitForSeconds(3f);
        Time.timeScale = 0.1f;
        swordAnimator.speed = 10f;
        skipAnimator.speed = 10f;
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 2);
        yield return new WaitForSeconds(0.2f);
        UI.GetComponent<GameMenu>().Sword();
        count = 0;
        while (count < 7)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitUntil(SkipAndPlay);
        procceed = false;
        skipAnimator.speed = 1f;
        swordAnimator.speed = 1f;
        Time.timeScale = 1f;

        learnedToGetHit = false;


    }


    public IEnumerator DialogFour()
    {

        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 3);
        yield return new WaitForSeconds(2f);
        UI.GetComponent<GameMenu>().Sword();

        InvisWallTwo.SetActive(false);

        yield return null;
    }

    public IEnumerator DialogFive()
    {
        yield return null;
    }

}
