using UnityEngine;
using System.Collections;
using System;

public enum State { Attack, Move, Charge, Idle }

public abstract class MonsterAI : MonoBehaviour, EnemyActions {

    protected float t1 = 0;
    protected float t2 = 0;

    [Header("Defense Values")]
    public float Health = 2f;

    [Header("Attack Values")]
    public float attackTime = 3f;
    public float attackRange = 5f;

    public float attackRotateSpeed = 90f;

    private float pathUpdateTime = 0.1f;

    public Transform targetObject;

    protected State state;
    protected Action stateFunc;

    public abstract void Attack();
    public abstract void Move();
    public abstract void Idle();
    public abstract void Taunt();

    public abstract void Init();

    void Awake()
    {
        InitNavMeshAgent();
        ToMove(); //Comment in to make aggroed at start
        //ToIdle(); //Comment in to Idle at start
        Init();
    }

    void Update()
    {
        stateFunc();
        updateTimer();
        UpdateNavMeshPath();
        
    }

    public void Taunted(Vector3 playerPosition)
    {
        throw new NotImplementedException();
    }

    //implement this in the base class
    public void Hit(int damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            gameObject.SetActive(false);
        }
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

    protected void ToMove()
    {
        Debug.Log("ToMove");
        ResumeNavMeshAgent();
        state = State.Move;
        stateFunc = Move;
    }

    protected void MoveToAttack()
    {
        Debug.Log("MoveToAttack");
        StopNavMeshAgent();
        state = State.Attack;
        stateFunc = Attack;
    }

    protected void AttackToMove()
    {
        Debug.Log("AttackToMove");
        ResumeNavMeshAgent();
        agent.velocity = Vector3.zero;
        ToMove();
    }

    protected void IdleToMove()
    {
        Debug.Log("IdleToMove");
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
        points = new Transform[0];
        GotoNextPoint();
        updateNavAgent();
    }

    void updateNavAgent()
    {
        agent.SetDestination(targetObject.position);
    }

    protected void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (points.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        agent.destination = points[destPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % points.Length;
    }

    protected bool RangeCheckNavMesh()
    {
        if (agent.remainingDistance > attackRange)
            return true;
        return false;
    }

    protected bool RangeCheck()
    {
        float dist = Vector3.Distance(transform.position, targetObject.position);
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
        updateNavAgent();
    }

    private void UpdateNavMeshPath()
    {
        if(t2 > pathUpdateTime)
        {
            updateNavAgent();
            ResetPathUpdateTimer();
        }
    }

    #endregion

    #region Helpers

    protected void rotateTowardsTarget()
    {
        Quaternion q = Quaternion.LookRotation(targetObject.position - transform.position);
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
            Debug.Log(transform.name + " : Has died");
            this.enabled = false;
        }
    }

    #endregion

}
