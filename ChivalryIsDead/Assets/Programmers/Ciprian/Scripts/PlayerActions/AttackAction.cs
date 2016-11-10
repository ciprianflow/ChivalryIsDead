using System.Collections.Generic;
using UnityEngine;


class AttackAction : MonoBehaviour
{

    public float AttackCooldown = 1f;
    private float cooldownTimeStamp;

    private float attackRange;
    private float attackAngle;
    private float attackDamage;

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

    public float AttackDamage
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

    public void NormalAttack(float radius)
    {


     
    }


    //receives enemies to attack
    public void ConeAttack(List<Collider> colliders)
    {
        /*
        if(checkCooldown())
        {
            return;
        }
        */

        playerBase.attack();

        cooldownTimeStamp = Time.time + AttackCooldown;

        foreach (Collider collider in colliders)
        {
            collider.GetComponent<MonsterAI>().Hit(attackDamage);
            //attack force
            collider.GetComponent<Rigidbody>().AddExplosionForce(500000f, transform.position, 5000f);
        }

    }


    public List<Collider> GetConeRange()
    {

        List<Collider> inRangeColliders = new List<Collider>();

        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange);
        foreach(Collider collider in colliders)
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

    private bool checkCooldown()
    {

        if (!getCoolDown())
        {
            return false;
        }

        return true;
    }

    //cooldown
    private bool getCoolDown()
    {
        if (cooldownTimeStamp >= Time.time)
        {
            return false;
        }
        return true;
    }
}
