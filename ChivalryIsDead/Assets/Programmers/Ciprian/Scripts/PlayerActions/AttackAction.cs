using System.Collections.Generic;
using UnityEngine;


class AttackAction : MonoBehaviour
{

    public float AttackCooldown = 0.3f;
    private float cooldownTimeStamp = 0;

    private float attackRange;
    private float attackAngle;
    private int attackDamage;

    private PlayerScript playerBase;

    public float AttackAngle
    {
        get
        {
            return attackAngle;
        }

        set
        {
            attackAngle = value;
        }
    }

    public float AttackRange
    {
        get
        {
            return attackRange;
        }

        set
        {
            attackRange = value;
        }
    }

    public int AttackDamage
    {
        get
        {
            return attackDamage;
        }

        set
        {
            attackDamage = value;
        }
    }

    void Awake()
    {
        playerBase = GetComponent<PlayerScript>();
    }

    public void NormalAttack(float radius)
    {



    }


    //receives enemies to attack
    public List<Collider> ConeAttack()
    {
        List<Collider> colliders = new List<Collider>();
        //Debug.Log("ATTACK CAN: " + playerBase.canDoAction(PlayerActions.ATTACK));
        if (!getCoolDown() || !playerBase.canDoAction(PlayerActions.ATTACK))
        {
            //clear colliders if attack doesnt go through
            return colliders;
        }

        playerBase.attack();

        cooldownTimeStamp = Time.time + AttackCooldown;

        colliders = GetConeRange();

        foreach (Collider collider in colliders)
        {
            collider.GetComponent<MonsterAI>().Hit(attackDamage);
            //attack force
            collider.GetComponent<Rigidbody>().AddExplosionForce(100000f, transform.position, 5000f);
        }

        return colliders;

    }


    public List<Collider> GetConeRange()
    {

        List<Collider> inRangeColliders = new List<Collider>();

        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {

                Vector3 vectorToCollider = (collider.transform.position - transform.position).normalized;
                if (Vector3.Dot(vectorToCollider, transform.forward) > attackAngle)
                {
                    inRangeColliders.Add(collider);
                }
            }
        }

        return inRangeColliders;
    }

    //cooldown
    private bool getCoolDown()
    {
        if (cooldownTimeStamp >= Time.time)
        {
            Debug.Log("COOLDOWN");
            return false;
        }

        return true;
    }

    public float elapsedCooldown()
    {
        if ( cooldownTimeStamp < Time.time)
        {
            return 1;
        }

        return 1 - Mathf.Abs(((Time.time - cooldownTimeStamp)) /AttackCooldown) ;
    }
}
