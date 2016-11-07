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
    #region Quest getters
    public IQuest GetRandomQuest()
    {
        var difficulty = Random.Range(0, 3);
        switch (difficulty) {
            case 0:
                return GenerateEasyQuest();
            case 1:
                return GenerateMediumQuest();
            case 2:
                return GenerateHardQuest();
            default:
                throw new System.Exception("Random int isn't in the range [0..2].");
        }
    }

    public IQuest GetQuest(Difficulty difficulty)
    {
        switch (difficulty) {
            case Difficulty.Easy:
                return GenerateEasyQuest();
            case Difficulty.Medium:
                return GenerateMediumQuest();
            case Difficulty.Hard:
                return GenerateHardQuest();
            default:
                Debug.LogWarning("Difficulty not specified, returning quest of random difficulty");
                return GetRandomQuest();
        }
    }

    public IQuest GetQuest(float repRatio)
    {
        if (repRatio > 0.6f) {
            return GenerateEasyQuest();
        } else if (repRatio > 0.2) {
            return GenerateMediumQuest();
        } else {
            return GenerateHardQuest();
        }
    }
    #endregion

    private IQuest GenerateEasyQuest()
    {
        IQuest retQuest;
        var questType = System.Convert.ToBoolean(Random.Range(0, 1));
        if (questType) { // DestroyTargetQuest
            retQuest = new BaseQuest("Destroy the enemies!", "Yalla Yalla, kabob 'dem fo' Dollah!", Difficulty.Easy);
            var meleeCount = Random.Range(2, 5);
            var rangedCount = Random.Range(0, 3);

            for (int i = 0; i < meleeCount + rangedCount; i++) {
                if (i <= meleeCount) {
                    retQuest.Objectives.Add(new DestroyTargetObjective(11));
                } else {
                    retQuest.Objectives.Add(new DestroyTargetObjective(12));
                }
            }
        } else { // ProtectTargetQuest
            retQuest = new BaseQuest("Defend the sheeple!", "Illuminati reptile people and shizz.", Difficulty.Easy);
            var sheepCount = Random.Range(1, 1); // TODO: Use an actual range
            var meleeCount = Random.Range(3, 3 + sheepCount); // TODO: Just fix...

            for (int i = 0; i < sheepCount + meleeCount; i++) {
                if (i < sheepCount)
                    retQuest.Objectives.Add(new ProtectTargetObjective(21));
                else
                    retQuest.Objectives.Add(new DestroyTargetObjective(11));
            }
        }

        return retQuest;
    }

    private IQuest GenerateMediumQuest()
    {
        var meleeCount = Random.Range(2, 5);
        var rangedCount = Random.Range(0, 3);

        throw new System.NotImplementedException();

        return null;
    }

    private IQuest GenerateHardQuest()
    {
        var meleeCount = Random.Range(2, 5);
        var rangedCount = Random.Range(0, 3);

        throw new System.NotImplementedException();

        return null;
    }
}
