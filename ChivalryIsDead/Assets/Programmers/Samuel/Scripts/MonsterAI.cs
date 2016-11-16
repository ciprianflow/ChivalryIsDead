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
    public int attackDamage = 2;
    public float attackTime = 3f;
    public float attackRange = 5f;

    [Space]
    public PlayerActionController playerAction;
    public Transform targetObject;
    protected Vector3 targetPoint;
    public bool patrolling = false;
    protected bool aggro = true;
    protected bool aggroed = false;

    public float attackRotateSpeed = 90f;
    private float pathUpdateTime = 0.1f;

    [Header("Reputation values")]
    //MONSTER ATTACk REP
    public int AttackRep = -20;
    public int OverreactRep = -60;
    public int ObjectiveAttackRep = -20;
    public int ObjectiveSheepRep = -100;
    //KNIGHT ATTACk REP
    public int PlayerAttackRep = 30;

    HealthScript healthScript;

    //Timers
    protected float t1 = 0;
    protected float t2 = 0;

    private Quaternion endRot;
    private Quaternion startRot;

    protected Animator anim;

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
    public abstract void MoveEvent();

    public abstract int GetObjectiveAttackReputation();
    public abstract int GetAttackReputation();

    public void InitMonster()
    {
        healthScript = new HealthScript((int)Health);
        InitNavMeshAgent();
        //ToMove(); //Comment in to make aggroed at start
        ToIdle(); //Comment in to Idle at start
        Init();
        anim = GetComponentInChildren<Animator>();
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
        Debug.DrawLine(transform.position, GetTargetPosition());
        if (targetObject != null && !targetObject.gameObject.activeSelf)
            targetObject = StaticIngameData.player;
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
        if (aggro && !aggroed)
        {
            ToMove();
            aggroed = true;
        }
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
        MoveEvent();
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
        if (targetObject == null)
            targetObject = StaticIngameData.player;
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

    protected float GetAngle(Vector3 targetPos)
    {
        Vector3 v = transform.forward;
        Vector3 v2 = transform.position;
        Vector3 v3 = targetPos;
        v.y = 0; v2.y = 0; v3.y = 0;
        return Vector3.Angle(v, v3 - v2);
    }

    protected void RotateTowardsTarget()
    {
        Quaternion q = Quaternion.LookRotation(GetTargetPosition() - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, q, attackRotateSpeed * Time.deltaTime);
    }

    protected void rotateTowardsTarget(Vector3 pos)
    {
        Quaternion q = Quaternion.LookRotation(pos - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, q, attackRotateSpeed * Time.deltaTime);
    }

    protected void RotateTowardsTargetTimed(float t)
    {
        transform.rotation = Quaternion.Slerp(startRot, endRot, t);
    }

    protected void initTimedRotation()
    {
        Vector3 v1 = GetTargetPosition();
        Vector3 v2 = transform.position;
        v1.y = 0;
        v2.y = 0;
        endRot = Quaternion.LookRotation(v1 - v2);
        startRot = transform.rotation;
    }

    private void KillRigidBodyRotation()
    {
        Rigidbody body = transform.GetComponent<Rigidbody>();
        if (body != null)
            body.angularVelocity = Vector3.zero;
    }



    public abstract void KillThis();

    public State getState()
    {
        return state;
    }

    protected Vector3 GetTargetPosition()
    {
        if (patrolling)
            return targetPoint;
        else
            return targetObject.position;
    }

    public int GetBaseAttackDamage()
    {
        return attackDamage;
    }

    public int PlayerAttackReputation()
    {
        return PlayerAttackRep;
    }

    public int GetOverreactReputation()
    {
        return OverreactRep;
    }

    public int GetSheepAttackReputation()
    {
        return ObjectiveSheepRep;
    }

    //implement this in the base class
    public void Hit(int damage)
    {

        if (healthScript.takeDamage(damage))
        {
            gameObject.SetActive(false);

            if(StaticIngameData.mapManager != null)
                StaticIngameData.mapManager.CheckObjectives(this);
        }

        anim.Play("TakeDamage");
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
