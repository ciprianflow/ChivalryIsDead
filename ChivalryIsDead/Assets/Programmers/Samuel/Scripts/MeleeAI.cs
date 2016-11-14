using UnityEngine;
using System;

public class MeleeAI : MonsterAI
{

    [Header("Melee Specific Values")]
    public float PreChargeTime = 1f;
    public float ChargeTurnTime = 0.5f;
    public float chargeSpeedMultiplier = 3f;

    public Animator anim;

    public float attackLength = 1f;
    public float attackAngleWidth = 0.6f;

    public float chargeForce = 250f;

    private float accelTimer = 0;
    private float accelTime = 0.2f;
    private float zVel = 0f;

    /// <summary>
    /// Storing the normal speed to change it back after charge is done
    /// </summary>
    private float normSpeed;

    /// <summary>
    /// If charge rotation is done
    /// </summary>
    private bool rotated = false;

    //Called one time at Awake()
    public override void Init()
    {

        normSpeed = agent.speed;
        targetPoint = GetRandomPointOnNavMesh();
    }

    //Called every frame in the attack state
    public override void Attack()
    {
        /* OBSOLETE CODE FOR TURNING
        //Rotate the AI towards its target
        rotateTowardsTarget();
        */

        if (!rotated && !ControlledRotation())
        {
            //If rotation is ongoing
            //chargeRotate resets timer so rotated makes sure that it does not enter the function again
            //if it has rotated
            return;
        }

        //If time is above attack time
        if (t1 > attackTime || patrolling)
        {
            //If the AI is in attack range of its target or it is in patrolling so it does not have a target
            if (RangeCheck() || patrolling)
           { 
                // TODO : do a melee attack here??? 
                // (ASK SAMUEL IF IN DOUBT IF YOU ARE, BCS HE IS??? BibleThump into SwiftRage)
                //Reset timers and go to move state
                ResetTimer();
                AttackToMove();
                return;
            }

            //Do an melee attack and reset timers
            MeleeAttack();
            ResetTimer();
        }

    }

    //Attack function, this function handles the calculations of an attack
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

