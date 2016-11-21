using UnityEngine;
using System.Collections;

public class TriggerSystemTutorial_02 : MonoBehaviour {

    public GameObject Dialog;

   


    public void TiggerCheck(int dialogNumber)
    {
        if (dialogNumber == 1)
        {
            Dialog.GetComponent<Tutorial_02_Dialog>().StartCoroutine("DialogOne");
        }

        if (dialogNumber == 2)
        {
            Dialog.GetComponent<Tutorial_02_Dialog>().StartCoroutine("DialogTwo");
        }

        if (dialogNumber == 3)
        {
            Dialog.GetComponent<Tutorial_02_Dialog>().StartCoroutine("DialogThree");
        }

        if (dialogNumber == 4)
        {
            Dialog.GetComponent<Tutorial_02_Dialog>().StartCoroutine("DialogFour");
        }

        if (dialogNumber == 5)
        {
            Dialog.GetComponent<Tutorial_02_Dialog>().StartCoroutine("DialogFive");
        }

        if (dialogNumber == 6)
        {
            Dialog.GetComponent<Tutorial_02_Dialog>().StartCoroutine("DialogSix");
        }

        if (dialogNumber == 7)
        {
            Dialog.GetComponent<Tutorial_02_Dialog>().StartCoroutine("DialogSeven");
        }

        if (dialogNumber == 8)
        {
            Dialog.GetComponent<Tutorial_02_Dialog>().StartCoroutine("DialogEight");
        }


    }




}
