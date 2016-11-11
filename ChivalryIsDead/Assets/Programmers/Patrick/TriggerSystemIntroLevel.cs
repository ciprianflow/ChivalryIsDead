using UnityEngine;
using System.Collections;

public class TriggerSystemIntroLevel : MonoBehaviour {

    public GameObject Dialog;


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
