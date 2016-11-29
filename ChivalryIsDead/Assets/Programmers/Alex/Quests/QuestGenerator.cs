﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Class used for procedural generation of quests, as described in the design document:
/// https://docs.google.com/document/d/1kywJ2Dgp2PWXsAsWruRTZ9UMfJE-9R39urLEXmOuQPo/edit
///
/// All quests should respect the maximum reputation change limit.
/// </summary>
public class QuestGenerator
{

    private System.Random rng;
    private int currentDay;
    private int currentReputation;

    public Difficulty CurrentDifficulty
    {
        get
        {
            if (currentReputation > QuestParameters.DifficultyThresholds[0]) {
                return Difficulty.Easy;
            } else if (currentReputation > QuestParameters.DifficultyThresholds[1]) {
                return Difficulty.Medium;
            } else {
                return Difficulty.Hard;
            }
        }
    }

    public EnemyTypes AvailableEnemies
    {
        get
        {
            var minDist = int.MaxValue;
            var idx = -1;
            for (int i = 0; i < QuestParameters.EnemyThresholds.Count; i++) {
                var ET = QuestParameters.EnemyThresholds[i];
                if (ET.DayMarker <= currentDay && currentDay - ET.DayMarker <= minDist) {
                    minDist = currentDay - ET.DayMarker;
                    idx = i;
                }
            }
            var thresholdValue = QuestParameters.EnemyThresholds[idx].AvailableEnemies;

            foreach (ReputationOverride rO in QuestParameters.ReputationOverrides) {
                if (currentReputation <= rO.RepMarker && ((thresholdValue & rO.enemyType) != rO.enemyType))
                    thresholdValue += (int)rO.enemyType;
            }
            return thresholdValue;
        }
    }

    public QuestGenerator(int currentDay, int currentReputation, int rngSeed)
    {
        if (!QuestParameters.IsLoaded)
            QuestParameters.LoadQuestParameters();

        this.currentDay = currentDay;
        this.currentReputation = currentReputation;
        this.rng = new System.Random(rngSeed);
    }

    // Actual protection quest generation. Polish and change the generator to follow this paradigm.
    private IQuest GenerateProtectQuest(int numFriendlies)
    {
        var protQuest = new BaseQuest();

        var curHouseStatus = QuestParameters.DifficultyDefs[(int)CurrentDifficulty].houseStatus;
        if (HasFlag(curHouseStatus, (int)HouseStatus.Bakery) || HasFlag(curHouseStatus, (int)HouseStatus.Farmhouse)) // ||
            //(HasFlag(curHouseStatus, (int)HouseStatus.Random) && System.Convert.ToBoolean(rng.Next(0, 2))))
        {
            protQuest.Objectives.Add(new ProtectTargetObjective(22));
            numFriendlies--;
        }

        // Adds sheep objectives.
        // Removed at request of the game designer (28-11-16)
        //for (int i = 0; i < numFriendlies; i++) {
        //    protQuest.Objectives.Add(new ProtectTargetObjective(21));
        //}

        return protQuest;
    }

    private IQuest GenerateDestroyQuest(int numNonSuicide, int numSuicide)
    {
        // Makes sure that suicide monsters are spawned if an override is active.
        if (numSuicide == 0 && (AvailableEnemies & EnemyTypes.HasSuicide) == EnemyTypes.HasSuicide)
            numSuicide = rng.Next(QuestParameters.ReputationOverrides[2].minOverride, QuestParameters.ReputationOverrides[2].maxOverride);

        var destQuest = new BaseQuest();
        var meleeAvailable = (AvailableEnemies & EnemyTypes.HasMelee) == EnemyTypes.HasMelee;

        for (int i = 0; i < numNonSuicide; i++) {
            if (i % 2 == 0 && meleeAvailable)
                destQuest.Objectives.Add(new DestroyTargetObjective(11));
            else
                destQuest.Objectives.Add(new DestroyTargetObjective(12));
        }
        for (int i = 0; i < numSuicide; i++) {
            destQuest.Objectives.Add(new DestroyTargetObjective(13)); 
        }

        return destQuest;
    }
    
