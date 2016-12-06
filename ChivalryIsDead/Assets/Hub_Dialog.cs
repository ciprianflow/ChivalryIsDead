using UnityEngine;
using System.Collections;

public class Hub_Dialog : MonoBehaviour {

    public GameObject skipBtn;

	// Use this for initialization
	IEnumerator Start () {

        yield return new WaitForSeconds(0.5f);

        if (StaticData.Reputation <= 110 && StaticData.Reputation > 100)
        {
            yield return new WaitForSeconds(2f);
            Debug.Log("110 - 100");
            this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 0);
            skipBtn.SetActive(false);
        }

        if (StaticData.Reputation <= 100 && StaticData.Reputation > 90)
        {
            yield return new WaitForSeconds(2f);
            Debug.Log("100 - 90");
            this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 1);
            skipBtn.SetActive(false);
        }

        if (StaticData.Reputation <= 90 && StaticData.Reputation > 80)
        {
            yield return new WaitForSeconds(1f);
            Debug.Log("90 - 80");
            this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 2);
            skipBtn.SetActive(false);
        }

        if (StaticData.Reputation <= 80 && StaticData.Reputation > 70)
        {
            Debug.Log("80 - 70");
            this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 3);
        }

        if (StaticData.Reputation <= 70 && StaticData.Reputation > 60)
        {
            Debug.Log("70 - 60");
            this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 4);
        }

        if (StaticData.Reputation <= 60 && StaticData.Reputation > 50)
        {
            Debug.Log("60 - 50");
            this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 5);
        }

        if (StaticData.Reputation <= 50 && StaticData.Reputation > 40)
        {
            Debug.Log("50 - 40");
            this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 6);
        }

        if (StaticData.Reputation <= 40 && StaticData.Reputation > 30)
        {
            Debug.Log("40 - 30");
            this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 7);
        }

        if (StaticData.Reputation <= 30 && StaticData.Reputation > 20)
        {
            Debug.Log("30 - 20");
            this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 8);
        }

        if (StaticData.Reputation <= 20 && StaticData.Reputation > 10)
        {
            Debug.Log("20 - 10");
            this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 9);
        }

        if (StaticData.Reputation <= 10 && StaticData.Reputation > 0)
        {
            Debug.Log("10 - 0");
            this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 10);
        }

        

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
