using UnityEngine;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(AreaScript))]
public class MapManager : MonoBehaviour {

    ObjectiveManager OM;
    AreaScript areas;

    Transform QuestTarget;

    Dictionary<int, List<Rect>> sortedAreas;

    internal void SetQuestObject(Transform transform)
    {
        QuestTarget = transform;
        QuestTarget.gameObject.SetActive(false);
    }

    void Awake()
    {
        areas = transform.GetComponent<AreaScript>();
        OM = new ObjectiveManager();
        StaticData.mapManager = this;

        //This functions gets all spawn areas in the map in a dictionary sorted based on monster ID
        sortedAreas = areas.GetSortedAreas();

    }

    void Start()
    {
        OM.LoadAllObjectives();
        InitQuest();
    }

    public void InitQuest()
    {
        // Find a way to pass QuestData to description generation.
        QuestData QD;

        if (QuestManager.currQuest == null)
        {
            QuestGenerator QG = new QuestGenerator();
            QuestManager.currQuest = (MultiQuest)QG.GenerateMultiQuest(out QD);
        }         

        List <IObjective> objectives = QuestManager.GetObjectives();

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
        if (ID == 21)
        {
            OM.GetObjectives().Add(QuestTarget.GetComponent<QuestObject>());
            QuestTarget.gameObject.SetActive(true);
            return;
        }

        OM.SpawnMonsters(ID, GetSpawnPoint(ID), QuestTarget);

    }

    Vector3 GetSpawnPoint(int ID)
    {
        if (sortedAreas.ContainsKey(ID) && sortedAreas[ID].Count > 0)
        {
            int index = (int)UnityEngine.Random.Range(0, sortedAreas[ID].Count);
            Debug.Log(sortedAreas[ID].Count);
            return areas.GetRandomPointOnRect(sortedAreas[ID][index]);
        }
        else

        Debug.Log("There does not seem to be an area for monster : " + ((monsterType)ID).ToString());
        return Vector3.zero;
    }

    internal void CheckObjectives(IObjectiveTarget IObj)
    {

        QuestManager.currQuest.CheckTarget(OM.GetObjectives());
        if (QuestManager.currQuest.IsChecked)
        {
            Debug.LogWarning("Shits done!");
        }

    }
}