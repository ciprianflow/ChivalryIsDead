using UnityEngine;
using System.Collections;
using System;

public class Projectile : MonoBehaviour {

    public GameObject particles;
    public MonsterAI originMonster;

    bool setToDestroy = false;
    bool hitGround = false;

    public float ExplosionRadius = 1f;
    public float ExplosionForce = 500f;
    public float PlayerExplosionForce = 5000f;

    void OnCollisionEnter(Collision col)
    {
        if (!hitGround && col.transform.CompareTag("Ground"))
            HitGround();

        if (setToDestroy || col.transform.CompareTag("Projectile") || originMonster == null  || col.gameObject == originMonster.transform.gameObject)
            return;

        ProjectileCollision(col.gameObject, col.contacts[0].point);
    }

    private void HitGround()
    {
        if (particles != null)
        {
            GameObject particlesObject = Instantiate(particles);
            particlesObject.transform.position = this.transform.position;
            hitGround = true;
        }

        WwiseInterface.Instance.PlayCombatSound(CombatHandle.ImpactStone, this.gameObject);

        MonsterAI.DoAOEAttack(transform.position, ExplosionRadius, ExplosionForce, PlayerExplosionForce, originMonster);
    }

    void ProjectileCollision(GameObject collObj, Vector3 pos)
    {
        DestroyTarget();
        setToDestroy = true;
        
        MonsterAI m = collObj.gameObject.GetComponent<MonsterAI>();
        QuestObject questObj = collObj.GetComponent<QuestObject>();
        if (m != null)
        {

            //other monsters
            if (m.GetType() == typeof(SheepAI))
            {
                //let player know objective is attacked
                originMonster.HitSheep(m.GetComponent<QuestObject>(), m, m.gameObject, ExplosionForce, false, originMonster);
                return;
            }
            //other monsters
            else
            {
                m.Hit(1);
                Rigidbody body = collObj.GetComponent<Rigidbody>();
                if (body)
                {
                    body.AddExplosionForce(ExplosionForce, transform.position, ExplosionRadius, 0f);
                }
                originMonster.playerAction.MonsterAttackedMonster(originMonster);
            }
        }

        if (questObj != null)
        {
            Debug.Log("QuestObject hitttiittitiit");
                questObj.takeDamage(3, false, pos);
            //let player know objective is attacked
            originMonster.playerAction.ObjectiveAttacked(originMonster);
        }

        //monster should make daamge not the projectile??
        if (collObj.CompareTag("Player"))
        {
            collObj.GetComponent<PlayerActionController>().PlayerAttacked(originMonster);
            Rigidbody body = collObj.GetComponent<Rigidbody>();
            if (body)
            {
                body.AddExplosionForce(PlayerExplosionForce, transform.position, ExplosionRadius, 0f);
            }
        }
    }

    void DestroyTarget()
    {
        Transform target = transform.parent;
        transform.SetParent(null);
        if (target != null)
            Destroy(target.gameObject);
        StartCoroutine(DestroyThis(2f));
    }

    IEnumerator DestroyThis(float t)
    {
        yield return new WaitForSeconds(t);
        Destroy(this.gameObject);
    }
}
