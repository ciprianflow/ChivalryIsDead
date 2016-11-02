using UnityEngine;
using System.Collections.Generic;

public class HubData : ScriptableObject {

    public int DayNumber;
    public int GlobalReputation;
    public int QueueLength { get { return Mathf.CeilToInt(GlobalReputation / 20); } }
    public List<IObjective> AvailableQuests;

    public HubData()
    {
        DayNumber = 0;
        GlobalReputation = 1000;
    }

    void OnEnable()
    {
        AvailableQuests = DummyQuestGenerator.GenerateMultipleQuests(QueueLength);
    }

}
