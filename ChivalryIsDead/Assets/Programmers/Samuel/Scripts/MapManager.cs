using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

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
        StaticIngameData.mapManager = this;

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

        if (StaticData.currQuest == null)
        {
            Debug.LogError("No current quest exists");
        }         

        List <IObjective> objectives = StaticData.GetObjectives();

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
        SceneManager.LoadScene("ProtoHubWorld 1");
    }
}