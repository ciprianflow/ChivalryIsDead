using System;
using System.Collections.Generic;
using UnityEngine;


class ScareAction : MonoBehaviour
{


    public void Scare(float radius)
    {

        //10 layer - Monster
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        int i = 0;
        while (i < hitColliders.Length)
        {

            if (hitColliders[i].CompareTag("Enemy"))
            {
                checkStateAndScare(hitColliders[i].GetComponent<MonsterAI>());
            }

            i++;
        }
    }

    private void checkStateAndScare(MonsterAI monster)
    {
        //if (monster.getState() != State.Charge)
        //{
        //monster.Scare();
        //}

        Debug.Log("SCARE MONSTER: " + monster.name);
    }

}

