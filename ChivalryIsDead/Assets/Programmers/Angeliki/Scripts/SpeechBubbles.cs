using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpeechBubbles : MonoBehaviour {
    public bool needsInput;
    public bool isPaused;

    HorizontalLayoutGroup bubblePadding;
    RectTransform BubbleRect;
    RectTransform TextRect;
    // Use this for initialization
    void Start () {
        // change padding of the speech bubble image so text stays inside it
        bubblePadding = transform.GetComponent<HorizontalLayoutGroup>();
        BubbleRect = transform.GetComponent<RectTransform>();
        
        int imgWidth = (int)BubbleRect.rect.width;
        int imgHeight = (int)BubbleRect.rect.height;
        
        RectOffset tempPadding = new RectOffset(
                bubblePadding.padding.left,
                bubblePadding.padding.right,
                bubblePadding.padding.top,
                bubblePadding.padding.bottom);
        
        tempPadding.left = imgWidth  / 2;
        tempPadding.right = imgWidth / 2;
        tempPadding.top = imgHeight  / 2;
        tempPadding.bottom = imgHeight / 2;

        bubblePadding.padding = tempPadding;

        bubblePadding.padding.left = 100;
        bubblePadding.padding.right = 100;
        bubblePadding.padding.top = 70;
        bubblePadding.padding.bottom = 100;


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
