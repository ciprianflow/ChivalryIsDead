using UnityEngine;
using System;

public class MeleeAI : MonsterAI
{

    [Header("Melee Specific Values")]
    public float PreChargeTime = 1f;
    public float chargeSpeedMultiplier = 3f;
    public float attackLength = 1f;
    public float attackAngleWidth = 0.6f;
    public float chargeForce = 250f;
    public float AttackForce = 5000;
    public float normalAttackColddown = 3f;

    private float accelTimer = 0;
    private float accelTime = 0.2f;
    private float zVel = 0f;
    private float normSpeed;
    private float attackDuration = 1f;
    private float ChargeTurnTime = 0.5f;
    private float normalAttackTimer = 0f;

    private bool rotated = false;
    private bool hasHit = false;
    private bool normalAttack = false;
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
        //Decides which attack to use
        if (normalAttack)
            NormalAttack(); //Normal attacks trigger when normalAttack == true
        else
            TurnAttack(); //This is default attack which happens at the end of a path

    }

    void NormalAttack()
    {
        //Debug.Log("I am in a normal attack state now");
        if (!rotated)
        {
            if (!TimedControlledRotation(t1 / ChargeTurnTime))
            {
                //Rotation not yet done
                return;
            }
            //Rotation done
        }
        if (t1 >= attackDuration)
        {
            //If the palyer is not in range any more go back to moving
            MeleeAttack();
            normalAttack = false;
            AttackToMove();
            ResetTimer();

            //Plays attack sound
            WwiseInterface.Instance.PlayGeneralMonsterSound(MonsterHandle.Melee, MonsterAudioHandle.Attack, this.gameObject);
        }

    }

    void TurnAttack()
    {
        if (!rotated)
        {
            if (!ControlledRotation())
            {
                //If rotation is ongoing
                //chargeRotate resets timer so rotated makes sure that it does not enter the function again
                //if it has rotated

                //Debug.Log("Turn");

                if(!hasHit)
                    if (MeleeAttack())
                        hasHit = true;
                return;
            }else
            {
                //Rotation is done
                rotated = true;
                hasHit = false;
                ResetTimer();

                //Plays attack sound
                WwiseInterface.Instance.PlayGeneralMonsterSound(MonsterHandle.Melee, MonsterAudioHandle.Attack, this.gameObject);
            }     
        }

        //If time is above attack time
        if (t1 > attackTime)// || patrolling)
        {
            // TODO : do a melee attack here??? 
            // (ASK SAMUEL IF IN DOUBT IF YOU ARE, BCS HE IS??? BibleThump into SwiftRage)
            //Reset timers and go to move state
            ResetTimer();
            AttackToMove();
            return;
        }
    }

    //Attack function, this function handles the calculations of an attack
    public bool MeleeAttack()
    {
        Collider[] Colliders = new Collider[0];
        Colliders = Physics.OverlapSphere(transform.position, attackLength);
        for (int i = 0; i < Colliders.Length; i++)
        {

            //Debug.Log(Colliders[i].gameObject.name);
            //Debug.Log("One in range");
            if (Colliders[i].tag == "Player")
            {
                Debug.DrawLine(transform.position + new Vector3(0, 1, 0), Colliders[i].transform.position + new Vector3(0, 1, 0), Color.red);
                //Debug.Log("This on is a player");
                Vector3 vectorToCollider = (Colliders[i].transform.position - transform.position).normalized;
                if (Vector3.Dot(vectorToCollider, transform.forward) > attackAngleWidth)
                {
                    Rigidbody body = Colliders[i].transform.GetComponent<Rigidbody>();
                    if (body)
                        body.AddExplosionForce(AttackForce * 5, transform.position, attackLength);

                    base.playerAction.PlayerAttacked(this);
                    Debug.LogError("O NO THE PLAYR HAS HITEN BY MOnsTR");
                    return true;
                }
            }
        }
        return false;
        //Debug.Log("Attacking");

    }

    void InitNormalAttack(Vector3 pos)
    {
        Vector3 targetPos = pos;
        targetPos.y = 0;
        targetPoint = targetPos;
        normalAttack = true;
        rotated = false;
        initTimedRotation();
        ResetTimer();
        normalAttackTimer = 0;
        MoveToAttack();
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
        if (t1 < 0.5f)
            return false;

        Vector3 v = transform.forward;
        Vector3 v2 = transform.position;
        Vector3 v3 = targetPoint;
        v.y = 0;
        v2.y = 0;
        v3.y = 0;
        //If the rotation is not done yet the function returns false
        if (Vector3.Angle(v, v3 - v2) < 20f) {
            anim.SetTrigger("Rotate");

        }

        if (Vector3.Angle(v, v3 - v2) > 1f)
        {
            RotateTowardsTarget();
            return false;
        }

        //If the rotation is done the function ruturns true
        RotDone();
        //anim.SetTrigger("Rotate");
        //anim.speed = 1f;
        return true;
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
        //Debug.Log(zVel);
        //Debug.Log(agent.velocity.magnitude);


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

        normalAttackTimer += Time.deltaTime;
        if (playerInRange && normalAttackTimer > normalAttackColddown)
        {
            InitNormalAttack(targetObject.position);
            return;
        }
            

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

        Quaternion q = Quaternion.LookRotation(targetPoint - transform.position);
        //Debug.Log((q.eulerAngles.y - transform.eulerAngles.y));
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, q, attackRotateSpeed * Time.deltaTime);
        //if (this.name == "Ranged") {
        //    if ((q.eulerAngles.y - transform.eulerAngles.y) > 5) {
        //        anim.SetBool("turnright", true);
        //        anim.SetBool("turnleft", false);
        //    }
        //    else if ((q.eulerAngles.y - transform.eulerAngles.y) < -5) {
        //        anim.SetBool("turnright", false);
        //        anim.SetBool("turnleft", true);
        //    }
        //    else {
        //        anim.SetBool("turnright", false);
        //        anim.SetBool("turnleft", false);
        //    }
        //}
        Vector3 v = transform.forward;
        Vector3 v2 = transform.position;
        Vector3 v3 = targetPoint;
        v.y = 0;
        v2.y = 0;
        v3.y = 0;

        if (((q.eulerAngles.y - transform.eulerAngles.y) > 0 && (q.eulerAngles.y - transform.eulerAngles.y) < 180) || (q.eulerAngles.y - transform.eulerAngles.y) < -180)  {
            //if (Vector3.Angle(v, v3 - v2) < 20f) {
            //    anim.SetTrigger("InstantRight");
            //    Debug.Log("InstantRight");
            //}
            //else {
            //    anim.SetTrigger("StartTurnRight");
            //}
            //Debug.Log("Right");
            anim.SetTrigger("StartTurnRight");

        }
        else {
            //if (Vector3.Angle(v, v3 - v2) < 20f) {
            //    anim.SetTrigger("InstantLeft");
            //    Debug.Log("InstantLeft");
            //}
            //else {
            //    anim.SetTrigger("StartTurnLeft");
            //}
            anim.SetTrigger("StartTurnLeft");

            //Debug.Log("Left");
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
            randomDirection += transform.position;
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
                QO.takeDamage(GetBaseAttackDamage(), true, coll.contacts[0].point);
                base.playerAction.ObjectiveAttacked(this);
                ChargeToMove();
            }else
            {
                anim.SetTrigger("HitObject");
                Debug.Log("hit another monster");
                ChargeToMove();
            }
        }
        else if (state == State.Charge) {
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
                InitNormalAttack(other.transform.position);
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

    public override void Turn()
    {
        throw new NotImplementedException();
    }
}