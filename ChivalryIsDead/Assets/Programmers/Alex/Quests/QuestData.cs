using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum QuestType
{
    Other, Destroy, Protect // Other is just a placeholder, shouldn't actually be used.
}

[Flags]
public enum EnemyTypes
{
    None, HasMelee, HasRanged, HasSuicide = 4
}

public struct QuestData
{
    public QuestType Type;
    public int EnemyCount;
    public int FriendlyCount;
    public EnemyTypes PresentEnemies;

    public QuestData(QuestType questType, int enemyCount, int friendlyCount, EnemyTypes presentEnemies)
    {
        Type = questType;
        EnemyCount = enemyCount;
        FriendlyCount = friendlyCount;
        PresentEnemies = presentEnemies;
    }
}
