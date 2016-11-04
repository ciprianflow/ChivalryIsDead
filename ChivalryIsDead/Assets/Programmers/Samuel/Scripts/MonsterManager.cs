using UnityEngine;
using System.Collections.Generic;

public class MonsterManager {

    List<MonsterAI> monsters = new List<MonsterAI>();

    /// <summary>
    /// Spawn a single monster at a certain location
    /// </summary>
    public void SpawnMonsters(MonsterAI monster)
    {

    }

    /// <summary>
    /// remove a monster with the index i, returns true if all monsters have been removed
    /// </summary>
    /// <param name="i"></param>
    public bool RemoveMonster(int i)
    {
        monsters.RemoveAt(i);
        if(monsters.Count <= 0)
            return true;
        return false;
    }


}
