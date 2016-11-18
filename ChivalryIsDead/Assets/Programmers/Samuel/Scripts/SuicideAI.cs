using UnityEngine;
using System.Collections;
using System;

public class SuicideAI : MonsterAI
{

    [Header("Suicide Specific Variables")]
    public float tauntTime = 5f;
    [Space]
    public float explosionForce = 6000000f;
    public float explosionRange = 25f;
    public GameObject explosionObject;

    public Animator animObj;

    bool taunted = false;

    public override void Attack()
    {
        KillThis();
    }

    public override void Idle()
    {
        
        if (taunted)
        {
            //Debug.Log(t1 + " > " + tauntTime);
            if(t1 > tauntTime)
            {
                Debug.Log("Un-taunt");
                ToMove();
                taunted = false;
            }
        }
    }

    public override void Init() { }

    public override void KillThis()
    {

        Debug.Log(transform.name + " : Has died");
        Explode();

    }

    public override void Move() {

        if (RangeCheckNavMesh()) {
            UpdateNavMeshPathDelayed();
        }
        else {
            MoveToAttack();
        }
    }

    public override void Scare() {}
    public override void Scared() {}

    public override void Taunt()
    {
        animObj.SetTrigger("Taunted");
        taunted = true;
        ResetTimer();
        ToIdle();
    }

    void Explode()
    {
        if(explosionObject != null)
        {
            Instantiate(explosionObject, transform.position, Quaternion.identity);
        }

        int multiplyer = 1;
        if (taunted)
            multiplyer = 2;

        float range = explosionRange * multiplyer;

        base.targetObject.GetComponent<PlayerActionController>().PlayerAttacked(this);

        Rigidbody body = targetObject.transform.GetComponent<Rigidbody>();
        if (body)
            body.AddExplosionForce(explosionForce * multiplyer, transform.position - new Vector3(0, -5, 0), range);

        //Debug.LogError("ALLUH AKHBAR INFIDEL!!");
        Destroy(this.gameObject);
    }

    void OnCollisionEnter(Collision coll)
    {
        //Debug.Log("Collided with something exploding");
        if (state == State.Idle)
            return;

        KillThis();
    }

    public override int GetAttackReputation()
    {
        int rep = AttackRep;
        //this means taunted..
        if (taunted)
        {
            rep *= 2;
        }

        return rep;
    }

    public override int GetObjectiveAttackReputation()
    {
        int rep = ObjectiveAttackRep;
        //this means taunted..
        if (taunted)
        {
            rep *= 2;
        }

        return rep;
    }

    public override void MoveEvent()
    {
        //Called every time AI goes into move state
    }

}