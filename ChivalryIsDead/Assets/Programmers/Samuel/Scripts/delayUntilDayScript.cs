using UnityEngine;
using System.Collections;

public class delayUntilDayScript : MonoBehaviour {

    public int appearAfterDay = 4;

	// Use this for initialization
	void Start () {

        if (appearAfterDay > (StaticData.maxDaysLeft - StaticData.daysLeft))
            gameObject.SetActive(false);

	}
}
