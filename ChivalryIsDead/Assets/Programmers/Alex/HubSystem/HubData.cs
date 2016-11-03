using UnityEngine;
using System.Collections.Generic;

public class HubData : ScriptableObject {

    public int DaysLeft;
    public int GlobalReputation;
    public int QueueLength { get { return Mathf.CeilToInt(GlobalReputation / 20); } }
    public List<IObjective> AvailableQuests;

    public HubData()
    {
        DaysLeft = 0;
        GlobalReputation = 1000;
    }

    public void GenerateQuests()
    {
        AvailableQuests = DummyQuestGenerator.GenerateMultipleQuests(QueueLength);
    }
}
