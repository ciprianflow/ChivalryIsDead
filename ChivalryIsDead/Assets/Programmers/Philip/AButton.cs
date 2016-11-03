using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class AButton : MonoBehaviour {

    public Text t;
    Coroutine CR = null;

    public SettingsMngr SettingsManager;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void dibado(String button) {
        t.text = "YOU PUSHED " + button;

        //if (button == "C") {
        //    SettingsManager.setButtonPos();
        //}



        if (CR != null)
            StopCoroutine(CR);

        CR = StartCoroutine(wait2());
    }
    IEnumerator wait2() {
        yield return new WaitForSeconds(2);
        t.text = "";
    }
}
