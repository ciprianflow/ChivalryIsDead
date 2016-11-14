using UnityEngine;
using System.Collections;

public class IntroLevelDialog : MonoBehaviour {

    public GameObject UI;
    public GameObject ControlMove;
    public GameObject ControlHit;

    public GameObject InvisWallOne;
    public GameObject InvisWallTwo;
    public GameObject InvisWallThree;

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public IEnumerator DialogOne()
    {
        ControlMove.SetActive(false);
        ControlHit.SetActive(false);
        UI.GetComponent<GameMenu>().Sword();
        yield return new WaitForSeconds(1);
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 0);
        ControlMove.SetActive(true);

    }

    public IEnumerator DialogTwo()
    {
        UI.GetComponent<GameMenu>().Sword();
        yield return new WaitForSeconds(1);
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 1);
        yield return new WaitForSeconds(2);
        InvisWallOne.SetActive(false);
    }

    public IEnumerator DialogThree()
    {
        UI.GetComponent<GameMenu>().Sword();
        yield return new WaitForSeconds(1);
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 2);
        ControlHit.SetActive(true);
        yield return new WaitForSeconds(2);
        InvisWallTwo.SetActive(false);
    }

    public IEnumerator DialogFour()
    {
        UI.GetComponent<GameMenu>().Sword();
        yield return new WaitForSeconds(1);
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 3);
        yield return new WaitForSeconds(2);
        InvisWallThree.SetActive(false);
    }

    public IEnumerator DialogFive()
    {
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 4);
        yield return null;
    }


}
