using UnityEngine;
using System;

public class MeleeAI : MonsterAI
{

    [Header("Melee Specific Values")]
    public float scaredTime = 2f;
    public float chargeSpeedMultiplier = 3f;

    public float attackLength = 1f;
    public float attackAngleWidth = 0.6f;

    public float chargeForce = 250f;

    private float accelTimer = 0;
    private float accelTime = 0.2f;

    private float normSpeed;

    private bool rotated = false;

    public override void Init()
    {

        normSpeed = agent.speed;
        targetPoint = GetRandomPointOnNavMesh();
    }

    public override void Attack()
    {
        rotateTowardsTarget();
        if (t1 > attackTime)
        {

            if (RangeCheck() || patrolling)
            {
                if (patrolling)
                    targetPoint = GetRandomPointOnNavMesh();

                ResetTimer();
                AttackToMove();
                return;
            }

            MeleeAttack();
            ResetTimer();
        }

    }

    public void MeleeAttack()
    {
        Collider[] Colliders = new Collider[0];
        Colliders = Physics.OverlapSphere(transform.position, attackLength);
        for (int i = 0; i < Colliders.Length; i++)
        {
            if (Colliders[i].tag == "Player")
            {
                Vector3 vectorToCollider = (Colliders[i].transform.position - transform.position).normalized;
                Debug.Log(Vector3.Dot(vectorToCollider, transform.forward));
                if (Vector3.Dot(vectorToCollider, transform.forward) > attackAngleWidth)
                {
                    Rigidbody body = Colliders[i].transform.GetComponent<Rigidbody>();
                    if (body)
                        body.AddExplosionForce(100000, transform.position, attackLength);

                    //@@HARDCODED
                    base.targetObject.GetComponent<PlayerActionController>().PlayerAttacked(this);
                    Debug.Log("Hit player");
                }
            }
        }

        Debug.Log("Attacking");

    }

    public void Charge()
    {
        if (patrolling)
        {
            if(!rotated)
                if (!chargeRotate())
                    return;

            accelTimer += Time.deltaTime;
            float accel = accelTimer / accelTime;
            transform.Translate(Vector3.forward * chargeSpeedMultiplier * accel * Time.deltaTime);
        }
        else
        {
            UpdateNavMeshPathDelayed();
        }
    }

    bool chargeRotate()
    {
        Vector3 v = transform.forward;
        Vector3 v2 = transform.position;
        Vector3 v3 = targetPoint;
        v.y = 0;
        v2.y = 0;
        v3.y = 0;
        Debug.Log(Vector3.Angle(v, v3 - v2));
        if (Vector3.Angle(v, v3 - v2) > 1f)
        {
            rotateTowardsTarget();
            return false;
        }
        rotated = true;
        return true;
    }

    public override void Idle()
    {

        if (agent.hasPath)
        {
            IdleToMove();
        }

    }

    public override void Move()
    {
        if (RangeCheckNavMesh())
            UpdateNavMeshPathDelayed();
        else
            MoveToAttack();
    }

    public override void Taunt()
    {
        ToCharge();
    }

    public void ToCharge()
    {
        if (state == State.Charge)
            return;

        if (patrolling)
        {
            //Point that the melee charges towards, checks for rotation is needed
            targetPoint = StaticIngameData.player.transform.position;
            rotated = false;
            accelTimer = 0;
            StopNavMeshAgent();
        }


        Debug.Log("ToCharge");
        agent.speed = normSpeed * chargeSpeedMultiplier;
        state = State.Charge;
        stateFunc = Charge;
    }

    public void ChargeToAttack()
    {
        Debug.Log("ChargeToAttack");
        StopNavMeshAgent();
        agent.speed = normSpeed;
        agent.velocity = Vector3.zero;
        state = State.Attack;
        stateFunc = Attack;
    }

    public override void KillThis()
    {
        Debug.Log(transform.name + " : Has died");
        this.enabled = false;
    }

    public override void Scared()
    {
        t1 += Time.deltaTime;
        Vector3 p = transform.position - GetTargetPosition().normalized;
        targetPoint = p + transform.position;
        if (t1 > scaredTime)
        {
            ToMove();
        }
    }

    public Vector3 GetRandomPointOnNavMesh()
    {
        float walkRadius = UnityEngine.Random.Range(8, 16);
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * walkRadius;
        randomDirection += StaticIngameData.player.transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
        if (hit.hit)
            return hit.position;
        else
            return GetRandomPointOnNavMesh();
    }

    public override void Scare()
    {
        ToScared();
    }

    //Charging collision
    void OnCollisionEnter(Collision coll)
    {
        if (state != State.Charge)
            return;

        QuestObject QO = coll.gameObject.GetComponent<QuestObject>();
        
        if(QO != null)
        {
            //If thing hit is a sheep invoke the sheep hit function
            MonsterAI m = coll.gameObject.GetComponent<MonsterAI>();
            if (m != null && m.GetType() == typeof(SheepAI))
            {
                Debug.Log("I HIT A SHEEP");
                HitSheep(QO, m, coll.gameObject);
            }
            //If its not a sheep it must be a static questObjective
            else
            {
                Debug.Log("Hit static quest object");
                QO.takeDamage((int)GetBaseAttackDamage(), true);
                ChargeToAttack();
            }
        }   
        else if (state == State.Charge)
        {
            ChargeToAttack();
        }
    }

    void HitSheep(QuestObject QO, MonsterAI m, GameObject g)
    {
        if (QO != null)
            QO.takeDamage(999, false);
        m.enabled = false;
        g.GetComponent<NavMeshAgent>().enabled = false;
        Rigidbody r = g.GetComponent<Rigidbody>();
        r.drag = 0;
        r.mass = 1;
        r.AddExplosionForce(chargeForce * (accelTimer / accelTime), g.transform.position, 100f, 1);
    }
}