using System;
using System.Collections.Generic;
using UnityEngine;

class OverreactAction: MonoBehaviour
{

    public float OverreactCooldown;

    private float timeStamp;

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

        Debug.Log("Overreact");
    }
}

