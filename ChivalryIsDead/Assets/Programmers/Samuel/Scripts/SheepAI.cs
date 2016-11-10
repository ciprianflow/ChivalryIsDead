using UnityEngine;
using System.Collections;
using System;

public class SheepAI : MonsterAI { 

    public override void Attack() { }

    public override void Idle()
    {
        if (RangeCheck())
        {
            IdleToMove();
        }
    }

    public override void Init()
    {
        Debug.Log("NOthing in Sheep Init");
    }

    public override void KillThis()
    {
        throw new NotImplementedException();
    }

    public override void Move()
    {
        if (RangeCheckNavMesh())
            UpdateNavMeshPathDelayed();
        else
            MoveToIdle();
    }

    public override void Taunt() { }

    public void MoveToIdle()
    {
        //Debug.Log("MoveToIdle");
        StopNavMeshAgent();
        state = State.Idle;
        stateFunc = Idle;
    }

    public override void Scared() {}

    public override void Scare() {}

}
