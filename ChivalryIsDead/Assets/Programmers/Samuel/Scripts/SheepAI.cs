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

    public override void Init() { }

    public override void KillThis() { }

    public override void Move()
    {
        if (RangeCheckNavMesh())
            UpdateNavMeshPathDelayed();
        else
            MoveToIdle();
    }

    public override void Taunt()
    {
        ToMove();
    }

    public void MoveToIdle()
    {
        //Debug.Log("MoveToIdle");
        StopNavMeshAgent();
        state = State.Idle;
        stateFunc = Idle;
    }

    public override void Scared() {}

    public override void Scare()
    {
        ToIdle();
    }

    public override int GetAttackReputation()
    {
        throw new NotImplementedException();
    }

    public override int GetObjectiveAttackReputation()
    {
        throw new NotImplementedException();
    }
}
