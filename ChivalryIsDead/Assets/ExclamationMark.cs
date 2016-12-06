using UnityEngine;
using System.Collections;

public class ExclamationMark : MonoBehaviour {
    public GameObject exclMark;
	// Use this for initialization
	void Start () {
       
    }

    // Update is called once per frame
    void Update () {
        if (gameObject.GetComponent<MonsterAI>().getState() == State.Death)
        {
            exclMark.SetActive(false);

        }

    }

    public GameObject ExclaMark()
    {

        exclMark.SetActive(true);
        return exclMark;
    }

    public void DisableMark()
    {
        exclMark.SetActive(false);
    }

}
