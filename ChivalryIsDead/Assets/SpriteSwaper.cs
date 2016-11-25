using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpriteSwaper : MonoBehaviour {
    Sprite[] sprites;
    Image tutImage;

    // Use this for initialization
    void Start () {
        tutImage = gameObject.GetComponent<Image>();
        sprites = Resources.LoadAll<Sprite>("TutorialSlides/GetHit");

        StartCoroutine("Swaper");

    }

    // Update is called once per frame
    void Update () {
        
    }

    IEnumerator Swaper()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            tutImage.sprite = sprites[i];
            yield return new WaitForSeconds(0.05f); //because of the timescale


        }
        StartCoroutine("Swaper");

    }
}
