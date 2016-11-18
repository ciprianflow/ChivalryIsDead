using System;
using UnityEngine;

public class RangedAI : MonsterAI
{

    [Header("Ranged Specific Values")]
    public GameObject projectile;
    public GameObject targetSprite;

    Transform targetObj;

    float force = 1;
    [Space]
    public float shootAngle = 60;
    public float randomShootRange = 4f;
    public float randomShootAngle = 20f;

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
            if (RangeCheck())
            {
                AttackToMove();
                return;
            }

            FireProjectTile();
            ResetTimer();
        }
    }

    public override void Move()
    {
        bool b = RangeCheckNavMesh();
        if (b)
            UpdateNavMeshPathDelayed();
        else
            MoveToAttack();
    }

    public override void Idle() { }

    void FireProjectTile()
    {

        GameObject obj = Instantiate(projectile);

        Projectile p = projectile.GetComponent<Projectile>();
        if (p != null)
            p.originMonster = this;

        obj.transform.position = transform.position + new Vector3(0, 3f, 0);

        Rigidbody objRigidBody = obj.GetComponent<Rigidbody>();

        Vector3 random = new Vector3(UnityEngine.Random.Range(-randomShootRange, randomShootRange), 0, UnityEngine.Random.Range(-randomShootRange, randomShootRange));
        float randomAng = UnityEngine.Random.Range(-randomShootAngle, randomShootAngle);

        Vector3 randTargetPos = targetObject.position;
        Vector3 velocity = Vector3.zero;
        if (taunted)
        {
            velocity = BallisticVel(targetObject.position, shootAngle) * force;
            taunted = false;
        }
        else
        {
            randTargetPos += random;
            velocity = BallisticVel(randTargetPos, shootAngle + randomAng) * force;
        }


        objRigidBody.velocity = velocity;
        objRigidBody.AddTorque(velocity);

        if (targetSprite == null)
                return;

        targetObj = Instantiate(targetSprite).transform;

        targetObj.position = randTargetPos;//hit.point + new Vector3(0, 0.5f, 0);
        targetObj.Rotate(0, 0, 90);

        obj.transform.SetParent(targetObj);
    }

    Vector3 BallisticVel(Vector3 target, float angle)
    {
        Vector3 dir = target - transform.position;  // get target direction
        float h = dir.y;  // get height difference
        dir.y = 0;  // retain only the horizontal direction
        float dist = dir.magnitude;  // get horizontal distance
        float a = angle * Mathf.Deg2Rad;  // convert angle to radians
        dir.y = dist * Mathf.Tan(a);  // set dir to the elevation angle
        float tanA = Mathf.Tan(a);
        //if (tanA == 0)
            //tanA = 0.01f;
        dist += h / tanA;  // correct for small height differences
        // calculate the velocity magnitude
        float vel = Mathf.Sqrt(Mathf.Abs(dist) * Physics.gravity.magnitude / Mathf.Sin(2 * a));
        //Debug.Log(vel + " = " + Mathf.Abs(dist) * Physics.gravity.magnitude + " / " + Mathf.Sin(2 * a));
        //Debug.Log(Mathf.Sqrt(dist * Physics.gravity.magnitude / Mathf.Sin(2 * a)));
        return vel * dir.normalized;
    }

    public override void Taunt() { taunted = true; }

    public override void KillThis()
    {
        Debug.Log(transform.name + " : Has died");
        this.enabled = false;
    }

    public override void Scared()
    {
    }

    public override void Scare()
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
}
