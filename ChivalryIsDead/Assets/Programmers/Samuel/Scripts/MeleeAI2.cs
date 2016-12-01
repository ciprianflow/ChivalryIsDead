using UnityEngine;
using System;

public class MeleeAI2 : MonsterAI
{

    [Header("Melee Specific Values")]
    public float PreChargeTime = 1f;
    public float chargeSpeedMultiplier = 3f;
    public float attackLength = 2f;
    public float attackAngleWidth = 0.6f;
    public float chargeForce = 250f;
    public float attackForce = 5000;
    public float spinAttackDuration = 0.9f;
    public float spinAttackColddown = 3f;

    private float accelTimer = 0;
    private float accelTime = 0.2f;
    private float zVel = 0f;
    private float normSpeed;
    private float ChargeTurnTime = 0.5f;
    private float SpinAttackTimer = 0f;

    private bool rotated = false;
    private bool hitPlayer = false;
    private bool playerInRange = false;

    //Called one time at Awake()
    public override void Init()
    {
        normSpeed = agent.speed;
        targetPoint = GetRandomPointOnNavMesh();
        GameObject obj = new GameObject("AttackTrigger");
        obj.transform.SetParent(this.transform, false);
        SphereCollider col = obj.AddComponent<SphereCollider>();
        col.center = new Vector3(0, 1);
        col.radius = 2f;
        col.isTrigger = true;

    }

    //Called every frame in the attack state
    public override void Attack()
    {
        //Debug.Log(t1 + " > " + spinAttackDuration);
        if(t1 < spinAttackDuration)
        {
            if (!hitPlayer)
                if (DoAOEAttack(transform.position, attackLength, attackForce, this))
                    hitPlayer = true;
        }
        else
        {
            Debug.Log("IM DONE");
            AttackToMove();
        }

    }

    //Attack function, this function handles the calculations of an attack
    public bool MeleeAttack()
    {
        Collider[] Colliders = new Collider[0];
        Colliders = Physics.OverlapSphere(transform.position, attackLength);
        for (int i = 0; i < Colliders.Length; i++)
        {
            MonsterAI m = Colliders[i].GetComponent<MonsterAI>();

            if(m != null && m != this)
            {
                m.Hit(1);
                Rigidbody body = Colliders[i].transform.GetComponent<Rigidbody>();
                if (body)
                    body.AddExplosionForce(attackForce, transform.position, attackLength);
            }

            if (Colliders[i].tag == "Player")
            {
                //Debug.Log("This on is a player");
                Vector3 vectorToCollider = (Colliders[i].transform.position - transform.position).normalized;
                if (Vector3.Dot(vectorToCollider, transform.forward) > attackAngleWidth)
                {
                    Rigidbody body = Colliders[i].transform.GetComponent<Rigidbody>();
                    if (body)
                        body.AddExplosionForce(attackForce * 5, transform.position, attackLength);

                    base.playerAction.PlayerAttacked(this);
                    Debug.LogError("O NO THE PLAYR HAS HITEN BY MOnsTR");
                    return true;
                }
            }
        }
        return false;
        //Debug.Log("Attacking");
    }

