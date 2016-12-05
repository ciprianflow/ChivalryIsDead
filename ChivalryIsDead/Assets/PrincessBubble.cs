using UnityEngine;
using System.Collections;

public class PrincessBubble : MonoBehaviour {

    public Texture DanishBubble;
    public Renderer textMat;
    // Use this for initialization
    void Awake () {
        if (PlayerPrefs.GetString("Language") == "Dansk" || 1 == 1) {
            textMat.material.SetTexture("_MainTex", DanishBubble);
        }

    }

    // Update is called once per frame
    void Update () {
	
	}
}
