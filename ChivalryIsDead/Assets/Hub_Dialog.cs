using UnityEngine;
using System.Collections;

public class Hub_Dialog : MonoBehaviour {

    public GameObject skipBtn;

    public GameObject winScreen;
    public GameObject loseScreen;
    public GameObject end;

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

    public IEnumerator Win()
    {
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 11);
        end.SetActive(true);
        skipBtn.SetActive(false);
        yield return new WaitForSeconds(8f);
        HubDataManager.ResetHubData();

        ResetData();

        winScreen.SetActive(true);
        StaticData.Reputation = StaticData.MaxReputation;
    }

    public IEnumerator Lose()
    {
        yield return new WaitForSeconds(1f);
        this.gameObject.GetComponent<DialogObject>().StopDialog();
        this.gameObject.GetComponent<DialogObject>().StartCoroutine("DialogSystem", 12);
        end.SetActive(true);
        skipBtn.SetActive(false);
        yield return new WaitForSeconds(8f);
        HubDataManager.ResetHubData();

        ResetData();

        loseScreen.SetActive(true);
        StaticData.Reputation = StaticData.MaxReputation;
    }

    private void ResetData()
    {
        PlayerPrefs.SetInt("lowCombo", 0);
        PlayerPrefs.SetInt("noGetHit", 1); 
        PlayerPrefs.SetInt("noSheepKill", 1);
        PlayerPrefs.SetInt("noTaunt", 1);
        PlayerPrefs.SetInt("noOverreact", 1);
        PlayerPrefs.SetInt("poorlyOverreact", 1);

        PlayerPrefs.SetInt("SuicideLevel", 0);
        PlayerPrefs.SetInt("SuicideTut", 0);


        //Reset to Tut
        /*
        // Player Controls
        PlayerPrefs.SetInt("Attack", 0);
        PlayerPrefs.SetInt("Taunt", 0);
        PlayerPrefs.SetInt("Overreact", 0);

        // Tut Levels
        PlayerPrefs.SetInt("AttackLevel", 0);
        PlayerPrefs.SetInt("TauntLevel", 0);
        PlayerPrefs.SetInt("OverreactLevel", 0);
        */

    }
}
