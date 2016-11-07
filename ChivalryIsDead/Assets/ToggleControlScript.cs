using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ToggleControlScript : MonoBehaviour {
    void Awake() {
        Player ps = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        gameObject.GetComponent<Button>().onClick.AddListener(delegate { ps.toggleControls(); });
    }
}
