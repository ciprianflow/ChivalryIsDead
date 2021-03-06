﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class PeasantLineScript : MonoBehaviour {

    public HubDataManager hubDataManager;

    public GameObject PeasantPrfab;
    public GameObject PeasantOutlinePrefab;
    public GameObject QuestMarkerPrefab;

    public int QuestMarkerHeight = 175;

    List<GameObject> Peasants = new List<GameObject>();

    public Sprite[] PeasantSprites;

    public void FillPeasantLine()
    {
        BezierSpline Bezier = transform.GetComponent<BezierSpline>();

        if (Bezier == null)
            return;

        int NumOfPeasants = (int)(20 * (StaticData.Reputation / 100)) + 1;
        //float PeasantDist = 1f / NumOfPeasants;
        float PeasantDist = 0.05f;

        float height = PeasantPrfab.GetComponent<Image>().rectTransform.sizeDelta.y / 2;

        float t = 0;
        for(int i = 0; i < NumOfPeasants; i++, t += PeasantDist)
        {
            Vector3 dir = Bezier.GetPoint(1 - t + 0.01f) - Bezier.GetPoint(1 - t - 0.01f);

            float RandX = Random.Range(0, 45);
            float RandY = Random.Range(0, 45);

            SpawnPeasant(Bezier.GetPoint(1 - t) + new Vector3(RandX, height + RandY, 0), dir);
        }

    }

    void SpawnPeasant(Vector3 pos, Vector3 dir)
    {
        GameObject PeasantObj = Instantiate(PeasantPrfab);

        if (dir.x > 0)
            PeasantObj.GetComponent<RectTransform>().rotation = new Quaternion(0, -180, 0, 1);
        PeasantObj.transform.SetParent(this.transform);
        PeasantObj.transform.position = pos;

        if(PeasantSprites != null)
        {
            PeasantObj.transform.GetChild(0).GetComponent<Image>().sprite = PeasantSprites[Random.Range(0, PeasantSprites.Length)];
        }

        Peasants.Add(PeasantObj);
    }

    public void PushQuestToPeasant(int PeasantIndex, int QuestIndex, BaseQuest quest)
    {

        Vector3 pos = Peasants[PeasantIndex].transform.position;
        Peasants[PeasantIndex].transform.GetComponent<Image>().enabled = true;
        Peasants[PeasantIndex].transform.GetComponent<colorLerp>().enabled = true;

        GameObject QuestMarkerObj = Instantiate(QuestMarkerPrefab);

        QuestMarkerObj.transform.position = pos + new Vector3(0, QuestMarkerHeight, 0);
        QuestMarkerObj.transform.SetParent(this.transform);

        PeasantQuest trigger = Peasants[PeasantIndex].AddComponent<PeasantQuest>();

        //hubDataManager.SelectQuest(QuestIndex)
        trigger.delegateOnClickEvent += 
            new PeasantQuest.DelegateOnClickEvent(hubDataManager.SetQuestLetter);
        trigger.delegateOnClickEvent2 +=
            new PeasantQuest.DelegateOnClickEvent(hubDataManager.setCurrSelectedQuest);
        trigger.QuestIndex = QuestIndex;

    }
}
