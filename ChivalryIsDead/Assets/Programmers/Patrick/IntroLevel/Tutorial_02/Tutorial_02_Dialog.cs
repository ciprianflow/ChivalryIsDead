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
    public GameObject InvisWallTwo;
    bool procceed;

    bool learnedToGetHit;
    bool learnedToTaunt;
    bool deadSheeps;

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
        deadSheeps = true;
        handAnimator = HandCanvas.GetComponent<Animator>();

        foreach (GameObject Sheep in Sheeps)
        {
            Sheep.GetComponent<SheepAI>().enabled = false;
        }
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
            if (Player.GetComponent<PlayerActionController>().GetPlayerState() == PlayerState.HIT)
            {
                StartCoroutine("DialogSix");
                learnedToTaunt = true;
            }
        }

        if(!Sheeps[0].activeSelf && !Sheeps[1].activeSelf && !Sheeps[2].activeSelf && !Sheeps[3].activeSelf)
        {
            if(deadSheeps)
            {
                StartCoroutine("DialogSeven");
                deadSheeps = false;
            }
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
        ControlMove.SetActive(false);
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
        ControlMove.SetActive(true);

        StartCoroutine("DialogThree");
    }

    public IEnumerator DialogThree()
    {
        yield return new WaitForSeconds(3f);
        ControlMove.SetActive(false);

        Time.timeScale = 0.1f;
        swordAnimator.speed = 10f;
        skipAnimator.speed = 10f;
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 2);
        yield return new WaitForSeconds(0.2f);
        UI.GetComponent<GameMenu>().Sword();
        count = 0;
        while (count < 2)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitUntil(SkipAndPlay);
        procceed = false;
        skipAnimator.speed = 1f;
        swordAnimator.speed = 1f;
        Time.timeScale = 1f;
        ControlMove.SetActive(true);
        learnedToGetHit = false;
    }


    public IEnumerator DialogFour()
    {
        ControlMove.SetActive(false);

        Time.timeScale = 0.1f;
        swordAnimator.speed = 10f;
        skipAnimator.speed = 10f;
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 3);
        yield return new WaitForSeconds(0.2f);
        UI.GetComponent<GameMenu>().Sword();

        count = 0;
        while (count < 2)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitUntil(SkipAndPlay);
        procceed = false;
        skipAnimator.speed = 1f;
        swordAnimator.speed = 1f;
        Time.timeScale = 1f;
        ControlMove.SetActive(true);
        InvisWallTwo.SetActive(false);
    }

    public IEnumerator DialogFive()
    {
        ControlMove.SetActive(false);

        Time.timeScale = 0.1f;
        swordAnimator.speed = 10f;
        skipAnimator.speed = 10f;
        enemyBillboard.GetComponent<CameraBillboard>().speaker = TrollB;
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 4);
        yield return new WaitForSeconds(0.2f);
        UI.GetComponent<GameMenu>().Sword();

        count = 0;
        while (count < 2)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitUntil(SkipAndPlay);
        procceed = false;
        skipAnimator.speed = 1f;
        swordAnimator.speed = 1f;
        Time.timeScale = 1f;
        ControlMove.SetActive(true);
        ControlHit.SetActive(true);

        learnedToTaunt = false;
    }

    public IEnumerator DialogSix()
    {
        ControlMove.SetActive(false);

        Time.timeScale = 0.1f;
        swordAnimator.speed = 10f;
        skipAnimator.speed = 10f;
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 5);
        yield return new WaitForSeconds(0.2f);
        UI.GetComponent<GameMenu>().Sword();

        count = 0;
        while (count < 5)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitUntil(SkipAndPlay);
        procceed = false;
        skipAnimator.speed = 1f;
        swordAnimator.speed = 1f;
        Time.timeScale = 1f;
        ControlMove.SetActive(true);


        foreach (GameObject Sheep in Sheeps)
        {
            Sheep.GetComponent<SheepAI>().enabled = true;
        }

        //StartCoroutine("DialogSeven");
        
    }

   


    public IEnumerator DialogSeven()
    {

        //yield return new WaitForSeconds(10f);

        ControlMove.SetActive(false);

        Time.timeScale = 0.1f;
        swordAnimator.speed = 10f;
        skipAnimator.speed = 10f;
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 6);
        yield return new WaitForSeconds(0.2f);
        UI.GetComponent<GameMenu>().Sword();

        count = 0;
        while (count < 6)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitUntil(SkipAndPlay);
        procceed = false;
        skipAnimator.speed = 1f;
        swordAnimator.speed = 1f;
        Time.timeScale = 1f;


        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(4);

    }
   

}
