using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Skip : MonoBehaviour, IPointerUpHandler, IPointerDownHandler {
    float t = 3;
    public GameObject filledSkip;
    Image filledSkipImage;
    //public Text texttext;
    bool holding = false;
	// Use this for initialization
	void Awake () {
        //texttext = text.GetComponent<Text>();
        //texttext.enabled = false;
        filledSkipImage = filledSkip.GetComponent<Image>();
    }
	
	// Update is called once per frame
	void Update () {
        if (holding) {
            t -= Time.deltaTime;
            //texttext.text = "Hold for " + (int)(t + 0.99f) + " sec";
            //texttext.text = "Hold for " + Mathf.Ceil(t) + " sec";
            filledSkipImage.fillAmount = 1 - (t / 3);
            if (t <= 0) {
                AkSoundEngine.PostEvent("stopCinematic", gameObject);
                SceneManager.LoadScene(3);
            }
        }
	}

    public void OnPointerDown(PointerEventData eventData) {

        //Debug.Log("Touched");
        holding = true;
        //texttext.enabled = true;
        filledSkip.SetActive(true);

    }
    public void OnPointerUp(PointerEventData eventData) {
        //texttext.text = "Hold for 3 sec";
        t = 3;
        holding = false;
        filledSkipImage.fillAmount = 0;

        //texttext.enabled = false;
        filledSkip.SetActive(false);
    }
}
