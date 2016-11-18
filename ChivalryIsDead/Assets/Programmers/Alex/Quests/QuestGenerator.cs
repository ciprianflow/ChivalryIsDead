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
    private int currentDay;
    private int currentReputation;

    public Difficulty CurrentDifficulty
    {
        get
        {
            if (currentReputation > 60) {
                return Difficulty.Easy;
            } else if (currentReputation > 20) {
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
            if (currentDay < 1) {
                return EnemyTypes.HasRanged;
            } else if (currentDay < 4) {
                return EnemyTypes.HasMelee | EnemyTypes.HasRanged;
            } else {
                return EnemyTypes.HasMelee | EnemyTypes.HasRanged | EnemyTypes.HasSuicide;
            }
        }
    }

    public QuestGenerator(int currentDay, int currentReputation)
    {
        this.currentDay = currentDay;
        this.currentReputation = currentReputation;
    }

    // Actual protection quest generation. Polish and change the generator to follow this paradigm.
    private IQuest GenerateProtectQuest(int numFriendlies)
    {
        var protQuest = new BaseQuest();
        for (int i = 0; i < numFriendlies; i++) {
            protQuest.Objectives.Add(new ProtectTargetObjective(21));
        }

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
    
    public MultiQuest GenerateMultiProtectQuest(out QuestData questData)
    {
        MultiQuest MQ = new MultiQuest();
        QuestType questType = QuestType.Protect;

        var numFriendlies = CurrentDifficulty != Difficulty.Hard ? Random.Range(1, 4) : Random.Range(5, 11);
        int minNonSuicide = 0, maxNonSuicide = 0;
        int minSuicide = 0, maxSuicide = 0;
        switch (CurrentDifficulty) {
            case Difficulty.Easy:
                minNonSuicide = 4;
                maxNonSuicide = 7;
                break;
            case Difficulty.Medium:
                minNonSuicide = 2;
                maxNonSuicide = 5;
                minSuicide = 1;
                maxSuicide = 3;
                break;
            case Difficulty.Hard:
                minNonSuicide = 1;
                maxNonSuicide = 3;
                minSuicide = 3;
                maxSuicide = 7;
                break;
        }

        var numNonSuicide = Random.Range(minNonSuicide, maxNonSuicide + 1); // Max is +1 because random generator max is exclusive.
        var numSuicide = Random.Range(minSuicide, maxSuicide + 1);

        questData = new QuestData(questType, numNonSuicide + numSuicide, numFriendlies, AvailableEnemies);

        MQ.Objectives.Add(GenerateProtectQuest(numFriendlies));
        MQ.Objectives.Add(GenerateDestroyQuest(numNonSuicide, numSuicide));
        MQ.Objectives.Add(new TimerObjective(31));

        return MQ;
    }

    public MultiQuest GenerateMultiDestroyQuest(out QuestData questData)
    {
        MultiQuest MQ = new MultiQuest();
        QuestType questType = QuestType.Destroy;
        int minNonSuicide = 0, maxNonSuicide = 0;
        int minSuicide = 0, maxSuicide = 0;
        switch (CurrentDifficulty) {
            case Difficulty.Easy:
                minNonSuicide = 4;
                maxNonSuicide = 7;
                break;
            case Difficulty.Medium:
                minNonSuicide = 2;
                maxNonSuicide = 5;
                minSuicide = 1;
                maxSuicide = 3;
                break;
            case Difficulty.Hard:
                minNonSuicide = 1;
                maxNonSuicide = 3;
                minSuicide = 3;
                maxSuicide = 7;
                break;
        }

        var numNonSuicide = Random.Range(minNonSuicide, maxNonSuicide + 1); // Max is +1 because random generator max is exclusive.
        var numSuicide = Random.Range(minSuicide, maxSuicide + 1);

        questData = new QuestData(questType, numNonSuicide + numSuicide, 0, AvailableEnemies);

        MQ.Objectives.Add(GenerateDestroyQuest(numNonSuicide, numSuicide));

        return MQ;
    }

    public MultiQuest GenerateMultiQuest(out QuestData questData)
    {
        MultiQuest MQ;

        var questType = currentReputation > 60 ? QuestType.Protect : (QuestType)Random.Range(1, 3);
        switch (questType) {
            case QuestType.Protect:
                MQ = GenerateMultiProtectQuest(out questData); break;
            case QuestType.Destroy:
                MQ = GenerateMultiDestroyQuest(out questData); break;
            default:
                throw new System.Exception("Random int out of switch range.");
        }

        return MQ;
    }
}
