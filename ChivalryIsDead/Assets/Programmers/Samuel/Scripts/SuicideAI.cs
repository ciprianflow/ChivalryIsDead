using UnityEngine;
using System.Collections;
using System;

public class SuicideAI : MonsterAI
{

    [Header("Suicide Specific Variables")]
    public float scaredTimer = 5f;
    [Space]
    public float explosionForce = 6000000f;
    public float explosionRange = 25f;
    public GameObject explosionObject;

    public float attackDamage = 30f;
    
    bool taunted = false;

    public override void Attack()
    {
        KillThis();
    }

    public override void Idle() { }

    public override void Init()
    {
    }

    public override void KillThis()
    {

        Debug.Log(transform.name + " : Has died");
        this.enabled = false;
        Explode();

    }

    public override void Move()
    {
        if (RangeCheckNavMesh())
            UpdateNavMeshPathDelayed();
        else
            MoveToAttack();
    }

    public override void Scare()
    {
        ToScared();
        StopNavMeshAgent();
        ResetTimer();
    }

    public override void Scared()
    {
        if(t1 > scaredTimer)
        {
            ResumeNavMeshAgent();
            ToMove();
        }
    }

    public override void Taunt()
    {
        Aggro();
        agent.speed *= 2;
        taunted = true;
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
        Destroy(this.gameObject);
        //Debug.LogError("ALLUH AKHBAR INFIDEL!!");
    }

    public override float GetBaseAttackDamage()
    {
        return attackDamage;
    }
}