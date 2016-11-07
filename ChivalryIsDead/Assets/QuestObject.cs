using UnityEngine;
using System.Collections;

public class QuestObject : MonoBehaviour {

	// Use this for initialization
	void Awake () {

        transform.parent.GetComponent<MapManager>().SetQuestObject(this.transform);

	}

}
