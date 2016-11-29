using UnityEngine;
using System.Collections;

public class TriggerMonster : MonoBehaviour {

    public GameObject enemy;

	// Use this for initialization
	void Start () {
        gameObject.transform.SetParent(enemy.transform);
        gameObject.transform.localPosition = new Vector3(0, 0, 0);

    }

    // Update is called once per frame
    void Update () {

        
	}
}
