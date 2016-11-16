using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CallVid : MonoBehaviour {

   

    // Use this for initialization
    void Start () {
        this.StartCoroutine(this.PlayStreamingVideo("CutsceneTest01.mp4"));
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    private IEnumerator PlayStreamingVideo(string url)
    {
        Debug.Log("Starting Movie");
        Handheld.PlayFullScreenMovie(url, Color.black, FullScreenMovieControlMode.CancelOnInput, FullScreenMovieScalingMode.AspectFit);
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        Debug.Log("Movie stopped");
        SceneManager.LoadScene(3);
    }
}
