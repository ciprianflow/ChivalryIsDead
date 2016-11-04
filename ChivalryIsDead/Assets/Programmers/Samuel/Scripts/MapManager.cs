using UnityEngine;
using System.Collections.Generic;
using System;

public class MapManager : MonoBehaviour {

    MonsterManager MM;
    QuestManager QM;

    void Awake()
    {

        MM = new MonsterManager();
        QM = new QuestManager();

    }



}
