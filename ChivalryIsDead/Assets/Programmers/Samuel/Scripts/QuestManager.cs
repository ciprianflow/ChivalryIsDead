using UnityEngine;
using System.Collections.Generic;

public static class QuestManager {

    //Current quest and objectives
    public static MultiQuest currQuest;

    public static List<IObjective> GetObjectives()
    {
        return currQuest.Objectives;
    }
}
