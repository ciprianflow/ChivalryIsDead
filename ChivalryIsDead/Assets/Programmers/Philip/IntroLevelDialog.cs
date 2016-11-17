using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class IntroLevelDialog : MonoBehaviour {

    public GameObject Player;

    public GameObject UI;
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
    void Start () {
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
                StartCoroutine("DialogTwo");
                learnedToMove = true;

            }
        }

        if(!learnedToAttack)
        {
            if (Player.GetComponent<PlayerScript>().attacking)
            {
                DialogFour();
                learnedToAttack = true;
            }
        }
       
        

	}

    public IEnumerator DialogOne()
    {
        ControlMove.SetActive(false);
        ControlHit.SetActive(false);
        yield return new WaitForSeconds(1);
        Time.timeScale = 0.1f;
        swordAnimator.speed = 10f;
        UI.GetComponent<GameMenu>().Princess();
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 0);
        startingPos = Player.transform.position;
        learnedToMove = false;



        Invoke("CallableSkip", 0.9f);
        //WwiseInterface.Instance.PlayKnightCombatSound(KnightCombatHandle.Attack, gameObject);
      
        yield return new WaitUntil(SkipAndPlay);
        
        procceed = false;
        handAnimator.speed = 10f;
        handAnimator.SetBool("playLeftJoy", true);
        ControlMove.SetActive(true);

    }

    public bool SkipAndPlay()
    {
        return procceed;
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
        UI.GetComponent<GameMenu>().Princess();
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 1);
        handAnimator.SetBool("playLeftJoy", false);
        handAnimator.speed = 1f;
        swordAnimator.speed = 1f;
        Time.timeScale = 1f;
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
        handAnimator.speed = 10f;
        swordAnimator.speed = 10f;
        UI.GetComponent<GameMenu>().Princess();
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 2);
        handAnimator.SetBool("playRightJoy", true);
        ControlHit.SetActive(true);
        learnedToAttack = false;
        return null;

       
    }


    void DialogFour()
    {
        handAnimator.SetBool("playRightJoy", false);
        handAnimator.speed = 1f;
        swordAnimator.speed = 1f;
        Time.timeScale = 1f;
        ControlMove.SetActive(true);
    }

    public IEnumerator DialogFive()
    {
        UI.GetComponent<GameMenu>().Princess();
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 3);
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(2);
    }

   

    

    

}
