using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    private PlayerActionController playerActioncontroller;


    void Awake()
    {
        playerActioncontroller = GetComponent<PlayerActionController>();

    }

    // Use this for initialization
    void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            playerActioncontroller.Attack();
            Debug.Log("Q");
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            playerActioncontroller.HandleTaunt();
            Debug.Log("T");
        }



        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);

    }
}
