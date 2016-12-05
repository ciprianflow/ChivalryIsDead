using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public static class StaticData {

    internal static float VersionNumber = 0.3011f;

    //Current quest and objectives
    public static MultiQuest currQuest;

    //Meta Data
    public static int daysLeft = 12;
    public static int maxDays = daysLeft;
    public static float Reputation = 80;
    public static readonly float MaxReputation = Reputation;
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

    public static SwordDialogueHandle GetSwordMood(string s)
    {
        switch (s)
        {
            case "Angry":
                return SwordDialogueHandle.Angry;
            case "Crazy":
                return SwordDialogueHandle.Crazy;
            case "Determined":
                return SwordDialogueHandle.Determined;
            case "ExplanatoryLong":
                return SwordDialogueHandle.ExplanatoryLong;
            case "ExplanatoryShort":
                return SwordDialogueHandle.ExplanatoryShort;
            case "HappyLong":
                return SwordDialogueHandle.HappyLong;
            case "HappyShort":
                return SwordDialogueHandle.HappyShort;
            case "Neutral":
                return SwordDialogueHandle.Neutral;
            default:
                throw new System.NotImplementedException();                
        }
    }

    public static PrincessDialogueHandle GetPrincessMood(string s)
    {
        switch (s)
        {
            case "Happy":
                return PrincessDialogueHandle.Happy;
            case "SuperHappy":
                return PrincessDialogueHandle.SuperHappy;
            case "Crazy":
                return PrincessDialogueHandle.Crazy;
            case "SuperCrazy":
                return PrincessDialogueHandle.SuperCrazy;
            case "Sad":
                return PrincessDialogueHandle.Sad;
            case "SuperSad":
                return PrincessDialogueHandle.SuperSad;
            case "Flirty":
                return PrincessDialogueHandle.Flirty;
            case "SuperFlirty":
                return PrincessDialogueHandle.SuperFlirty;
            default:
                throw new System.NotImplementedException();
        }
    }

    #endregion
}