    void ToSpinAttack()
    {
        hitPlayer = false;
        SpinAttackTimer = 0;
        ResetTimer();
        MoveToAttack();


        Quaternion q = Quaternion.LookRotation(targetObject.transform.position - transform.position);
        //Debug.Log((q.eulerAngles.y - transform.eulerAngles.y));

        if (((q.eulerAngles.y - transform.eulerAngles.y) > 0 && (q.eulerAngles.y - transform.eulerAngles.y) < 180) || (q.eulerAngles.y - transform.eulerAngles.y) < -180) {

            anim.SetTrigger("attackRight");
        }
        else {

            anim.SetTrigger("attackLeft");
        }


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

    bool TimedControlledRotation(float t)
    {
        //Safety for going above 1, even though it should never happen
        if(t > 1.1)
        {
            RotDone();
            return true;
        }

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
        RotDone();
        return true;
    }

    //Gets called when a charge is about to begin
    //after rotation is done but before the PreChargeTime
    private void RotDone()
    {
        ResetTimer();
        rotated = true;
    }

    //Called every frame in the Idle state
    public override void Idle()
    {
        //If the agent has a path go to the Move state again
        if (agent.hasPath)
        {
            IdleToMove();
        }

    }

    void FixedUpdate()
    {
        if (Mathf.Abs(zVel - (agent.velocity.magnitude)) < 0.05f)
        {
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

        SpinAttackTimer += Time.deltaTime;
        if (playerInRange && SpinAttackTimer > spinAttackColddown)
        {
            ToSpinAttack();
            return;
        }

        if (GetAngleToTarget() > 5)
            ToTurn();

        //Checks if the monster is in range of its target, returns true if it is not
        if (RangeCheckNavMesh())
            UpdateNavMeshPathDelayed(); //If the monster is not in range of his target yet update its path
        else
        {
            targetPoint = GetRandomPointOnNavMesh();
        }
    }


    //Called every time the monster is getting taunted
    public override void Taunt()
    {
        if (state == State.Death)
            return;

        //Plays Taunt sound
        WwiseInterface.Instance.PlayGeneralMonsterSound(MonsterHandle.Melee, MonsterAudioHandle.Taunted, this.gameObject);

        ToCharge();
    }

    //Called every time AI goes into move state
    public override void MoveEvent()
    {

        //Phillip do your animation stuff here

    }

    //This is not used any more
    //But we're still saving the function it, dont question it! k?
    public override void EnterUtilityState()
    {
        ToScared();
    }

    //This is not used any more
    //But we're still saving the function it, dont question it! k?
    public override void Utility()
    {
        t1 += Time.deltaTime;
        Vector3 p = transform.position - GetTargetPosition().normalized;
        targetPoint = p + transform.position;
        if (t1 > PreChargeTime)
        {
            ToMove();
        }
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
        NavMeshHit hit;
        while (true)
        {
            float walkRadius = UnityEngine.Random.Range(4, 8);
            Vector3 randomDirection = UnityEngine.Random.insideUnitCircle.normalized.normalized * walkRadius;
            randomDirection = new Vector3(randomDirection.x, 0, randomDirection.y);
            randomDirection += targetObject.position;
            NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
            if (hit.hit)
                break;
        }
        return hit.position;
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
        anim.SetTrigger("StartChargeMelee");
        agent.speed = normSpeed * chargeSpeedMultiplier; //Set speed of monster to charge speed
        state = State.Charge;
        stateFunc = Charge;

        //play charge sound
        WwiseInterface.Instance.PlayUniqueMeleeSound(UniqueMeleeAudioHandle.Charge, this.gameObject);
    }

    //Transistion from Charge to Attack satte
    public void ChargeToMove()
    {

        Debug.Log("ChargeToAttack");
        agent.speed = normSpeed;
        agent.velocity = Vector3.zero;
        ToMove();
        rotated = true;
    }

    //Charging collision
    void OnCollisionEnter(Collision coll)
    {
        //If the state is not charge do nothing
        if (state != State.Charge)
            return;

        
        MonsterAI m = coll.gameObject.GetComponent<MonsterAI>();
        //If collision happened with a quest object
        if (m != null)
        {
            //If AI hit a sheep invoke the sheep hit function
            QuestObject QO = coll.gameObject.GetComponent<QuestObject>();
            if (QO != null && m.GetType() == typeof(SheepAI))
            {
                Debug.Log("I HIT A SHEEP");
                HitSheep(QO, m, coll.gameObject, chargeForce * (accelTimer / accelTime), false, this);
            }
            //If its not a sheep it must be a static questObjective
            else if(QO != null)
            {
                Debug.Log("Hit static quest object");
                anim.SetTrigger("HitObject");
                QO.takeDamage(GetBaseAttackDamage(), true);
                base.playerAction.ObjectiveAttacked(this);
                ChargeToMove();
            }else
            {
                anim.SetTrigger("HitObject");
                Debug.Log("hit another monster");
                ChargeToMove();
            }
        }
        else {
            anim.SetTrigger("HitObject");
            Debug.Log("Hit wall");
            agent.velocity = Vector3.zero;
            ChargeToMove();
        }

        MeleeAttack();
    }

    //This function should trigger a turn animation //PHILIP
    //Trigger for player getting close to a monster, this triggers an attack
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (state == State.Move)
            {
                ToSpinAttack();
            }

            playerInRange = true;

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    public override void HitThis() { }

    void ToTurn()
    {

        StopNavMeshAgent();
        state = State.Turn;
        stateFunc = Turn;
        rotated = false;

        Quaternion q = Quaternion.LookRotation(targetPoint - transform.position);

        if (Mathf.Abs(q.eulerAngles.y - transform.eulerAngles.y) < 10)
            return;

        if (((q.eulerAngles.y - transform.eulerAngles.y) > 0 && (q.eulerAngles.y - transform.eulerAngles.y) < 180) || (q.eulerAngles.y - transform.eulerAngles.y) < -180) {
            Debug.Log("RIGHT");
            anim.SetTrigger("StartTurnRight");
        }
        else {
            Debug.Log("LEFT");

            anim.SetTrigger("StartTurnLeft");
        }
    }

    public override void Turn()
    {
        if ( ControlledRotation() )
        {
            ToMove();
        }


    }

    //Rotates the AI towards a point
    bool ControlledRotation()
    {
        Vector3 v = transform.forward;
        Vector3 v2 = transform.position;
        Vector3 v3 = agent.steeringTarget;
        v.y = 0;
        v2.y = 0;
        v3.y = 0;
        //If the rotation is not done yet the function returns false
        if (!rotated && Vector3.Angle(v, v3 - v2) < 20f)
        {
            anim.SetTrigger("Rotate");
            rotated = true;
        }

        if (Vector3.Angle(v, v3 - v2) > 1f)
        {
            RotateTowardsTarget(agent.steeringTarget);
            return false;
        }
        rotated = false;
        return true;
    }

    float GetAngleToTarget()
    {
        Vector3 v = transform.forward;
        Vector3 v2 = transform.position;
        Vector3 v3 = agent.steeringTarget;
        v.y = 0;
        v2.y = 0;
        v3.y = 0;
        //If the rotation is not done yet the function returns false
        return Vector3.Angle(v, v3 - v2);
    }
}