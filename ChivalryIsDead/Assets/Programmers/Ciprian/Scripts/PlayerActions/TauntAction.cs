using System.Collections.Generic;
using UnityEngine;


class TauntAction: MonoBehaviour
{

    public float AggroRadius;
    public float TauntDuration;
    public float TauntRadius;

    //used for cooldown i guess
    private bool taunted = false;

    private float currentAggroRadius;
    private float currentTauntDuration;
    private float overTime;

    void Start()
    {
        currentAggroRadius = AggroRadius;
    }

    void Update()
    {
        //Aggro
        StartTaunt(currentAggroRadius, this.transform.position);
        
        if (taunted)
        {
            shrinkTauntArea();
        }
    }

    public void StartTaunt(float radius, Vector3 position)
    {

        Collider[] hitColliders = Physics.OverlapSphere(position, radius);
        int i = 0;
        while (i < hitColliders.Length)
        {
            
            if (hitColliders[i].CompareTag("Enemy"))
            {
                hitColliders[i].GetComponent<MonsterAI>().Taunt();
            }
            
            //hitColliders[i].SendMessage("AddDamage");
            //Debug.Log(hitColliders[i].name);

            i++;
        }



    }



    //TAUNT Action
    public void Taunt()
    {
        //just change aggro radius 
        if (!taunted)
        {
            currentAggroRadius = AggroRadius + TauntRadius;
            currentTauntDuration = TauntDuration;
            overTime = 0;
            taunted = true;
        }
    }

    //shrinkTauntArea
    private void shrinkTauntArea()
    {

        overTime += (0.01f / TauntDuration) * Time.deltaTime;

        currentAggroRadius = Mathf.Lerp(currentAggroRadius, AggroRadius, overTime);

        if (Mathf.Abs(currentAggroRadius - AggroRadius) < 0.1)
        {
            taunted = false;
        }

    }

}
