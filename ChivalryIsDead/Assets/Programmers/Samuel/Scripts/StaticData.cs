using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public static class StaticData {

    //Current quest and objectives
    public static MultiQuest currQuest;
    
    // Limits
    public static int MaximumReputation = 100;
    public static int TotalDays = 12;

    //Meta Data
    public static int daysLeft = 14;
    public static int maxDaysLeft = daysLeft;
    public static float Reputation = 100;
    public static readonly float MaxReputation = 100;
    public static int Suspicion = 0;

    #region Helper functions
    //Helper functions
    public static List<IObjective> GetObjectives()
    {
        if (currQuest == null)
            return null;

        return currQuest.Objectives;
    }

    public static IEnumerator PlayStreamingVideo(string url)
    {
        Debug.Log("Starting Movie");
        Handheld.PlayFullScreenMovie(url, Color.black, FullScreenMovieControlMode.CancelOnInput, FullScreenMovieScalingMode.AspectFit);
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        Debug.Log("Movie stopped");
        //SceneManager.LoadScene("MainMenu");
    }

    #endregion
}
