using UnityEngine;
using System.Collections.Generic;

public static class StaticData {

    //Current quest and objectives
    public static MultiQuest currQuest;

    //Meta Data
    public static int daysLeft = 7;
    public static int maxDaysLeft = daysLeft;
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
