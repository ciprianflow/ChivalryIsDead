using UnityEngine;
using System.Collections;

public class DummyWwiseTester : MonoBehaviour {
	// Use this for initialization
	void Start () {

        StartCoroutine(CoTestSound());
	}

    IEnumerator CoTestSound()
    {
        float innerTimer = 2f;
        while (innerTimer >= 0) {
            innerTimer -= Time.deltaTime;
            yield return null;
        }
        if (WwiseInterface.Instance != null)
        {
            Debug.Log("Playing test sound.");
            WwiseInterface.Instance.PlayMenuSound(MenuHandle.PlayButtonPressed);
            WwiseInterface.Instance.PlayKnightCombatSound(KnightCombatHandle.Attack, gameObject);

        }
        else
        {
            Debug.LogWarning("Attach a WwiseInterface to a class (e.g. WwiseGlobal) to use the WwiseInterface!");
        }
        StartCoroutine(CoTestSound());
    }
}
