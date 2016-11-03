using System;
using UnityEngine;

public class RangedAI : MonsterAI {

    [Header("Ranged Specific Values")]
    public GameObject projectile;
    public GameObject targetSprite;

    Transform targetObj;

    float force = 1;
    float angle = 60;
    float randomRange = 5f;
    float randomAngle = 5f;

    public override void Init() { }

    public override void Attack()
    {
        rotateTowardsTarget();
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

        obj.transform.position = transform.position + new Vector3(0, 2f, 0);

        Rigidbody objRigidBody = obj.GetComponent<Rigidbody>();

        Vector3 random = new Vector3(UnityEngine.Random.Range(-randomRange, randomRange), 0, UnityEngine.Random.Range(-randomRange, randomRange));
        float randomAng = UnityEngine.Random.Range(-randomAngle, randomAngle);

        Vector3 randTargetPos = targetObject.position + random;
        Vector3 velocity = BallisticVel(randTargetPos, angle + randomAng) * force;

        //Target point calc
        //Target point calc
        float peakTime = velocity.y / Physics.gravity.y;
        float height = -0.5f * Physics.gravity.y * Mathf.Pow(peakTime, 2);

        Vector3 halfPoint = (randTargetPos - transform.position) / 2 + randTargetPos;
        Vector3 maxPoint = halfPoint + new Vector3(0, height, 0);
        Vector3 dir = (randTargetPos - maxPoint).normalized;
        //Target point calc
        //Target point calc

        //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //cube.transform.position = maxPoint;

        objRigidBody.velocity = velocity;

        RaycastHit hit;
        //if (Physics.Raycast(target.position + random + new Vector3(0, 99, 0), Vector3.down, out hit))
        if (Physics.Raycast(maxPoint, dir, out hit))
        {
            targetObj = Instantiate(targetSprite).transform;

            targetObj.position = hit.point + new Vector3(0, 0.5f, 0);
            targetObj.LookAt(hit.normal + targetObj.position);
            targetObj.Rotate(0, 0, 90);

            obj.transform.SetParent(targetObj);
            
        }
    }

    Vector3 BallisticVel(Vector3 target, float angle){
        Vector3 dir = target - transform.position;  // get target direction
        float h = dir.y;  // get height difference
        dir.y = 0;  // retain only the horizontal direction
        float dist = dir.magnitude;  // get horizontal distance
        float a = angle * Mathf.Deg2Rad;  // convert angle to radians
        dir.y = dist* Mathf.Tan(a);  // set dir to the elevation angle
        dist += h / Mathf.Tan(a);  // correct for small height differences
        // calculate the velocity magnitude
        float vel = Mathf.Sqrt(dist * Physics.gravity.magnitude / Mathf.Sin(2 * a));
        return vel* dir.normalized;
    }

    public override void Taunt() { }

}
