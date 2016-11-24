using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;


/// <summary>
/// <author>Jefferson Reis</author>
/// <explanation>Works only on Android, or platform that supports mp3 files. To test, change the platform to Android.</explanation>
/// </summary>

public class GoogleSpeech : MonoBehaviour
{
    public string words = "Hello";

    IEnumerator Start()
    {
        // Remove the "spaces" in excess
        Regex rgx = new Regex("\\s+");
        // Replace the "spaces" with "% 20" for the link Can be interpreted
        string result = rgx.Replace(words, "%20");
        string url = "http://translate.google.com/translate_tts?tl=en&q=" + result;
        WWW www = new WWW(url);
        yield return www;
        GetComponent<AudioSource>().clip = www.GetAudioClip(false, false, AudioType.MPEG);
        GetComponent<AudioSource>().Play();
    }

    void OnGUI()
    {
        words = GUI.TextField(new Rect(Screen.width / 2 - 200 / 2, 10, 200, 30), words);
        if (GUI.Button(new Rect(Screen.width / 2 - 150 / 2, 40, 150, 50), "Speak"))
        {
            StartCoroutine(Start());
        }
    }


}//closes the class