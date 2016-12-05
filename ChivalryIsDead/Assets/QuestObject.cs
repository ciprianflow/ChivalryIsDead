using UnityEngine;
using System.Collections;
using System;

public class QuestObject : MonoBehaviour, IObjectiveTarget
{

    public int id = 22;
    public int health = 2;
    public bool ForceDestroyOnDeath = false;

    HealthScript healthScript;
    //private PlayerBehaviour pb;
    // Use this for initialization
    void Awake()
    {
        //healthScript = transform.GetComponent<HealthScript>();
        healthScript = new HealthScript(health);
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
            return id;
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
    public void takeDamage(int dmg, bool destroy)
    {
        
        Debug.Log("IM hit " + transform.name);

        if (healthScript.takeDamage(dmg))
        {   
            if(destroy || ForceDestroyOnDeath)
                gameObject.SetActive(false);

            Debug.Log("Quest Objective died");
            if(StaticIngameData.mapManager != null)
                StaticIngameData.mapManager.CheckObjectives(this);
        }

        //add reputation
        //pb.ScoreChange -= dmg;
        //pb.Invoke();
    }
}
