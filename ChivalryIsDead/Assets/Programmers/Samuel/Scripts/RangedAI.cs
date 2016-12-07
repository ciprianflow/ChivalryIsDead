using System;
using UnityEngine;

public class RangedAI : MonsterAI
{
    #region Overrides
    public new int AttackRep = -10;
    #endregion

    [Header("Ranged Specific Values")]
    public GameObject projectile;
    public GameObject targetSprite;
    public Sprite tauntedTarget;

    Transform targetObj;

    [Space]
    public float shootAngle = 60;
    public float randomShootRange = 4f;
    public float randomShootAngle = 20f;
    public float softAttackRangeBreak = 12;

    [Space]
    private bool taunted = false;


    public override void Init()
    {
    }

    public override void Attack()
    {
        RotateTowardsTarget();
        if (t1 > attackTime)
        {
            if (RangeCheck(softAttackRangeBreak))
            {
                AttackToMove();
                return;
            }

            anim.SetTrigger("PickUpRock");
            ResetTimer();
        }
    }

    public override void Move()
    {

        if (RangeCheck())
            UpdateNavMeshPathDelayed();
        else
            MoveToAttack();
    }

    public override void Idle() { }

    public void FireProjectTile(ref GameObject obj)
    {


        //GameObject obj = Instantiate(projectile);

        //Projectile p = projectile.GetComponent<Projectile>();
        //if (p != null)
        //    p.originMonster = this;
        //else
        //    Debug.Log("p doesnt exist");

        obj.GetComponent<Projectile>().originMonster = this;

        //obj.transform.position = transform.position + new Vector3(0, 3f, 0);

        Rigidbody objRigidBody = obj.GetComponent<Rigidbody>();

        Vector3 random = new Vector3(UnityEngine.Random.Range(-randomShootRange, randomShootRange), 0, UnityEngine.Random.Range(-randomShootRange, randomShootRange));
        float randomAng = UnityEngine.Random.Range(-randomShootAngle, randomShootAngle);

        Vector3 targetPos = GetTargetPosition();
        Vector3 velocity = Vector3.zero;

        if (targetSprite == null) {
            return;
        }

        targetObj = Instantiate(targetSprite).transform;
        //GameObject vrsdagrse = Instantiate(projectile);

        targetObj.name = "ROCKTARGET";

        if (taunted)
        {
            velocity = BallisticVel(targetPos, obj.transform.position, 10);
            targetObj.GetComponent<SpriteRenderer>().sprite = tauntedTarget;
            taunted = false;
        }
        else
        {
            targetPos += random;
            velocity = BallisticVel(targetPos, obj.transform.position, shootAngle + randomAng);
        }


        objRigidBody.velocity = velocity;
        objRigidBody.AddTorque(velocity * 5);



        anim.SetBool("Taunted", false);
        targetObj.position = targetPos;//hit.point + new Vector3(0, 0.5f, 0);
        targetObj.Rotate(0, 0, 90);

        obj.transform.SetParent(targetObj);

        //Plays attack sound
        WwiseInterface.Instance.PlayGeneralMonsterSound(MonsterHandle.Ranged, MonsterAudioHandle.Attack, this.gameObject);
    }

    Vector3 BallisticVel(Vector3 target, Vector3 pos, float initialAngle)
    {

        float gravity = Physics.gravity.magnitude;
        // Selected angle in radians
        float angle = initialAngle * Mathf.Deg2Rad;

        // Positions of this object and the target on the same plane
        Vector3 planarTarget = new Vector3(target.x, 0, target.z);
        Vector3 planarPostion = new Vector3(pos.x, 0, pos.z);

        // Planar distance between objects
        float distance = Vector3.Distance(planarTarget, planarPostion);
        // Distance along the y axis between objects
        float yOffset = pos.y - target.y;

        float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));

        Vector3 velocity = new Vector3(0, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

        // Rotate our velocity to match the direction between the two objects
        float angleBetweenObjects = Vector3.Angle(Vector3.forward, planarTarget - planarPostion) * (target.x > pos.x ? 1 : -1);
        return Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;

        /*
        Vector3 dir = target - pos;  // get target direction
        float h = dir.y;  // get height difference
        dir.y = 0;  // retain only the horizontal direction
        float dist = dir.magnitude;  // get horizontal distance
        float a = angle * Mathf.Deg2Rad;  // convert angle to radians
        dir.y = dist * Mathf.Tan(a);  // set dir to the elevation angle
        float tanA = Mathf.Tan(a);
        dist += h / tanA;  // correct for small height differences
        // calculate the velocity magnitude
        float vel = Mathf.Sqrt(Mathf.Abs(dist) * Physics.gravity.magnitude / Mathf.Sin(2 * a));
        return vel * dir.normalized;
    */
    }

    public override void Taunt() {
        //Plays Taunt sound
        WwiseInterface.Instance.PlayGeneralMonsterSound(MonsterHandle.Ranged, MonsterAudioHandle.Taunted, this.gameObject);
        
        taunted = true;
        anim.SetBool("Taunted", true);
    }

    public override void KillThis()
    {
        Debug.Log(transform.name + " : Has died");
        this.enabled = false;
    }

    public override void Utility()
    {
    }

    public override void EnterUtilityState()
    {
    }

    public override int GetAttackReputation()
    {
        int rep = AttackRep;
        //this means taunted..
        if (taunted)
        {
            rep *= 2;
        }

        return rep;
    }

    public override int GetObjectiveAttackReputation()
    {
        int rep = ObjectiveAttackRep;
        //this means taunted..
        if (taunted)
        {
            rep *= 2;
        }

        return rep;
    }


    public override void MoveEvent()
    {
        //Called every time AI goes into move state
    }

    public override void HitThis() { }

    public override void Turn()
    {
        throw new NotImplementedException();
    }
}
