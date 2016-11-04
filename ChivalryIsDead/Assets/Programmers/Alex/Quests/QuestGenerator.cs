using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Class used for procedural generation of quests, as described in the design document:
/// https://docs.google.com/document/d/1kywJ2Dgp2PWXsAsWruRTZ9UMfJE-9R39urLEXmOuQPo/edit
///
/// All quests should respect the maximum reputation change limit.
/// Easy quests should have many enemies.
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
        return null;
    }

    private IQuest GenerateMediumQuest()
    {
        return null;
    }

    private IQuest GenerateHardQuest()
    {
        return null;
    }
}
