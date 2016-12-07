using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class FadeScript : MonoBehaviour {

    public float stayTime = 2f;
    public float fadeTime = 1f;
    public GameObject howToPlay;
    public GameObject Unity;
    public GameObject DADIU;
    public GameObject Menu;

    float timer = 0;

    CanvasGroup CG;

	// Use this for initialization
	IEnumerator Start ()
    {
        DADIU.SetActive(true);
        yield return new WaitForSeconds(2f);
        DADIU.SetActive(false);
        Unity.SetActive(true);
        yield return new WaitForSeconds(2f);
        Unity.SetActive(false);

        if (!PlayerPrefs.HasKey("Setup"))
        {
            
            howToPlay.SetActive(true);
            yield return new WaitForSeconds(2f);
        }

        howToPlay.SetActive(false);
        Menu.SetActive(true);
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
