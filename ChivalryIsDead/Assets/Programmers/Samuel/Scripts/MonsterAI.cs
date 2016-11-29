using UnityEngine;
using System.Collections;
using System;

public enum State { Attack, Move, Charge, Idle, Utility, Death, Turn }

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

    public MonsterHandle monsterHandle;

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
    public abstract void Turn();
    public abstract void Taunt();
    public abstract void EnterUtilityState();
    public abstract void Utility();
    public abstract void Init();
    public abstract void MoveEvent();
    public abstract void HitThis();
    void Death() {}

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
        Debug.DrawLine(transform.position, transform.position + transform.forward * attackRange, Color.blue);
        if (targetObject != null && !targetObject.gameObject.activeSelf)
            targetObject = StaticIngameData.player;
        //HARD CODED REMOVE LATER
        //HARD CODED REMOVE LATER
        //HARD CODED REMOVE LATER
        //HARD CODED REMOVE LATER
        //HARD CODED REMOVE LATER
        //HARD CODED REMOVE LATER
        //HARD CODED REMOVE LATER

        //if (agent.angularSpeed > 3) {

        //}

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
        agent.velocity = Vector3.zero;
        StopNavMeshAgent();
        stateFunc = Idle;
        //if(anim != null)
        //anim.SetFloat("Speed", 0);
        //Debug.Log("TOIDLE");

    }

    public void Aggro()
    {
        if (aggro && !aggroed)
        {
            ToMove();
            aggroed = true;

            //Plays aggro sound
            WwiseInterface.Instance.PlayGeneralMonsterSound(monsterHandle, MonsterAudioHandle.Aggro, this.gameObject);
        }
    }

    protected void ToScared()
    {
        Debug.Log("To Scared");
        ResetTimer();
        ResumeNavMeshAgent();
        state = State.Utility;
        stateFunc = Utility;
    }

    protected void ToMove()
    {
        
        //Debug.Log("ToMove");
        MoveEvent();
        ResumeNavMeshAgent();
        state = State.Move;
        stateFunc = Move;

        if (monsterHandle != MonsterHandle.Ranged) {
            anim.SetTrigger("StartCharge");
        }
        anim.SetFloat("Speed", 1);


        //Plays move sound
        //WwiseInterface.Instance.PlayGeneralMonsterSound(monsterHandle, MonsterAudioHandle.Walk, this.gameObject);

    }

    protected void MoveToAttack()
    {
        //Debug.Log("MoveToAttack");

        StopNavMeshAgent();
        state = State.Attack;
        stateFunc = Attack;
        anim.SetFloat("Speed", 0);
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

    void ToDeath()
    {
        state = State.Death;
        stateFunc = Death;
        StopNavMeshAgent();
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
        if (agent.isOnNavMesh)
            agent.SetDestination(GetTargetPosition());
        else
            Debug.LogWarning("NavMesh Agent is not on a NavMesh!!");
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

    protected bool RangeCheck(float range)
    {
        float dist = Vector3.Distance(transform.position, GetTargetPosition());
        if (dist > range)
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
        if (this.name == "Ranged") {
            if((q.eulerAngles.y - transform.eulerAngles.y) > 5) {
                anim.SetBool("turnright", true);
                anim.SetBool("turnleft", false);
            }
            else if ((q.eulerAngles.y - transform.eulerAngles.y) < -5) {
                anim.SetBool("turnright", false);
                anim.SetBool("turnleft", true);
            }
            else {
                anim.SetBool("turnright", false);
                anim.SetBool("turnleft", false);
            }
        }

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

    public void Hit(int damage)
    {

        if (state == State.Death)
            return;

        if (healthScript.takeDamage(damage))
        {
            //Plays death sound
            WwiseInterface.Instance.PlayGeneralMonsterSound(monsterHandle, MonsterAudioHandle.Death, this.gameObject);

            //Updates the objective
            if (StaticIngameData.mapManager != null)
                StaticIngameData.mapManager.CheckObjectives(this);
            ToDeath();
            anim.Play("Death", 0, 0);
            if(this.GetType().Equals(typeof(SuicideAI)))
                gameObject.SetActive(false);
            else
                StartCoroutine(death());

            if (this.GetType().Equals(typeof(SheepAI))) {
                anim.SetTrigger("Flying");
            }

        }
        else {
            anim.Play("TakeDamage");
        }

        //Plays attacked sound
        WwiseInterface.Instance.PlayGeneralMonsterSound(monsterHandle, MonsterAudioHandle.Attacked, this.gameObject);
        HitThis();

    }

    IEnumerator death() {
        yield return new WaitForSeconds(5);
        gameObject.SetActive(false);
    }

    //This is the function that makes the sheep go fly
    public void HitSheep(QuestObject QO, MonsterAI m, GameObject g, float force, bool useMosnsterOrigin)
    {

        //Check the Quest Objective for nullpointer and if not make the sheep deed
        if (QO != null)
        {
            QO.takeDamage(999, false);
            playerAction.ObjectiveAttacked(this);
        }

        //sheep goes fly
        m.ToDeath();
        m.enabled = false;
        g.GetComponent<NavMeshAgent>().enabled = false;
        Rigidbody r = g.GetComponent<Rigidbody>();
        r.drag = 0;
        r.mass = 1;
        if(useMosnsterOrigin)
            r.AddExplosionForce(force, this.transform.position, 100f, 3);
        else
            r.AddExplosionForce(force, g.transform.position, 100f, 1);
        r.AddTorque((this.transform.position - g.transform.position) * 10);

        g.GetComponentInChildren<Animator>().SetTrigger("Flying");
        g.GetComponent<Sheep_flying>().flying = true;
    }

    public static bool DoAOEAttack(Vector3 pos, float radius, float force, MonsterAI Monster)
    {
        Collider[] Colliders = new Collider[0];
        Colliders = Physics.OverlapSphere(pos, radius);
        for (int i = 0; i < Colliders.Length; i++)
        {

            MonsterAI m = Colliders[i].GetComponent<MonsterAI>();
            if(m != null)
            {
                //Hit a sheep
                if (m.GetType().Equals(typeof(SheepAI)))
                {
                    Monster.HitSheep(m.GetComponent<QuestObject>(), m, m.gameObject, force, false);
                }
            }

            //Debug.Log("One in range");
            if (Colliders[i].tag == "Player")
            {
                //Debug.Log("This on is a player");
                Rigidbody body = Colliders[i].transform.GetComponent<Rigidbody>();
                if (body)
                    body.AddExplosionForce(force, pos, radius);


                PlayerActionController PAC = Colliders[i].gameObject.GetComponent<PlayerActionController>();
                PAC.PlayerAttacked(Monster);
                return true;
            }
        }
        return false;
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
