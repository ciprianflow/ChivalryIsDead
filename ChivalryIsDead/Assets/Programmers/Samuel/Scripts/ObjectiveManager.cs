using UnityEngine;
using System.Collections.Generic;

public class ObjectiveManager {

    List<IObjectiveTarget> objectives = new List<IObjectiveTarget>();
    Dictionary<int, GameObject> monsterPrefabs = new Dictionary<int, GameObject>();

    Transform monsterListObject;

    /// <summary>
    /// Spawn a single monster at a certain location
    /// </summary>
    public void SpawnMonsters(int ID, Vector3 pos, Transform target)
    {
        Debug.Log(ID);
        GameObject obj = GameObject.Instantiate(monsterPrefabs[ID]);

        obj.transform.position = pos;

        MonsterAI Monster = obj.GetComponent<MonsterAI>();

        if (Monster.GetType() == typeof(RangedAI))
        {
            Monster.targetObject = target;
        }
        else
        {
            Monster.targetObject = StaticData.player;
        }

        Monster.InitMonster();
        obj.transform.SetParent(monsterListObject);     
        objectives.Add(Monster);

        //Init monsters
    }

    /// <summary>
    /// remove a monster with the index i, returns true if all monsters have been removed
    /// </summary>
    /// <param name="i"></param>
    public bool RemoveMonster(int i)
    {
        objectives.RemoveAt(i);
        if(objectives.Count <= 0)
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
    }

    public List<IObjectiveTarget> GetObjectives()
    {
        return objectives;
    }


}
