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

    public override void Init()
    {
        normSpeed = agent.speed;
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
                    base.targetObject.GetComponent<PlayerActionController>().Attacked();
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
            accelTimer += Time.deltaTime;
            float accel = accelTimer / accelTime;
            transform.Translate(Vector3.forward * chargeSpeedMultiplier * accel * Time.deltaTime);
        }
        else
        {
            UpdateNavMeshPathDelayed();
        }
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
        if(t1 > scaredTime)
        {
            ToMove();
        }
    }

    public Vector3 GetRandomPointOnNavMesh()
    {
        float walkRadius = UnityEngine.Random.Range(8, 16);
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * walkRadius;
        randomDirection += StaticData.player.transform.position;
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
        Debug.Log(name + "  Collided with something");
        MonsterAI m = coll.gameObject.GetComponent<MonsterAI>();
        if (m != null && m.GetType() == typeof(SheepAI) && state == State.Charge)
        {
            Debug.Log("I HIT A SHEEP");
            m.enabled = false;
            coll.gameObject.GetComponent<NavMeshAgent>().enabled = false;
            Rigidbody r = coll.gameObject.GetComponent<Rigidbody>();
            r.drag = 0;
            r.AddExplosionForce(chargeForce * (accelTimer / accelTime), coll.transform.position, 100f, 1);
        }
        else if (state == State.Charge)
        {
            ChargeToAttack();
        }
    }
}