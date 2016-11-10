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
    // Actual protection quest generation. Polish and change the generator to follow this paradigm.
    private IQuest GenerateProtectQuest(int numFriendlies)
    {
        var protQuest = new BaseQuest();
        for (int i = 0; i < numFriendlies; i++) {
            protQuest.Objectives.Add(new ProtectTargetObjective(21));
        }
        protQuest.Objectives.Add(new TimerObjective(31));

        return protQuest;
    }

    private IQuest GenerateDestroyQuest(int numEnemies, EnemyTypes types)
    {
        var destQuest = new BaseQuest();

        var hasMelee = (types & EnemyTypes.HasMelee) == EnemyTypes.HasMelee;
        var hasRanged = (types & EnemyTypes.HasRanged) == EnemyTypes.HasRanged;
        var hasSuicide = (types & EnemyTypes.HasSuicide) == EnemyTypes.HasSuicide;

        var numTypes = System.Convert.ToInt32(hasMelee) + System.Convert.ToInt32(hasRanged) + System.Convert.ToInt32(hasSuicide);

        var numMelee =  hasMelee ? numEnemies / numTypes : 0;
        var numRanged =  hasRanged ? numEnemies / numTypes : 0;
        var numSuicide = hasSuicide ? numEnemies / numTypes : 0;
        var numRemaining = numEnemies - (numMelee + numRanged + numSuicide);

        // TODO: Replace ID with Enum?
        for (int i = 0; i < numMelee; i++) {
            destQuest.Objectives.Add(new DestroyTargetObjective(11));
        }
        for (int i = 0; i < numRanged; i++) {
            destQuest.Objectives.Add(new DestroyTargetObjective(12));
        }
        for (int i = 0; i < numSuicide; i++) {
            destQuest.Objectives.Add(new DestroyTargetObjective(13)); 
        }
        for (int i = 0; i < numRemaining; i++) {
            var enemyID = Random.Range(11, 14);
            destQuest.Objectives.Add(new DestroyTargetObjective(enemyID));
        }

        return destQuest;
    }
    
    public MultiQuest GenerateProtectQuest(EnemyTypes enemyTypes, int enemyAvg, int friendlyAvg, out QuestData questData)
    {
        MultiQuest MQ = new MultiQuest();
        QuestType questType = QuestType.Protect;
        int enemies = Random.Range(enemyAvg - 2, enemyAvg + 3);
        int friendlies = Random.Range(friendlyAvg - 2, friendlyAvg + 3);

        questData = new QuestData(questType, enemies, friendlies, enemyTypes);

        MQ.Objectives.Add(GenerateProtectQuest(friendlies));
        MQ.Objectives.Add(GenerateDestroyQuest(enemies, enemyTypes));

        return MQ;
    }

    public MultiQuest GenerateDestroyQuest(EnemyTypes enemyTypes, int enemyAvg, out QuestData questData)
    {
        MultiQuest MQ = new MultiQuest();
        QuestType questType = QuestType.Destroy;
        int enemies = Random.Range(enemyAvg - 2, enemyAvg + 3);

        questData = new QuestData(questType, enemies, 0, enemyTypes);

        MQ.Objectives.Add(GenerateDestroyQuest(enemies, enemyTypes));

        return MQ;
    }

    public MultiQuest GenerateMultiQuest(EnemyTypes enemyTypes, int enemyAvg, int friendlyAvg, out QuestData questData)
    {
        MultiQuest MQ;

        var questType = Random.Range(0, 2);
        switch (questType) {
            case 0:
                MQ = GenerateProtectQuest(enemyTypes, enemyAvg, friendlyAvg, out questData); break;
            case 1:
                MQ = GenerateDestroyQuest(enemyTypes, enemyAvg, out questData); break;
            default:
                throw new System.Exception("Random int out of switch range.");
        }

        return MQ;
    }

    public MultiQuest GenerateMultiQuest(out QuestData questData)
    {
        return GenerateMultiQuest(EnemyTypes.HasMelee | EnemyTypes.HasRanged, 3, 3, out questData);
    }
}
