using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Skip : MonoBehaviour, IPointerUpHandler, IPointerDownHandler {
    float t = 3;
    public GameObject text;
    public Text texttext;
    bool holding = false;
	// Use this for initialization
	void Awake () {
        texttext = text.GetComponent<Text>();
        //texttext.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (holding) {
            t -= Time.deltaTime;
            texttext.text = "Hold for " + (int)(t + 0.99f) + " sec";
            if(t <= 0) {
                SceneManager.LoadScene(3);
            }
        }
	}

    public void OnPointerDown(PointerEventData eventData) {

        Debug.Log("Touched");
        holding = true;
        //texttext.enabled = true;
        text.SetActive(true);

    }
    public void OnPointerUp(PointerEventData eventData) {
        texttext.text = "Hold for 3 sec";
        t = 3;
        holding = false;
        //texttext.enabled = false;
        text.SetActive(false);
    }
}
