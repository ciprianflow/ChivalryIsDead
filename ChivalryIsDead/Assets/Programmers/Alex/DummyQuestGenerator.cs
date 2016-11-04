using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public static class DummyQuestGenerator {
    
    // TODO: Update with objective generation logic.
    public static IObjective GenerateObjective()
    {
        return new DummyObjective();
    }

    public static BaseQuest GenerateProtectQuest(int objCount)
    {
        List<IObjective> objectives = new List<IObjective>();
        for (int i = 0; i < objCount; i++) {
            objectives.Add(GenerateObjective());
        }

        var retQuest = new BaseQuest(GenerateDummyTitle(), "", Difficulty.Easy);
        retQuest.Objectives.AddRange(objectives);
        return retQuest;
    }

    public static List<IQuest> GenerateMultipleQuests(int count)
    {
        List<IQuest> quests = new List<IQuest>();
        for (int i = 0; i < count; i++) {
            quests.Add(GenerateProtectQuest(1));
        }
        return quests;
    }

    // TODO: Replace with actual title generation.
    private static string GenerateDummyTitle()
    {
        var titleLength = UnityEngine.Random.Range(10, 20);
        var hasSpace = Convert.ToBoolean(UnityEngine.Random.Range(0, 2));
        var spaceLoc = UnityEngine.Random.Range(4, titleLength - 4);

        StringBuilder sBuilder = new StringBuilder();
        char nextChar;
        for (int i = 0; i < titleLength; i++) {
            if (hasSpace && i == spaceLoc) {
                sBuilder.Append(' ');
                continue;
            }

            if (i == 0 || (hasSpace && i == spaceLoc + 1)) { 
                nextChar = (char)UnityEngine.Random.Range(65, 90);
            } else {
                nextChar = (char)UnityEngine.Random.Range(97, 122);
            }
            sBuilder.Append(nextChar);
        }

        return sBuilder.ToString();
    }
}
