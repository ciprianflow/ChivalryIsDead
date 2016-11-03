using System.Collections.Generic;
using UnityEngine;


class AttackAction : MonoBehaviour
{

    public float AttackRange;
    public float AttackAngle;

    public void NormalAttack(float radius, Transform transform)
    {


        RaycastHit hit;
        //loop all enemies...
        Vector3 rayDirection = transform.position - transform.position;

        float fieldOfViewDegrees = 180f;
        float visibilityDistance = 10f;//attackRange;

        //Debug.Log(Vector3.Angle(rayDirection, transform.forward));
        //Debug.Log(fieldOfViewDegrees);
        Debug.Log(Vector3.Angle(rayDirection, transform.forward));


        if ((Vector3.Angle(rayDirection, transform.forward)) <= fieldOfViewDegrees)
        {

            // Detect if player is within the field of view
            if (Physics.Raycast(transform.position, rayDirection, out hit, visibilityDistance))
            {

                //if (hitColliders[i].CompareTag("Enemy"))
                if(hit.transform.CompareTag("Enemy"))
                {
                    Debug.Log("HITTED");
                    Debug.Log(hit.transform.name);
                }
                
            }
        }


        Debug.Log("Attack");

    }



    public void ConeAttack()
    {
        //get enemies in range
        List<Collider> colliders = getConeRange();
        //Debug.Log("ENEMIES INSIDE: " + colliders.Count);

        foreach (Collider collider in colliders)
        {
            collider.GetComponent<MonsterAI>().Hit(1);
        }

    }


    private List<Collider> getConeRange()
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
