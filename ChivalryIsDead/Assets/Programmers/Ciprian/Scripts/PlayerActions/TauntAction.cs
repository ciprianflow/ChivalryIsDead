﻿using System.Collections.Generic;
using UnityEngine;


class TauntAction: MonoBehaviour
{

    public float TauntDuration;
    public float TauntRadius;
    public float TauntCooldown = 5f;

    //used for cooldown i guess
    private bool alreadyTaunting = false;

    private float currentTauntRadius;
    private float currentTauntDuration;
    private float overTime;

    private float cooldownTimeStamp;

    void Start()
    {
        currentTauntRadius = TauntRadius;
    }

    void Update()
    {
        //Aggro
        /*
        if (getCoolDown())
        {
            startTaunt(currentTauntRadius, this.transform.position);
            //shrinkTauntArea();
        }
        */
    }

    private void startTaunt(float radius, Vector3 position)
    {
        cooldownTimeStamp = Time.time + TauntCooldown;

        //10 layer - Monster
        Collider[] hitColliders = Physics.OverlapSphere(position, radius);
        int i = 0;
        while (i < hitColliders.Length)
        {
            
            if (hitColliders[i].CompareTag("Enemy"))
            {
                checkStateAndTaunt(hitColliders[i].GetComponent<MonsterAI>());
            }

            i++;
        }
    }

    private void checkStateAndTaunt(MonsterAI monster)
    {
        //everything but idle
        if (monster.getState() != State.Idle || monster.GetType() == typeof(SuicideAI))
        {
            monster.Taunt();
        }
    }


    //TAUNT Action
    public void Taunt()
    {

        if (getCoolDown())
        {
            startTaunt(currentTauntRadius, this.transform.position);
            //shrinkTauntArea();
        }
        /*
        //just change aggro radius 
        if (!alreadyTaunting)
        {
            currentTauntDuration = TauntDuration;
            overTime = 0;
            alreadyTaunting = true;
        }
        */
    }

    //shrinkTauntArea
    private void shrinkTauntArea()
    {

        overTime += (0.01f / TauntDuration) * Time.deltaTime;

        currentTauntRadius = Mathf.Lerp(currentTauntRadius, TauntRadius, overTime);

        if (Mathf.Abs(currentTauntRadius - TauntRadius) < 0.1)
        {
            alreadyTaunting = false;
        }

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

}
