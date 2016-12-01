using System.Collections.Generic;
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
            var thresholdValue = (uint)QuestParameters.EnemyThresholds[idx].AvailableEnemies;

            foreach (ReputationOverride rO in QuestParameters.ReputationOverrides) {
                if (currentReputation <= rO.RepMarker && ((thresholdValue & (uint)rO.enemyType) != (uint)rO.enemyType))
                    thresholdValue += (uint)rO.enemyType;
            }
            return (EnemyTypes)thresholdValue;
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
    private IQuest GenerateProtectQuest()
    {
        var protQuest = new BaseQuest();

        var curHouseStatus = QuestParameters.DifficultyDefs[(int)CurrentDifficulty].houseStatus;
        protQuest.Objectives.Add(new ProtectTargetObjective(22));

        return protQuest;
    }

    private IQuest GenerateDestroyQuest(int numNonSuicide, int numSuicide)
    {
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

        int numFriendlies, numNonSuicide, numSuicide;
        GetObjectiveCounts(out numFriendlies, out numNonSuicide, out numSuicide);

        MQ.Objectives.Add(GenerateProtectQuest());
        MQ.Objectives.Add(GenerateDestroyQuest(numNonSuicide, numSuicide));
        MQ.Objectives.Add(new TimerObjective(31));

        //var hasHouse = MQ.GetAllObjectives().Any(o => (o as BaseObjective).targetID == 22);
        int AvailableFriendlies = (int)FriendlyTypes.None; // (int)FriendlyTypes.Sheep;
        AvailableFriendlies += (CurrentDifficulty == Difficulty.Easy || CurrentDifficulty == Difficulty.Medium) ? (int)FriendlyTypes.Bakery : (int)FriendlyTypes.None;
        AvailableFriendlies += (CurrentDifficulty == Difficulty.Medium || CurrentDifficulty == Difficulty.Hard) ? (int)FriendlyTypes.Farmhouse : (int)FriendlyTypes.None;
        MQ.Data = new QuestData(questType, numNonSuicide + numSuicide, numFriendlies, AvailableEnemies, (FriendlyTypes)AvailableFriendlies);

        // Adds sheep spawn.
        for (int i = 0; i < numFriendlies; i++) {
            MQ.Spawns.Add(new ProtectTargetObjective(21));
        }

        return MQ;
    }

    public MultiQuest GenerateMultiDestroyQuest()
    {
        MultiQuest MQ = new MultiQuest();
        QuestType questType = QuestType.Destroy;

        int numFriendlies, numNonSuicide, numSuicide;
        GetObjectiveCounts(out numFriendlies, out numNonSuicide, out numSuicide);

        MQ.Data = new QuestData(questType, numNonSuicide + numSuicide, 0, AvailableEnemies, FriendlyTypes.None);

        MQ.Objectives.Add(GenerateDestroyQuest(numNonSuicide, numSuicide));
        MQ.Objectives.Add(new TimerObjective(31));

        // Adds sheep spawn.
        for (int i = 0; i < numFriendlies; i++) {
            MQ.Spawns.Add(new ProtectTargetObjective(21));
        }

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

    private void GetObjectiveCounts(out int numFriendlies, out int numNonSuicide, out int numSuicide)
    {

        numFriendlies =
            rng.Next(
                QuestParameters.DifficultyDefs[(int)CurrentDifficulty].minFriendlies,
                QuestParameters.DifficultyDefs[(int)CurrentDifficulty].maxFriendlies + 1);
        numNonSuicide =
            rng.Next(
                QuestParameters.DifficultyDefs[(int)CurrentDifficulty].minNonSuicide,
                QuestParameters.DifficultyDefs[(int)CurrentDifficulty].maxNonSuicide + 1);
        numSuicide = (AvailableEnemies & EnemyTypes.HasSuicide) == EnemyTypes.HasSuicide ?
            rng.Next(
                QuestParameters.DifficultyDefs[(int)CurrentDifficulty].minSuicide,
                QuestParameters.DifficultyDefs[(int)CurrentDifficulty].maxSuicide + 1) :
            0;
    }

    private bool HasFlag(HouseStatus e, int value)
    {
        return (e & (HouseStatus)value) == e;
    }
}
