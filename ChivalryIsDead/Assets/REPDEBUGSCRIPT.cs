using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class REPDEBUGSCRIPT : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Text>().text = StaticData.Reputation.ToString();
	}
	

}
