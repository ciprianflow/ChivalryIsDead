using System.Collections.Generic;
using UnityEngine;


class TauntAction: MonoBehaviour
{

    public float TauntDuration;
    public float TauntRadius;

    //used for cooldown i guess
    private bool alreadyTaunting = false;

    private float currentTauntRadius;
    private float currentTauntDuration;
    private float overTime;

    void Start()
    {
        currentTauntRadius = TauntRadius;
    }

    void Update()
    {
        //Aggro
        if (alreadyTaunting)
        {
            startTaunt(currentTauntRadius, this.transform.position);
            shrinkTauntArea();
        }
    }

    private void startTaunt(float radius, Vector3 position)
    {

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
        if (monster.getState() != State.Idle)
        {
            monster.Taunt();
        }
    }


    //TAUNT Action
    public void Taunt()
    {
        //just change aggro radius 
        if (!alreadyTaunting)
        {
            currentTauntDuration = TauntDuration;
            overTime = 0;
            alreadyTaunting = true;
        }
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

}
