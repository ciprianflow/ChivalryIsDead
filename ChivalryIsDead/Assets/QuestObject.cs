using UnityEngine;
using System.Collections;
using System;

public class QuestObject : MonoBehaviour, IObjectiveTarget
{

    HealthScript healthScript;

    // Use this for initialization
    void Awake()
    {

        //healthScript = transform.GetComponent<HealthScript>();
        healthScript = new HealthScript(2);
        transform.parent.GetComponent<MapManager>().SetQuestObject(this.transform);

    }

    public int Health
    {
        get
        {
            return healthScript.getHealth();
        }
    }

    public int ID
    {
        get
        {
            return 21;
        }
    }

    public int MaxHealth
    {
        get
        {
            return healthScript.getMaxhealth();
        }
    }

    public bool IsChecked { get; set; } 

    public void takeDamage(int dmg)
    {
        Debug.Log("IM hit ");
        if (healthScript.takeDamage(dmg))
        {
            gameObject.SetActive(false);
            StaticData.mapManager.CheckObjectives(this);
        }
    }
}
