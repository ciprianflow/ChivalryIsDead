using UnityEngine;
using System.Collections;
using System;

public class TimerObjectScript : MonoBehaviour, IObjectiveTarget {

    int id = 31;

    float timer = 0;
    float maxTime = 10;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= maxTime)
            StaticData.mapManager.CheckObjectives(this);
    }

    public int ID
    {
        get
        {
            return id;
        }
    }

    public int Health
    {
        get
        {
            return (int)(maxTime - timer);
        }
    }

    public int MaxHealth
    {
        get
        {
            return (int)maxTime;
        }
    }

    public bool IsChecked
    {
        get ; set ;
    }

}
