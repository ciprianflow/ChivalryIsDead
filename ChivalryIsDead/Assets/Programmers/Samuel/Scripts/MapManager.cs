using UnityEngine;
using System.Collections.Generic;
using System;

public class MapManager : MonoBehaviour {

    MonsterManager MM;

    void Awake()
    {

        MM = new MonsterManager();
        InitQuest();

    }

    public void InitQuest()
    {
        if (QuestManager.currQuest == null)
            return;

        List <IObjective> objectives = QuestManager.GetObjectives();

        for(int i = 0; i < objectives.Count; i++)
        {

            TranslateQuest(objectives[i]);

        }
    }

    void TranslateQuest(IObjective objective)
    {

        var ID = (objective as BaseObjective).targetID;
        MM.SpawnMonsters(ID, Vector3.zero);

    }



}
