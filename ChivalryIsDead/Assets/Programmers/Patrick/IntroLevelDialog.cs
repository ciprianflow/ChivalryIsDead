using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class IntroLevelDialog : MonoBehaviour {

    public GameObject UI;
    public GameObject ControlMove;
    public GameObject ControlHit;

    public GameObject InvisWallOne;
    public GameObject InvisWallTwo;
    bool procceed;
    // Use this for initialization
    void Start () {
        procceed = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public IEnumerator DialogOne()
    {
        ControlMove.SetActive(false);
        ControlHit.SetActive(false);
        //UI.GetComponent<GameMenu>().Sword();
        yield return new WaitForSeconds(0.3f);
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 0);
        
        yield return new WaitUntil(SkipAndPlay);
        procceed = false;
        ControlMove.SetActive(true);
        yield return new WaitForSeconds(4);
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

    public void ActivateMove()
    {
        ControlMove.SetActive(true);
        StopCoroutine("DialogOne");
    }

    public IEnumerator DialogTwo()
    {
        //UI.GetComponent<GameMenu>().Sword();
        yield return new WaitForSeconds(0.3f);
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 1);
        ControlHit.SetActive(true);
    }

    public void EventOne()
    {
        InvisWallTwo.SetActive(false);
    }

    public IEnumerator DialogThree()
    {
        //UI.GetComponent<GameMenu>().Sword();
        yield return new WaitForSeconds(0.3f);
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 2);
        yield return new WaitForSeconds(2);
    }


    public IEnumerator DialogFour()
    {
        //UI.GetComponent<GameMenu>().Sword();
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 3);
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(2);
    }

   

    

    

}
