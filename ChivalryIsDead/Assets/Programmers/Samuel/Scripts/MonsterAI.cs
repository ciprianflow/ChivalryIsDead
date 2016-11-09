using UnityEngine;
using System.Collections;
using System;

public enum State { Attack, Move, Charge, Idle, Scared }

public abstract class MonsterAI : MonoBehaviour, IObjectiveTarget {

    #region fields

    public int id = 0;

    [Header("Defense Values")]
    public float Health = 2f;

    [Header("Attack Values")]
    public float attackDamage = 1f;
    public float attackTime = 3f;
    public float attackRange = 5f;

    [Space]
    public Transform targetObject;
    protected Vector3 targetPoint;
    public bool patrolling = false;

    public float attackRotateSpeed = 90f;
    private float pathUpdateTime = 0.1f;

    HealthScript healthScript;

    //Timers
    protected float t1 = 0;
    protected float t2 = 0;

    #endregion

    protected State state;
    protected Action stateFunc;

    public abstract void Attack();
    public abstract void Move();
    public abstract void Idle();
    public abstract void Taunt();
    public abstract void Scare();
    public abstract void Scared();
    public abstract void Init();

    public void InitMonster()
    {
        healthScript = new HealthScript((int)Health);
        InitNavMeshAgent();
        //ToMove(); //Comment in to make aggroed at start
        ToIdle(); //Comment in to Idle at start
        Init();
    }

    void Update()
    {
        stateFunc();
        updateTimer();
        UpdateNavMeshPathDelayed();

        //HARD CODED REMOVE LATER
        //HARD CODED REMOVE LATER
        //HARD CODED REMOVE LATER
        //HARD CODED REMOVE LATER
        //HARD CODED REMOVE LATER
        //HARD CODED REMOVE LATER
        //HARD CODED REMOVE LATER
        Debug.DrawLine(transform.position, targetPoint);
        if (targetObject != null && !targetObject.gameObject.activeSelf)
            targetObject = StaticData.player;
        //HARD CODED REMOVE LATER
        //HARD CODED REMOVE LATER
        //HARD CODED REMOVE LATER
        //HARD CODED REMOVE LATER
        //HARD CODED REMOVE LATER
        //HARD CODED REMOVE LATER
        //HARD CODED REMOVE LATER
    }

    #region Timers

    void updateTimer()
    {
        t1 += Time.deltaTime;
        t2 += Time.deltaTime;
    }

    protected void ResetTimer()
    {
        t1 = 0;
    }

    protected void ResetPathUpdateTimer()
    {
        t2 = 0;
    }

    #endregion

    #region State Transistions

    protected void ToIdle()
    {
        state = State.Idle;
        StopNavMeshAgent();
        stateFunc = Idle;
    }

    public void Aggro()
    {
        ToMove();
    }

    protected void ToScared()
    {
        Debug.Log("To Scared");
        ResetTimer();
        ResumeNavMeshAgent();
        state = State.Scared;
        stateFunc = Scared;
    }

    protected void ToMove()
    {
        //Debug.Log("ToMove");
        ResumeNavMeshAgent();
        state = State.Move;
        stateFunc = Move;
    }

    protected void MoveToAttack()
    {
        //Debug.Log("MoveToAttack");
        StopNavMeshAgent();
        state = State.Attack;
        stateFunc = Attack;
    }

    protected void AttackToMove()
    {
        //Debug.Log("AttackToMove");
        ResumeNavMeshAgent();
        agent.velocity = Vector3.zero;
        ToMove();
    }

    protected void IdleToMove()
    {
        //Debug.Log("IdleToMove");
        ToMove();
    }

    #endregion

    #region NavMesh

    //Navmesh Variables
    protected NavMeshAgent agent;
    private Transform[] points;
    private int destPoint = 0;

    Vector3 lastAgentVelocity;
    NavMeshPath lastAgentPath;
    Vector3 lastAgentDestination;

    void InitNavMeshAgent()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = true;
        points = new Transform[0];
        updateNavMeshPath();
    }

    protected void updateNavMeshPath()
    {
        agent.SetDestination(GetTargetPosition());
    }

    protected bool RangeCheckNavMesh()
    {
        if (agent.remainingDistance > attackRange)
            return true;
        return false;
    }

    protected bool RangeCheck()
    {
        float dist = Vector3.Distance(transform.position, GetTargetPosition());
        if (dist > attackRange)
            return true;
        return false;
    }

    protected void StopNavMeshAgent()
    {
        agent.velocity = Vector3.zero;
        agent.Stop();
        KillRigidBodyRotation();
    }

    protected void ResumeNavMeshAgent()
    {
        agent.Resume();
        updateNavMeshPath();
    }

    protected void UpdateNavMeshPathDelayed()
    {
        if(t2 > pathUpdateTime)
        {
            updateNavMeshPath();
            ResetPathUpdateTimer();
        }
    }

    #endregion

    #region Misc Functions

    protected void rotateTowardsTarget()
    {
        Quaternion q = Quaternion.LookRotation(GetTargetPosition() - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, q, attackRotateSpeed * Time.deltaTime);
    }

    private void KillRigidBodyRotation()
    {
        Rigidbody body = transform.GetComponent<Rigidbody>();
        if (body != null)
            body.angularVelocity = Vector3.zero;
    }

    public void TakeDamage(float num)
    {
        Health -= num;
        if(Health <= 0)
        {
            KillThis();
        }
    }

    public abstract void KillThis();

    public State getState()
    {
        return state;
    }

    protected Vector3 GetTargetPosition()
    {
        Vector3 p = targetObject.position;
        if (patrolling)
            p = targetPoint;
        return p;
    }

    public float GetBaseAttackDamage()
    {
        return attackDamage;
    }

    //implement this in the base class
    public void Hit(float damage)
    {
        if (healthScript.takeDamage((int)damage))
        {
            gameObject.SetActive(false);
            StaticData.mapManager.CheckObjectives(this);
        }
    }

    #endregion

    #region IObjective funtioncs

    public int ID
    {
        get
        {
            return id;
        }
    }

    int IObjectiveTarget.Health
    {
        get
        {
            return healthScript.getHealth();
        }
    }

    public int MaxHealth
    {
        get
        {
            return healthScript.getMaxhealth();
        }
    }

    public bool IsChecked { get; set; }
    #endregion
}
