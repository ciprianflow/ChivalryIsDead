using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using System.Linq;

[RequireComponent(typeof(AreaScript))]
public class MapManager : MonoBehaviour {

    ObjectiveManager OM;
    AreaScript areas;

    Transform QuestTarget;

    Dictionary<int, List<Rect>> sortedAreas;
    Dictionary<int, List<int>> sortedMaxSpawn;

    public GameObject endLetter;

    internal void SetQuestObject(Transform transform)
    {
        QuestTarget = transform;
        QuestTarget.gameObject.SetActive(false);
    }

    void Awake()
    {
        var staticProtectObjects = transform.Find("StaticProtectObjects");
        if (staticProtectObjects == null)
            return;
        else
            QuestTarget = staticProtectObjects.GetChild(0);

        areas = transform.GetComponent<AreaScript>();
        OM = new ObjectiveManager();
        StaticIngameData.mapManager = this;

        //This functions gets all spawn areas in the map in a dictionary sorted based on monster ID
        sortedAreas = areas.GetSortedAreas();
        sortedMaxSpawn = areas.GetSortedMaxSpawn();

    }

    void Start()
    {
        OM.LoadAllObjectives();
        InitQuest();
    }

    public void InitQuest()
    {

        if (StaticData.currQuest == null)
        {
            Debug.LogWarning("No current quest exists");
        }         

        List <IObjective> objectives = StaticData.GetObjectives();

        if (objectives == null)
            return;

        for(int i = 0; i < objectives.Count; i++)
        {

            TranslateQuest(objectives[i]);

        }
    }

    void TranslateQuest(IObjective objective)
    {
        var ID = 0;
        if (objective.GetType() == typeof(BaseQuest))
        {
            foreach (IObjective io in (objective as BaseQuest).Objectives)
            {
                TranslateQuest(io);
            }
        } else
        {
            ID = (objective as BaseObjective).targetID;
        }

        if (ID == 0) return;

        //NEED TO CHANGE THIS APROACH
        if (ID == 22)
        {
            OM.GetObjectives().Add(QuestTarget.GetComponent<QuestObject>());
            QuestTarget.gameObject.SetActive(true);
            return;
        }

        Transform target = QuestTarget;
        if (target == null)
            target = StaticIngameData.player;

        OM.SpawnMonsters(ID, GetSpawnPoint(ID), target);

    }

    Vector3 GetSpawnPoint(int ID)
    {
        if (sortedAreas.ContainsKey(ID) && sortedAreas[ID].Count > 0)
        {
            //Loops through the spawn areas for that particular monster type at random
            System.Random r = new System.Random();
            foreach (int index in Enumerable.Range(0, sortedAreas[ID].Count).OrderBy(x => r.Next()))
            {
                Debug.Log(index + " INDEX");
                Debug.Log(sortedMaxSpawn[ID][index] + " COUNT");
                //If the amount of spawns left is above zero it-sa go (Why dont you like me?, i'ma sorrie green mario)'
                //If the amount of spawns is below zero its unlimited spawna timaeee.....
                if(sortedMaxSpawn[ID][index] != 0)
                {
                    Debug.Log("SPAWNING OBJECTIVE");
                    sortedMaxSpawn[ID][index]--; //Subtracts one from the amount of monsters able to spawn
                    return areas.GetRandomPointOnRect(sortedAreas[ID][index]); //Return a random point inside the spawn rect
                }          
            }
        }

        //If no suitable spawn area for a monster was found spawn in 0,0
        Debug.Log("There does not seem to be an area for monster : " + ((monsterType)ID).ToString());
        return Vector3.zero;
    }

    internal void CheckObjectives(IObjectiveTarget IObj)
    {
        if (StaticData.currQuest == null)
        {
            Debug.LogWarning("No quest could be found");
            return;
        }

        StaticData.currQuest.CheckTarget(IObj, OM.GetObjectives());
        if (StaticData.currQuest.IsChecked)
        {
            Debug.LogWarning("Shits done!");
            EndQuest();
        }

    }

    private void EndQuest()
    {
        //SET THE DAYS LEFT
        StaticData.daysLeft--;

        //CALC SCORE
        float localRepGain = StaticIngameData.dummyManager.GetGlobalScore();
        StaticData.Reputation += localRepGain;

        //Load Quest Hub Manager
        if (endLetter != null)
        {
            endLetter.SetActive(true);
            Time.timeScale = 0.1f;
        }
        else
            LoadHubArea();
    }

    public void LoadHubArea()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("ProtoHubWorld 1"); //TODO: Change to string based loading
    }

    public void CreateQuestTypeObjects()
    {

        List<string> QuestTypes = Enum.GetNames(typeof(QuestType)).ToList();

        GameObject list;
        for (int i = 0; i < QuestTypes.Count; i++)
        {
            if (transform.FindChild(QuestTypes[i]) != null)
                continue;

            list = new GameObject(QuestTypes[i]);
            list.transform.SetParent(this.transform);
        }

        if (transform.FindChild("StaticProtectObjects") != null)
            return;

        list = new GameObject("StaticProtectObjects");
        list.transform.SetParent(this.transform);

    }
}