    public MultiQuest GenerateMultiProtectQuest()
    {
        MultiQuest MQ = new MultiQuest();
        QuestType questType = QuestType.Protect;

        var numFriendlies = 
            rng.Next(
                QuestParameters.DifficultyDefs[(int)CurrentDifficulty].minFriendlies,
                QuestParameters.DifficultyDefs[(int)CurrentDifficulty].maxFriendlies + 1);
        var numNonSuicide =
            rng.Next(
                QuestParameters.DifficultyDefs[(int)CurrentDifficulty].minNonSuicide,
                QuestParameters.DifficultyDefs[(int)CurrentDifficulty].maxNonSuicide + 1);
        var numSuicide =
            rng.Next(
                QuestParameters.DifficultyDefs[(int)CurrentDifficulty].minSuicide,
                QuestParameters.DifficultyDefs[(int)CurrentDifficulty].maxSuicide + 1);

        MQ.Objectives.Add(GenerateProtectQuest(numFriendlies));
        MQ.Objectives.Add(GenerateDestroyQuest(numNonSuicide, numSuicide));
        MQ.Objectives.Add(new TimerObjective(31));

        var hasHouse = MQ.GetAllObjectives().Any(o => (o as BaseObjective).targetID == 22);
        int AvailableFriendlies = 0;/*(int)FriendlyTypes.Sheep;*/
        AvailableFriendlies += hasHouse && (CurrentDifficulty == Difficulty.Easy || CurrentDifficulty == Difficulty.Medium) ? (int)FriendlyTypes.Bakery : (int)FriendlyTypes.None;
        AvailableFriendlies += hasHouse && (CurrentDifficulty == Difficulty.Medium || CurrentDifficulty == Difficulty.Hard) ? (int)FriendlyTypes.Farmhouse : (int)FriendlyTypes.None;
        MQ.Data = new QuestData(questType, numNonSuicide + numSuicide, numFriendlies, AvailableEnemies, (FriendlyTypes)AvailableFriendlies);

        return MQ;
    }

    public MultiQuest GenerateMultiDestroyQuest()
    {
        MultiQuest MQ = new MultiQuest();
        QuestType questType = QuestType.Destroy;
        var numNonSuicide =
            rng.Next(
                QuestParameters.DifficultyDefs[(int)CurrentDifficulty].minNonSuicide,
                QuestParameters.DifficultyDefs[(int)CurrentDifficulty].maxNonSuicide + 1);
        var numSuicide =
            rng.Next(
                QuestParameters.DifficultyDefs[(int)CurrentDifficulty].minSuicide,
                QuestParameters.DifficultyDefs[(int)CurrentDifficulty].maxSuicide + 1);

        MQ.Data = new QuestData(questType, numNonSuicide + numSuicide, 0, AvailableEnemies, FriendlyTypes.None);

        MQ.Objectives.Add(GenerateDestroyQuest(numNonSuicide, numSuicide));
        MQ.Objectives.Add(new TimerObjective(31));

        return MQ;
    }

    public MultiQuest GenerateMultiQuest()
    {
        MultiQuest MQ;

        var questType = currentReputation > QuestParameters.DifficultyThresholds[0] ? QuestType.Protect : (QuestType)rng.Next(1, 3);
        switch (questType) {
            case QuestType.Protect:
                MQ = GenerateMultiProtectQuest(); break;
            case QuestType.Destroy:
                MQ = GenerateMultiDestroyQuest(); break;
            default:
                throw new System.Exception("Random int out of switch range.");
        }

        return MQ;
    }

    private bool HasFlag(HouseStatus e, int value)
    {
        return (e & (HouseStatus)value) == e;
    }
}
