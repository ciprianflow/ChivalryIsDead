using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {

    PlayerScript ps;
    PlayerActionController pac;
    public GameObject AUI;

    // Use this for initialization
    void Awake () {
        ps = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        pac = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerActionController>();
        //AUI = GameObject.FindGameObjectWithTag("ActionUI");
        AUI = GameObject.Find("Canvas").transform.FindChild("Joystick_Action").transform.FindChild("ActionUI").gameObject;
        AUI.SetActive(true);
    }
    void Start() {
        AUI.SetActive(true);
        AUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(201, -509);
    }

    // Update is called once per frame
    void Update () {
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.4f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.4f)
            ps.move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        else {
            ps.move(0,0);
        }

        if (Input.GetButtonDown("Fire1")) {
            pac.HandleAttack();
        }
        if (Input.GetButtonDown("Fire2")) {
            pac.HandleTaunt();
        }
        if (Input.GetButtonDown("Fire3")) {
            pac.HandleOverreact();

        }
        if (Input.GetButtonDown("Jump")) {
            Debug.Log("^");
        }
    }

}