                    base.playerAction.PlayerAttacked(this);
                    Debug.Log("Hit player");
                }
            }
        }

        Debug.Log("Attacking");

    }

    //Called every frame in the charge state
    public void Charge()
    {
        if (patrolling)
        {
            //Checks if rotation is the same as the target
            if (!rotated && !TimedControlledRotation(t1 / ChargeTurnTime))
                return;

            //Checks if PreChargeTime has been waited for
            if (t1 < PreChargeTime)
                return;

            //If the conditions has been met charges the AI forward with acceleration
            //AccelTime is the time it takes for the charge to reach MAXIMUM SPEED
            accelTimer += Time.deltaTime;
            float accel = accelTimer / accelTime;
            transform.Translate(Vector3.forward * chargeSpeedMultiplier * accel * Time.deltaTime);
        }
        else
        {
            UpdateNavMeshPathDelayed();
        }
    }

    //Rotates the AI towards a point
    bool ControlledRotation()
    {
        Vector3 v = transform.forward;
        Vector3 v2 = transform.position;
        Vector3 v3 = targetPoint;
        v.y = 0;
        v2.y = 0;
        v3.y = 0;
       //If the rotation is not done yet the function returns false
        if (Vector3.Angle(v, v3 - v2) > 1f)
        {
            RotateTowardsTarget();
            return false;
        }

        //If the rotation is done the function ruturns true
        ChargeRotDone();
        return true;
    }

    bool TimedControlledRotation(float t)
    {
        Vector3 v = transform.forward;
        Vector3 v2 = transform.position;
        Vector3 v3 = targetPoint;
        v.y = 0;
        v2.y = 0;
        v3.y = 0;
        //If the rotation is not done yet the function returns false
        if (Vector3.Angle(v, v3 - v2) > 1f)
        {
            RotateTowardsTargetTimed(t);
            return false;
        }

        //If the rotation is done the function ruturns true
        ChargeRotDone();
        return true;
    }

    //Gets called when a charge is about to begin
    //after rotation is done but before the PreChargeTime
    private void ChargeRotDone()
    {
        ResetTimer();
        rotated = true;
    }

    //Called every frame in the Idle state
    public override void Idle()
    {
        Debug.Log("IS IDLE");
        //If the agent has a path go to the Move state again
        if (agent.hasPath)
        {
            IdleToMove();
        }

    }

    void FixedUpdate()
    {
        //Debug.Log(agent.velocity.magnitude);


        if(Mathf.Abs(zVel - (agent.velocity.magnitude)) < 0.05f){
            return;
        }
        else if (zVel < (agent.velocity.magnitude))
        {
            zVel += 0.1f;
        }
        else
        {
            zVel -= 0.1f;
        }
        anim.SetFloat("Speed", zVel);
    }

    //Called every frame in the Move state
    public override void Move()
    {


        //Checks if the monster is in range of its target, returns true if it is not
        if (RangeCheckNavMesh())
            UpdateNavMeshPathDelayed(); //If the monster is not in range of his target yet update its path
        else //From move to Attack
        {
            MeleeMoveToAttack();
        }    
    }

    private void MeleeMoveToAttack()
    {
        MoveToAttack(); //If the monster is in range of its target go to the Attack state
        ResetTimer(); //Resets the attack timer
        rotated = false;

        //If in patrolling get a new point on the navmesh and set it as the next destination
        if (patrolling)
            targetPoint = GetRandomPointOnNavMesh();
    }

    //Called every time the monster is getting taunted
    public override void Taunt()
    {
        ToCharge();
    }

    //Transistion to the Charge state
    public void ToCharge()
    {
        //If AI is already in Charge do nothing
        if (state == State.Charge)
            return;

        //If AI is in patrolling mode charge towards a point
        if (patrolling)
        {
            //Point that the melee charges towards, checks for rotation is needed
            targetPoint = StaticIngameData.player.transform.position;
            rotated = false;
            accelTimer = 0;
            StopNavMeshAgent();
        }

        Debug.Log("ToCharge");
        initTimedRotation();
        ResetTimer();
        anim.SetTrigger("StartCharge");
        agent.speed = normSpeed * chargeSpeedMultiplier; //Set speed of monster to charge speed
        state = State.Charge;
        stateFunc = Charge;
    }

    //Transistion from Charge to Attack satte
    public void ChargeToAttack()
    {

        Debug.Log("ChargeToAttack");
        StopNavMeshAgent();
        agent.speed = normSpeed;
        agent.velocity = Vector3.zero;
        state = State.Attack;
        stateFunc = Attack;
        rotated = true;
    }

    //Called every time AI goes into move state
    public override void MoveEvent()
    {
        //Phillip do your animation stuff here

    }

    //This is not used any more
    //But we're still saving the function it, dont question it! k?
    public override void Scare()
    {
        ToScared();
    }

    //This is not used any more
    //But we're still saving the function it, dont question it! k?
    public override void Scared()
    {
        t1 += Time.deltaTime;
        Vector3 p = transform.position - GetTargetPosition().normalized;
        targetPoint = p + transform.position;
        if (t1 > PreChargeTime)
        {
            ToMove();
        }
    }

    //This is the function that makes the sheep go fly
    void HitSheep(QuestObject QO, MonsterAI m, GameObject g)
    {
        //Check the Quest Objective for nullpointer and if not make the sheep deed
        if (QO != null)
        {
            QO.takeDamage(999, false);
            base.playerAction.ObjectiveAttacked(this);
        }
        
        //sheep goes fly
        m.enabled = false;
        g.GetComponent<NavMeshAgent>().enabled = false;
        Rigidbody r = g.GetComponent<Rigidbody>();
        r.drag = 0;
        r.mass = 1;
        r.AddExplosionForce(chargeForce * (accelTimer / accelTime), g.transform.position, 100f, 1);
    }

    //This is some rep stuff
    public override int GetAttackReputation()
    {
        int rep = AttackRep;
        //this means taunted..
        if (state == State.Charge)
        {
            rep *= 2;
        }

        return rep;
    }

    //This is some rep stuff
    public override int GetObjectiveAttackReputation()
    {
        int rep = ObjectiveAttackRep;
        //this means taunted..
        if (state == State.Charge)
        {
            rep *= 2;
        }

        return rep;
    }

    //Kill this monster
    public override void KillThis()
    {
        Debug.Log(transform.name + " : Has died");
        this.enabled = false;
    }

    //Gets a random point on the navmesh
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

    //Charging collision
    void OnCollisionEnter(Collision coll)
    {
        //If the state is not charge do nothing
        if (state != State.Charge)
            return;

        QuestObject QO = coll.gameObject.GetComponent<QuestObject>();

        //If collision happened with a quest object
        if (QO != null)
        {
            //If AI hit a sheep invoke the sheep hit function
            MonsterAI m = coll.gameObject.GetComponent<MonsterAI>();
            if (m != null && m.GetType() == typeof(SheepAI))
            {
                Debug.Log("I HIT A SHEEP");
                HitSheep(QO, m, coll.gameObject);
                base.playerAction.SheepAttacked(this);
            }
            //If its not a sheep it must be a static questObjective
            else
            {
                Debug.Log("Hit static quest object");
                QO.takeDamage(GetBaseAttackDamage(), true);
                base.playerAction.ObjectiveAttacked(this);
                ChargeToAttack();
            }
        }
        else if (state == State.Charge)
        {
            ChargeToAttack();
        }
        anim.SetTrigger("HitObject");
    }
}