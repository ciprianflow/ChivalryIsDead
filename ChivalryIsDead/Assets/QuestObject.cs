using UnityEngine;
using System.Collections;
using System;

public class QuestObject : MonoBehaviour, IObjectiveTarget
{

    public int health = 2;

    HealthScript healthScript;
    //private PlayerBehaviour pb;
    // Use this for initialization
    void Awake()
    {

        //healthScript = transform.GetComponent<HealthScript>();
        healthScript = new HealthScript(health);
        transform.parent.GetComponent<MapManager>().SetQuestObject(this.transform);

        //pb = new PlayerBehaviour("rep");

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
    //need monster state too
    public void takeDamage(int dmg)
    {
        
        Debug.Log("IM hit ");


        if (healthScript.takeDamage(dmg))
        {   
            gameObject.SetActive(false);
            StaticIngameData.mapManager.CheckObjectives(this);
        }

        //add reputation
        //pb.ScoreChange -= dmg;
        //pb.Invoke();
    }
}
