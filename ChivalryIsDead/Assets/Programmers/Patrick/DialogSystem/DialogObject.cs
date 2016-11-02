using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DialogObject : MonoBehaviour {

    public DialogInfo dialog;

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
        if(Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine("Test", dialog);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            StopCoroutine("Test");
            gameObject.GetComponent<Text>().text = "";
        }

    }

    IEnumerator Test(DialogInfo v)
    {
        v.Dialog = v.Name.Length;

        for(int i = 0; i < v.Dialog; i++)
        {
            if(v.Name[i] == "Player")
            {
                gameObject.GetComponent<Text>().text = "Player: " + v.Text[i];
                yield return new WaitForSeconds(v.Wait[i]);
            }

            if (v.Name[i] == "Sword")
            {
                gameObject.GetComponent<Text>().text = "Sword: " + v.Text[i];
                yield return new WaitForSeconds(v.Wait[i]);
            }

            if (v.Name[i] == "Enemy")
            {
                gameObject.GetComponent<Text>().text = "Enemy: " + v.Text[i];
                yield return new WaitForSeconds(v.Wait[i]);
            }
        }

        gameObject.GetComponent<Text>().text = "";

    }
}
