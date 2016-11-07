using System.Collections.Generic;
using UnityEngine;


class AttackAction : MonoBehaviour
{

    public float AttackRange;
    public float AttackAngle;

    public void NormalAttack(float radius)
    {


     
    }


    //receives enemies to attack
    public void ConeAttack(List<Collider> colliders)
    {
        //Debug.Log("ENEMIES INSIDE: " + colliders.Count);

        foreach (Collider collider in colliders)
        {
            collider.GetComponent<MonsterAI>().Hit(1);

        }

    }


    public List<Collider> GetConeRange()
    {

        List<Collider> inRangeColliders = new List<Collider>();

        Collider[] colliders = Physics.OverlapSphere(transform.position, AttackRange);
        foreach(Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                
                Vector3 vectorToCollider = (collider.transform.position - transform.position).normalized;
                if (Vector3.Dot(vectorToCollider, transform.forward) > AttackAngle)
                {
                    inRangeColliders.Add(collider);
                }
            }
        }

        return inRangeColliders;
    }
}
