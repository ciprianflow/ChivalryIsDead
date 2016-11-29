using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SettingsMngr : MonoBehaviour {

    public GameObject joystick_move;
    public GameObject joystick_action;
    //public Text buttonText;
    //public Text thisButtonText;
    //public Button butA;
    //public Button butB;
    //public Dragit DragA;
    //public Dragit DragB;

    bool settingButtons = false;

    // Use this for initialization
    void Awake () {
	if(PlayerPrefs.GetInt("Swapped") == 1) {
            Debug.Log("AWAKE SWAPPED");

            Vector3 tempX = joystick_move.transform.localPosition;
            joystick_move.transform.localPosition = joystick_action.transform.localPosition;
            joystick_action.transform.localPosition = tempX;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}


    //public void setButtonPos() {
    //    //settingButtons = !settingButtons;

    //    //Time.timeScale = 0f;
    //    //joystick.SetActive(!settingButtons);
    //    //butA.enabled = !settingButtons;
    //    //butB.enabled = !settingButtons;
    //    //DragA.canDrag = settingButtons;
    //    //DragB.canDrag = settingButtons;
    //    //if (settingButtons) { 
    //    //    buttonText.text = "Please set the button positions";
    //    //    thisButtonText.text = "Lock Buttons";
    //    //}
    //    //else {
    //    //    buttonText.text = "";
    //    //    thisButtonText.text = "Set Buttons";
    //    //}
    //}

    public void swapSides() {
        Debug.Log("SWAPPED");
        Vector3 tempX = joystick_move.transform.localPosition;
        joystick_move.transform.localPosition = joystick_action.transform.localPosition;
        joystick_action.transform.localPosition = tempX;
        joystick_move.GetComponent<CnControls.SimpleJoystick1>().swap();
        joystick_action.GetComponent<CnControls.SimpleJoystickAction1>().swap();
        PlayerPrefs.SetInt("Swapped", Mathf.Abs(PlayerPrefs.GetInt("Swapped") - 1));
    }
}
