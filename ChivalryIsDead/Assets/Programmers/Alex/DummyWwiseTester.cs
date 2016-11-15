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
        Debug.Log("Playing random sound.");
        WwiseInterface.Instance.PlayMenuSound(MenuHandle.PlayButtonPressed);
        StartCoroutine(CoTestSound());
    }
}
