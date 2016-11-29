using UnityEngine;
using System.Collections;

public class Knight_Events : MonoBehaviour {

    public PlayerScript ps;

	// Use this for initialization
	void Start () {
        //Time.timeScale = 0.1f;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButton("Jump")) {
            //AkSoundEngine.PostEvent("knight_walk", transform.parent.gameObject);
            //AkSoundEngine.PostEvent("knight

        }
    }

    void EndState(int value) {
        //Debug.Log(value);
        ps.EndState(value);
    }

    void step() {

        AkSoundEngine.PostEvent("knight_move", gameObject);
        //WwiseInterface.Instance.PlayKnightCombatVoiceSound(KnightCombatVoiceHandle.Attack, this.gameObject);
        //WwiseInterface.Instance.PlayKnightCombatSound(KnightCombatHandle., transform.parent.gameObject);
    }
}
