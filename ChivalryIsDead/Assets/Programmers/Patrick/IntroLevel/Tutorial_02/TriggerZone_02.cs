using UnityEngine;
using System.Collections;

public class TriggerZone_02 : MonoBehaviour {

    public GameObject Trigger;
    public GameObject UI;
    public int DialogNumber;


    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (UI.GetComponent<GameMenu>().speaking == false)
            {

                Trigger.GetComponent<TriggerSystemTutorial_02>().TiggerCheck(DialogNumber);
                UI.GetComponent<GameMenu>().speaking = true;
                DestoryThis();
            }


        }

    }

    void DestoryThis()
    {
        Destroy(gameObject);
    }
}
