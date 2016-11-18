using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class HubData
{
    public int RandomSeed;
    public int DaysLeft;
    public float GlobalReputation;
    public int QueueLength { get { return Mathf.CeilToInt(GlobalReputation / 20); } }
    [NonSerialized] public List<IQuest> AvailableQuests;

    public HubData()
    {
        RandomSeed = UnityEngine.Random.Range(0, int.MaxValue);
        DaysLeft = 14;
        GlobalReputation = 100;
    }

    public HubData(int daysLeft, float reputation)
    {
        DaysLeft = daysLeft;
        GlobalReputation = reputation;
    }

    public void GenerateQuests()
    {
        var curDay = StaticData.TotalDays - DaysLeft;
        QuestGenerator QG = new QuestGenerator(curDay, (int)GlobalReputation, RandomSeed);

        QuestData QD = new QuestData();

        if (AvailableQuests == null)
            AvailableQuests = new List<IQuest>();

        for (int i = 0; i < QueueLength; i++)
            AvailableQuests.Add(QG.GenerateMultiQuest(out QD));
    }
}
