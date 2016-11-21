using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class IntroLevelDialog : MonoBehaviour {

    public GameObject Player;

    public GameObject MainCam;
    public GameObject AnimCam;

    public GameObject UI;
    public GameObject ScreenFreeze;
    public GameObject ControlMove;
    public GameObject ControlHit;

    public GameObject InvisWallOne;
    public GameObject InvisWallTwo;
    bool procceed;

    bool learnedToMove;
    bool learnedToAttack;
    Vector3 startingPos;

    public GameObject HandCanvas;
    Animator handAnimator;
    public Animator swordAnimator;


    // Use this for initialization
    void Awake () {
        procceed = false;
        learnedToMove = true;
        learnedToAttack = true;
        handAnimator = HandCanvas.GetComponent<Animator>();


    }

	
	// Update is called once per frame
	void Update () {
       
            
        if (!learnedToMove)
        {
            if (Vector3.Distance(startingPos, Player.transform.position) > 0.01f)
            {
                gameObject.GetComponent<DialogObject>().StopDialog();

                handAnimator.speed = 1f;
                swordAnimator.speed = 1f;
                Time.timeScale = 1f;
                handAnimator.SetBool("playLeftJoy", false);
                ScreenFreeze.SetActive(false);

                learnedToMove = true;

            }
        }

        if(!learnedToAttack)
        {
            if (Player.GetComponent<PlayerScript>().attacking)
            {
                gameObject.GetComponent<DialogObject>().StopDialog();
                AttackEvent();
                learnedToAttack = true;
            }
        }
       
        

	}

    public IEnumerator Start()
    {
        AnimCam.SetActive(true);
        ControlMove.SetActive(false);
        ControlHit.SetActive(false);
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 0);
       
        yield return new WaitForSeconds(2f);


        UI.GetComponent<GameMenu>().Sword();
        //Invoke("CallableSkip", 1.3f);

        
         
    }

    public IEnumerator DialogOne()
    {
        yield return new WaitUntil(SkipAndPlay);

        procceed = false;
        //yield return new WaitForSeconds(13f);
        MainCam.SetActive(true);
        AnimCam.SetActive(false);

        Time.timeScale = 0.1f;
        ScreenFreeze.SetActive(true);
        swordAnimator.speed = 10f;


        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 1);
        yield return new WaitForSeconds(0.2f);
        UI.GetComponent<GameMenu>().Sword();


        startingPos = Player.transform.position;
        learnedToMove = false;



        //Invoke("CallableSkip", 1.3f);

        //yield return new WaitUntil(SkipAndPlay);

        //procceed = false;


        handAnimator.speed = 10f;
        handAnimator.SetBool("playLeftJoy", true);
        ControlMove.SetActive(true);
    }


    public bool SkipAndPlay()
    {
        //return procceed;
        return true;
    }

    public void CallableSkip()
    {
        procceed = true;
    }

    public void ActivateMove()
    {
        ControlMove.SetActive(true);
        StopCoroutine("DialogOne");
    }

    public IEnumerator DialogTwo()
    {
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 2);
        yield return new WaitForSeconds(2f);
        UI.GetComponent<GameMenu>().Sword();

        //handAnimator.speed = 1f;
        //swordAnimator.speed = 1f;
        //Time.timeScale = 1f;
        //handAnimator.SetBool("playLeftJoy", false);


        
        
       
        
        InvisWallOne.SetActive(false);
        yield return null;
    }

    public void EventOne()
    {
        handAnimator.SetBool("playRightJoy", false);
        InvisWallTwo.SetActive(false);
    }

    public IEnumerator DialogThree()
    {
        ControlMove.SetActive(false);
        //JoyCanvas.transform.FindChild("Joystick_Move").gameObject.GetComponent<SimpleJoystick1>().StopMoving();

        Time.timeScale = 0.1f;
        ScreenFreeze.SetActive(true);
        handAnimator.speed = 10f;
        swordAnimator.speed = 10f;
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 3);
        yield return new WaitForSeconds(0.2f);
        UI.GetComponent<GameMenu>().Sword();
        handAnimator.SetBool("playRightJoy", true);
        ControlHit.SetActive(true);
        learnedToAttack = false;


       
    }


    void AttackEvent()
    {
        handAnimator.SetBool("playRightJoy", false);
        handAnimator.speed = 1f;
        swordAnimator.speed = 1f;
        Time.timeScale = 1f;
        ScreenFreeze.SetActive(false);
        ControlMove.SetActive(true);
        StartCoroutine("DialogFour");
    }

    public IEnumerator DialogFour()
    {
        yield return new WaitForSeconds(2);
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 4);
        yield return new WaitForSeconds(2);

        UI.GetComponent<GameMenu>().Sword();
        

    }

    public IEnumerator DialogFive()
    {
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 5);
        yield return new WaitForSeconds(2);
        UI.GetComponent<GameMenu>().Sword();
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(2);
    }







}
