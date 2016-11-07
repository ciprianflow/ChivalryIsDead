using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class ToggleSwap : MonoBehaviour {

    public SettingsMngr SM;
    void Awake() {
        gameObject.GetComponent<Button>().onClick.AddListener(delegate { SM.swapSides(); });
    }
}
