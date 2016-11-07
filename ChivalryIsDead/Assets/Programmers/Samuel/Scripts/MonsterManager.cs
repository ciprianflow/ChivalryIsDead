using UnityEngine;
using System.Collections.Generic;

public class MonsterManager {

    List<MonsterAI> monsters = new List<MonsterAI>();
    Dictionary<int, GameObject> monsterPrefabs = new Dictionary<int, GameObject>();

    Transform monsterListObject;

    /// <summary>
    /// Spawn a single monster at a certain location
    /// </summary>
    public void SpawnMonsters(int ID, Vector3 pos)
    {
        GameObject obj = GameObject.Instantiate(monsterPrefabs[ID]);

        MonsterAI Monster = obj.GetComponent<MonsterAI>();

        obj.transform.SetParent(monsterListObject);

        monsters.Add(Monster);

        //Init monsters
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

    public void LoadAllMonsters()
    {
        //Load monsters
        monsterListObject = new GameObject("MonsterList").transform;
        GameObject[] monsterPrefabsList = Resources.LoadAll<GameObject>("Monsters/");

        for(int i = 0; i < monsterPrefabsList.Length; i++)
        {
            monsterPrefabs.Add(monsterPrefabsList[i].GetComponent<MonsterAI>().ID, monsterPrefabsList[i]);
        }

        Debug.Log(monsterPrefabsList.Length);
    }


}
