using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Flags]
public enum HouseStatus
{
    Random, Bakery, Farmhouse
    //Random, Never, Always
}

[Serializable]
public class EnemyThreshold
{
    public bool IsEmpty { get { return DayMarker == 0 && AvailableEnemies == EnemyTypes.None; } }
    public int DayMarker;
    public EnemyTypes AvailableEnemies;
}

[Serializable]
public class DifficultyDefinition
{
    public int minNonSuicide, maxNonSuicide;
    public int minSuicide, maxSuicide;

    public int minFriendlies, maxFriendlies;
    public HouseStatus houseStatus;
}

[Serializable]
public class QuestParameters : ScriptableObject
{
    public static bool IsLoaded { get; set; }
    public static int[] DifficultyThresholds;
    public static List<EnemyThreshold> EnemyThresholds;
    public static DifficultyDefinition[] DifficultyDefs;

    public int[] difficultyThresholds;
    [SerializeField]
    public DifficultyDefinition[] difficultyDefinitions;
    [SerializeField]
    public List<EnemyThreshold> enemyThresholds;

    public QuestParameters()
    {
        difficultyThresholds = new int[2];
        difficultyDefinitions = new DifficultyDefinition[3];
        enemyThresholds = new List<EnemyThreshold>();
    }

    public void PushToStatic()
    {
        DifficultyThresholds = difficultyThresholds;
        DifficultyDefs = difficultyDefinitions;
        EnemyThresholds = enemyThresholds;
    }

    public static void LoadQuestParameters()
    {
        var QP = Resources.Load<QuestParameters>(@"data/QuestParameters");
        if (QP == null)
            throw new NullReferenceException("QuestParameters were not found. Create QuestParameters.asset and restart game.");
        else
            QP.PushToStatic();
    }
}
