using UnityEngine;
using System.Collections;

public class TriggerSystemIntroLevel : MonoBehaviour {

    public GameObject Dialog;

    public int introKill;
    public int endKill;

    void Start()
    {
        introKill = 0;
        endKill = 0;
    }

    void Update()
    {
        if(introKill == 1)
        {
            Dialog.GetComponent<IntroLevelDialog>().EventOne();
            ++introKill;
        }

        if (endKill == 3)
        {
            Dialog.GetComponent<IntroLevelDialog>().StartCoroutine("DialogFour");
            ++endKill;
        }
    }


    public void TiggerCheck(int dialogNumber)
    {
        if (dialogNumber == 1)
        {
            Dialog.GetComponent<IntroLevelDialog>().StartCoroutine("DialogOne");
        }

        if (dialogNumber == 2)
        {
            Dialog.GetComponent<IntroLevelDialog>().StartCoroutine("DialogTwo");
        }

        if (dialogNumber == 3)
        {
            Dialog.GetComponent<IntroLevelDialog>().StartCoroutine("DialogThree");
        }

        if (dialogNumber == 4)
        {
            Dialog.GetComponent<IntroLevelDialog>().StartCoroutine("DialogFour");
        }


    }

  


}
