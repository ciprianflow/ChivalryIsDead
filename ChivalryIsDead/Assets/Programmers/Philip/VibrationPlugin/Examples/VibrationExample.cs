using UnityEngine;
using System.Collections;

public class VibrationExample : MonoBehaviour
{
    // Test text
    public GUIText guiText;

    void Start()
    {
        // Check the vibrator on existence
        if (Vibration.HasVibrator())
            guiText.text = "Vibration.HasVibrator() = true";
        else
            guiText.text = "Vibration.HasVibrator() = false";
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 250, 50), "Vibrate();"))
            Vibration.Vibrate();

        if (GUI.Button(new Rect(10, 90, 250, 50), "Vibrate(200);"))
            Vibration.Vibrate(200);

        if (GUI.Button(new Rect(10, 170, 250, 50), "Vibrate(250);"))
            Vibration.Vibrate(250);

        if (GUI.Button(new Rect(10, 250, 250, 50), "Vibrate(50);"))
            Vibration.Vibrate(50);

        if (GUI.Button(new Rect(10, 330, 250, 50), "Cancel();"))
            Vibration.Cancel();
    }
}