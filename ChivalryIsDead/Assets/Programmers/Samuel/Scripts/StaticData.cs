using UnityEngine;
using System.Collections.Generic;

public static class StaticData {

    //Current quest and objectives
    public static MultiQuest currQuest;
    
    // Limits
    public static int MaximumReputation = 100;
    public static int TotalDays = 12;

    //Meta Data
    public static int daysLeft = 14;
    public static float Reputation = 100;
    public static int Suspicion = 0;

    #region Helper functions
    //Helper functions
    public static List<IObjective> GetObjectives()
    {
        if (currQuest == null)
            return null;

        return currQuest.Objectives;
    }

    #endregion
}
