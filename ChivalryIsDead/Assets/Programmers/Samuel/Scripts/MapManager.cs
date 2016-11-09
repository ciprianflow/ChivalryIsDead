using UnityEngine;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(AreaScript))]
public class MapManager : MonoBehaviour {

    ObjectiveManager OM;
    AreaScript areas;

    Transform QuestTarget;

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

    }

    void Start()
    {
        OM.LoadAllMonsters();
        InitQuest();
    }

    public void InitQuest()
    {
        if (QuestManager.currQuest == null)
        {
            QuestGenerator QG = new QuestGenerator();
            QuestManager.currQuest = (MultiQuest)QG.GenerateMultiQuest();
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

        OM.SpawnMonsters(ID, Vector3.zero, QuestTarget);
    }

    internal void CheckObjectives(IObjectiveTarget IObj)
    {

        QuestManager.currQuest.CheckTarget(OM.GetObjectives());
        if (QuestManager.currQuest.IsChecked)
        {
            Debug.LogWarning("Shits done!");
        }
        //foreach(IObjective iO in QuestManager.currQuest.Objectives)
        //{
        //    if (!iO.IsChecked && iO.CheckTarget(IObj))
        //    {
        //        return;
        //    }
        //}

    }
}