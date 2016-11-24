using System;
using System.Collections.Generic;
using UnityEngine;

class OverreactAction: MonoBehaviour
{

    public float OverreactCooldown;

    private float cooldownTimeStamp;

    private PlayerScript playerBase;

    void Awake()
    {
        playerBase = GetComponent<PlayerScript>();
    }

    public bool Overreact()
    {

       // Debug.Log("Overreact CAN: " + playerBase.canDoAction(PlayerActions.OVERREACT));
        //if not on cooldown do action
        if (getCoolDown() && playerBase.canDoAction(PlayerActions.OVERREACT))
        {
            startOverreact();

            return true;
        }

        return false;
    }

    //cooldown
    private bool getCoolDown()
    {
        if (cooldownTimeStamp >= Time.time)
        {
            return false;
        }
        return true;
    }


    private void startOverreact()
    {
        //reset cooldown 
        playerBase.overreact();
        cooldownTimeStamp = Time.time + OverreactCooldown;
        Debug.Log("Overreact");

        
    }

    public float elapsedCooldown()
    {
        if (cooldownTimeStamp < Time.time)
        {
            return 1;
        }

        return 1 - Mathf.Abs(((Time.time - cooldownTimeStamp)) / OverreactCooldown);
    }
}

