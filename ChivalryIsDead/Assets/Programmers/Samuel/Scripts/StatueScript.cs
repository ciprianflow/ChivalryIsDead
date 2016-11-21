using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatueScript : MonoBehaviour {

    public Sprite[] sprites;

	// Use this for initialization
	void Start () {

        if (sprites == null || sprites.Length == 0)
            return;

        float percantage = StaticData.Reputation / 100;

        float tic = 1f / sprites.Length;

        for(int i = 0; i < sprites.Length; i++)
        {
            if(tic * i > percantage)
            {
                GetComponent<SpriteRenderer>().sprite = sprites[i];
                break;
            }
        }


	}
}
