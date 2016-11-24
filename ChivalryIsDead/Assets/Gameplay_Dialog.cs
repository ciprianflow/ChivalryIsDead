using UnityEngine;
using System.Collections;

public class Gameplay_Dialog : MonoBehaviour {


    public GameObject playerTauntBubble;
    public GameObject enemyTauntBubble;


    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
        if(Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine("PlayerTaunt");
        }
	}

    public IEnumerator PlayerTaunt()
    {
        playerTauntBubble.SetActive(true);
        yield return new WaitForSeconds(4f);
        playerTauntBubble.SetActive(false);
    }
}
