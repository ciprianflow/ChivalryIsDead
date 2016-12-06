using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class FadeScript : MonoBehaviour {

    public float stayTime = 2f;
    public float fadeTime = 1f;
    public GameObject howToPlay;

    float timer = 0;

    CanvasGroup CG;

	// Use this for initialization
	IEnumerator Start ()
    {
        if(!PlayerPrefs.HasKey("Setup"))
        {
            howToPlay.SetActive(true);
        }
        
        yield return new WaitForSeconds(4f);
        howToPlay.SetActive(false);
        this.gameObject.SetActive(true);
        CG = GetComponent<CanvasGroup>();
        StartCoroutine(stay());
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
            CG.alpha = 1 - timer / fadeTime;
            yield return new WaitForSeconds(Time.deltaTime);

        }

        this.gameObject.SetActive(false);

    }
}
