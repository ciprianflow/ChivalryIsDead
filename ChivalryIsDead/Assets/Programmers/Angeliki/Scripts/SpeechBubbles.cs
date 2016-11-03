using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpeechBubbles : MonoBehaviour {
    public bool needsInput;
    public bool isPaused;

    HorizontalLayoutGroup bubblePadding;
    RectTransform BubbleRect;
    // Use this for initialization
    void Start () {
        // change padding of the speech bubble image so text stays inside it
        bubblePadding = transform.GetComponent<HorizontalLayoutGroup>();
        BubbleRect = this.transform as RectTransform;
        int width = (int)BubbleRect.rect.width;
        int height = (int)BubbleRect.rect.height;

        RectOffset tempPadding = new RectOffset(
                bubblePadding.padding.left,
                bubblePadding.padding.right,
                bubblePadding.padding.top,
                bubblePadding.padding.bottom);
        tempPadding.left = width / 4;
        tempPadding.right = width / 4;
        tempPadding.top = height / 4;
        tempPadding.bottom = height / 4;

        bubblePadding.padding = tempPadding;

       

        // stuff for pausing
        //Time.timeScale = 0;
        //isPaused = true;
    }

    // Update is called once per frame
    void Update () {
        // stuff for pausing
        //if (needsInput)
        //{
        //    StartCoroutine(WaitForKeyDown(KeyCode.Mouse0));
        //}
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
