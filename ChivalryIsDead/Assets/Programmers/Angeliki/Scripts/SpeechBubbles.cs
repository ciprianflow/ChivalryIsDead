using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpeechBubbles : MonoBehaviour {
    public bool needsInput;
    public bool isPaused;

    Text text;
	// Use this for initialization
	void Start () {
        //text = GetComponentInChildren(Text);

        // stuff for pausing
        Time.timeScale = 0;
        isPaused = true;
    }

    // Update is called once per frame
    void Update () {
        // stuff for pausing
        if (needsInput)
        {
            StartCoroutine(WaitForKeyDown(KeyCode.Mouse0));
        }
    }

    // stuff for pausing
    IEnumerator WaitForKeyDown(KeyCode keyCode)
    {
        while (!Input.GetKeyDown(keyCode))
        {
            
            yield return null;
        }
        Time.timeScale = 1;
        isPaused = false;
    }
}
