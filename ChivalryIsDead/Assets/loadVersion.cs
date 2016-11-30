using UnityEngine;
using UnityEngine.UI;

public class loadVersion : MonoBehaviour {

	// Use this for initialization
	void Start () {

        GetComponent<Text>().text = "v." + StaticData.VersionNumber;

	}
}
