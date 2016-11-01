using UnityEngine;
using System.Collections;

public class SpeechBubbles : MonoBehaviour {
    public bool needsInput;
    public bool isPaused;
	// Use this for initialization
	void Start () {
        Time.timeScale = 0;
        isPaused = true;
    }

    // Update is called once per frame
    void Update () {
        if (needsInput)
        {
            StartCoroutine(WaitForKeyDown(KeyCode.Mouse0));
        }
    }

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
