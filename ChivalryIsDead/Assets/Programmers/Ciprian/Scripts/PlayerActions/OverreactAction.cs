using System;
using System.Collections.Generic;
using UnityEngine;

class OverreactAction: MonoBehaviour
{

    public int OverreactCooldown;
    private bool alreadyOverreact = false;

    public void Overreact()
    {

        //alreadyOverreact = true;
        
    }



    public void Update()
    {
        if (alreadyOverreact)
        {
            startOverreact();
        }

        float coolDownPeriodInSeconds = 2.5f;

        float timeStamp = Time.time + coolDownPeriodInSeconds;

    }


    private void startOverreact()
    {

        Debug.Log("Overreact");

    }
}

