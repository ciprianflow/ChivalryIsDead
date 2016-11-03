using System.Collections.Generic;
using UnityEngine;


class AttackAction
{


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

        foreach(Collider collider in ConeCollider.CollidersInside)
        {
            if (collider.CompareTag("Enemy"))
            {
                collider.GetComponent<MonsterAI>().Hit(1);
            }
        }

    }

}
