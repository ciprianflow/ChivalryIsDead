using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Tutorial_02_Dialog : MonoBehaviour {

    public GameObject Player;

    public GameObject MainCam;
    public GameObject AnimCam;

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

    // Use this for initialization
    void Start()
    {
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
                StartCoroutine("DialogThree");
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
        AnimCam.SetActive(true);
        ControlMove.SetActive(false);
        ControlHit.SetActive(false);
        yield return new WaitForSeconds(1);
        ControlMove.SetActive(true);
        MainCam.SetActive(true);
        AnimCam.SetActive(false);
        UI.GetComponent<GameMenu>().Princess();
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 0);

        Invoke("CallableSkip", 5f);

        yield return new WaitUntil(SkipAndPlay);

        procceed = false;
        InvisWallOne.SetActive(false);

    }

    public bool SkipAndPlay()
    {
        return procceed;
    }

    public void CallableSkip()
    {
        procceed = true;
    }




    public IEnumerator DialogTwo()
    {
        Time.timeScale = 0.1f;
        swordAnimator.speed = 10f;


        UI.GetComponent<GameMenu>().Princess();
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 1);
        Invoke("CallableSkip", 0.3f);
        yield return new WaitUntil(SkipAndPlay);
        procceed = false;

        swordAnimator.speed = 1f;
        Time.timeScale = 1f;

        learnedToGetHit = false;

    }

    public IEnumerator DialogThree()
    {
        InvisWallTwo.SetActive(false);

        UI.GetComponent<GameMenu>().Princess();
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 2);


        yield return null;


    }


    public IEnumerator DialogFour()
    {
        yield return null;
    }

    public IEnumerator DialogFive()
    {
        yield return null;
    }

}
