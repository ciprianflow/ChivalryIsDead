using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class ToggleSwap : MonoBehaviour {
    void Awake() {
        SettingsMngr SM = GameObject.FindGameObjectWithTag("SettingsManager").GetComponent<SettingsMngr>();
        gameObject.GetComponent<Button>().onClick.AddListener(delegate { SM.swapSides(); });
    }
}
