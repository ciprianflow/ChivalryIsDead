using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class FadeScriptTooltip : MonoBehaviour {

    public float stayTime = 2f;
    public float fadeTime = 1f;
    //public GameObject TooltipText;

    float timer = 0;

    CanvasGroup CG;

	// Use this for initialization
	void OnEnable()
    {
        CG = GetComponent<CanvasGroup>();
        StartCoroutine(stay());
        if (PlayerPrefs.GetString("Language") == "English")
        {
            transform.GetChild(0).GetComponent<Text>().text = "Days until the wedding";
        }
        else
        {
            transform.GetChild(0).GetComponent<Text>().text = "Dage indtil brylluppet";
        }
    }

    void OnDisable()
    {
        timer = 0f;
    }
	
    IEnumerator stay()
    {

        yield return new WaitForSeconds(stayTime);
        CG.blocksRaycasts = false;
        StartCoroutine(fade());

    }

    IEnumerator fade()
    {

        while(timer < fadeTime)
        {
            timer += Time.deltaTime;
            CG.alpha = timer / fadeTime;
            yield return new WaitForSeconds(Time.deltaTime);

        }
    }
}
