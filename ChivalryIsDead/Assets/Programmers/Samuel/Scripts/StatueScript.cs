using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatueScript : MonoBehaviour {

    public Sprite[] sprites;

	// Use this for initialization
	void Start () {

        if (sprites == null || sprites.Length == 0)
            return;

        float percantage = StaticData.Reputation / StaticData.MaxReputation;


        Debug.Log(StaticData.Reputation + " / " + StaticData.MaxReputation);
        float tic = 1f / sprites.Length;

        for(int i = 0; i < sprites.Length; i++)
        {
            Debug.Log((i * tic + tic) + " percantage = " + percantage);
            if(tic * i + tic >= percantage)
            {
                GetComponent<Image>().sprite = sprites[i];
                break;
            }
        }


	}
}
