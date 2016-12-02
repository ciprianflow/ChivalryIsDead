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
    /*None, */HasMelee = 1, HasRanged = 2, HasSuicide = 4
}

[Flags]
public enum FriendlyTypes
{
    None, Well, Bakery, Farmhouse = 4
}

public struct QuestData
{
    public string TypeString { get { return Enum.GetName(typeof(QuestType), Type); } }
    public QuestType Type;
    public int EnemyCount;
    public int FriendlyCount;
    public EnemyTypes PresentEnemies;
    public FriendlyTypes PresentFriends;

    public QuestData(QuestType questType, int enemyCount, int friendlyCount, EnemyTypes presentEnemies, FriendlyTypes presentFriends)
    {
        Type = questType;
        EnemyCount = enemyCount;
        FriendlyCount = friendlyCount;
        PresentEnemies = presentEnemies;
        PresentFriends = presentFriends;
    }

    public IEnumerable<string> GetEnemies()
    {
        List<string> enemyTypes = new List<string>();
        if ((PresentEnemies & EnemyTypes.HasMelee) == EnemyTypes.HasMelee)
            enemyTypes.Add("Melee");
        if ((PresentEnemies & EnemyTypes.HasRanged) == EnemyTypes.HasRanged)
            enemyTypes.Add("Ranged");
        if ((PresentEnemies & EnemyTypes.HasSuicide) == EnemyTypes.HasSuicide)
            enemyTypes.Add("Suicide");

        return enemyTypes;
    }

    public IEnumerable<string> GetFriends()
    {
        List<string> friendTypes = new List<string>();
        if ((PresentFriends & FriendlyTypes.Sheep) == FriendlyTypes.Sheep)
            friendTypes.Add("Sheep");
        if ((PresentFriends & FriendlyTypes.Bakery) == FriendlyTypes.Bakery)
            friendTypes.Add("Bakery");
        if ((PresentFriends & FriendlyTypes.Farmhouse) == FriendlyTypes.Farmhouse)
            friendTypes.Add("Farmhouse");

        return friendTypes;
    }
}
