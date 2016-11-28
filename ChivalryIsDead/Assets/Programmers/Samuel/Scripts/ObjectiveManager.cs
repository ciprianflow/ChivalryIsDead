using UnityEngine;
using System.Collections.Generic;

public class ObjectiveManager {

    List<IObjectiveTarget> objectives = new List<IObjectiveTarget>();
    Dictionary<int, GameObject> objectivePrefabs = new Dictionary<int, GameObject>();
    List<MonsterAI> monsters = new List<MonsterAI>();

    Transform objectiveListObject;

    /// <summary>
    /// Spawn a single monster at a certain location
    /// </summary>
    public void SpawnMonsters(int ID, Vector3 pos, Transform target)
    {
        GameObject obj = GameObject.Instantiate(objectivePrefabs[ID]);

        obj.transform.position = pos;

        IObjectiveTarget objective = obj.GetComponent<IObjectiveTarget>();
        MonsterAI monster = obj.GetComponent<MonsterAI>();

        if (monster != null)
            initMonster(monster, target);
        
        obj.transform.SetParent(objectiveListObject);     
        objectives.Add(objective);

        //Init monsters
    }

    void initMonster(MonsterAI Monster, Transform target)
    {
        if (Monster.GetType() == typeof(RangedAI))
        {
            Monster.targetObject = target;  
        }
        else
        {
            Monster.targetObject = StaticIngameData.player;
        }
        Monster.InitMonster();

        monsters.Add(Monster);

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

    public void LoadAllObjectives()
    {
        //Load monsters
        objectiveListObject = new GameObject("ObjectiveList").transform;
        GameObject[] objectivePrefabsList = Resources.LoadAll<GameObject>("Objectives/");

        for(int i = 0; i < objectivePrefabsList.Length; i++)
        {
            objectivePrefabs.Add(objectivePrefabsList[i].GetComponent<IObjectiveTarget>().ID, objectivePrefabsList[i]);
        }
    }

    public List<IObjectiveTarget> GetObjectives()
    {
        return objectives;
    }

    public List<MonsterAI> GetMonsters()
    {
        return monsters;
    }


}
