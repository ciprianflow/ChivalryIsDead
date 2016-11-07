using UnityEngine;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(AreaScript))]
public class MapManager : MonoBehaviour {

    MonsterManager MM;
    AreaScript areas;

    Transform QuestTarget;

    internal void SetQuestObject(Transform transform)
    {
        QuestTarget = transform;
    }

    void Awake()
    {
        areas = transform.GetComponent<AreaScript>();
        MM = new MonsterManager();

    }

    void Start()
    {
        MM.LoadAllMonsters();
        InitQuest();
    }

    public void InitQuest()
    {
        if (QuestManager.currQuest == null)
        {
            QuestGenerator QG = new QuestGenerator();
            QuestManager.currQuest = (BaseQuest)QG.GetQuest(Difficulty.Easy);
        }         

        List <IObjective> objectives = QuestManager.GetObjectives();

        for(int i = 0; i < objectives.Count; i++)
        {

            TranslateQuest(objectives[i]);

        }
    }

    void TranslateQuest(IObjective objective)
    {
        var ID = (objective as BaseObjective).targetID;
        MM.SpawnMonsters(ID, Vector3.zero, QuestTarget);
    }
}