using System;
using System.Collections.Generic;
using UnityEngine;

class AggroAction : MonoBehaviour
{

    public float AggroRadius;


    void Update()
    {
        startAggro(AggroRadius, this.transform.position);
    }

    private void startAggro(float radius, Vector3 position)
    {

        //10 layer - Monster
        Collider[] hitColliders = Physics.OverlapSphere(position, radius);
        int i = 0;
        while (i < hitColliders.Length)
        {

            if (hitColliders[i].CompareTag("Enemy"))
            {
                checkStateAndAggro(hitColliders[i].GetComponent<MonsterAI>());
            }

            i++;
        }
    }

    private void checkStateAndAggro(MonsterAI monster)
    {
        if (monster.getState() == State.Idle)
        {
            monster.Aggro();
        }
    }
}