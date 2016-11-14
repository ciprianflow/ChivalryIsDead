using System;
using System.Collections.Generic;
using UnityEngine;

class OverreactAction: MonoBehaviour
{

    public float OverreactCooldown;

    private float timeStamp;

    private PlayerScript playerBase;

    void Awake()
    {
        playerBase = GetComponent<PlayerScript>();
    }

    public bool Overreact()
    {
        //if not on cooldown do action
        if (getCoolDown())
        {
            startOverreact();

            return true;
        }

        return false;
    }

    //cooldown
    private bool getCoolDown()
    {
        if (timeStamp >= Time.time)
        {
            return false;
        }
        return true;
    }


    private void startOverreact()
    {
        //reset cooldown   
        timeStamp = Time.time + OverreactCooldown;
        playerBase.overreact();
        Debug.Log("Overreact");
    }
}

