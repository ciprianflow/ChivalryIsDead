using UnityEngine;
using System.Collections;
using System;

public class SuicideAI : MonsterAI
{
    public override void Attack()
    {
        KillThis();
    }

    public override void Idle()
    {
        if (agent.hasPath)
        {
            IdleToMove();
        }
    }

    public override void Init() { }

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
        throw new NotImplementedException();
    }

    public override void Scared()
    {
        throw new NotImplementedException();
    }

    public override void Taunt()
    {
        Aggro();
    }

    void Explode()
    {
        Rigidbody body = targetObject.transform.GetComponent<Rigidbody>();
        if (body)
            body.AddExplosionForce(6000000, transform.position - new Vector3(0, -5, 0), 25);
        Destroy(this.gameObject);
        Debug.LogError("ALLUH AKHBAR INFIDEL!!");
    }
}
