using UnityEngine;
using System.Collections;

public class endKills : MonoBehaviour {

    GameObject triggersystem;

    // Use this for initialization
    void Start()
    {
        triggersystem = GameObject.FindGameObjectWithTag("TriggerSystem");
    }

    // Update is called once per frame
    void Update () {
	
	}

    void OnDisable()
    {
        ++triggersystem.GetComponent<TriggerSystemIntroLevel>().endKill;
    }
}
