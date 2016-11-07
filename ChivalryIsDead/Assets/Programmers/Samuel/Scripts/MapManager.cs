using UnityEngine;
using System.Collections.Generic;
using System;

public class MapManager : MonoBehaviour {

    MonsterManager MM;

    //EDITOR VARIABLES
    [SerializeField]
    public List<Rect> SpawnAreas;
    [SerializeField]
    public List<Color> AreaColor = new List<Color>();

    void Awake()
    {

        MM = new MonsterManager();
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
        MM.SpawnMonsters(ID, Vector3.zero);
    }

    public void RemoveArea(int index)
    {
        if (index > SpawnAreas.Count - 1)
            return;

        SpawnAreas.RemoveAt(index);
    }

    public void AddArea()
    {
        Debug.Log("Adding SpawnArea");
        SpawnAreas.Add(new Rect(0, 0, 2f, 2f));
    }

    public void ResetAll()
    {
        SpawnAreas = new List<Rect>();
    }
}