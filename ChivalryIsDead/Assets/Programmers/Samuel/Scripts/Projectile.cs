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

	void OnCollisionEnter(Collision col)
    {
        if (!hitGround && col.transform.CompareTag("Ground"))
            HitGround();

        if (setToDestroy || col.transform.CompareTag("Projectile") || originMonster == null  || col.gameObject == originMonster.transform.gameObject)
            return;

        ProjectileCollision(col.gameObject);
    }

    private void HitGround()
    {
        if (particles != null)
        {
            GameObject particlesObject = Instantiate(particles);
            particlesObject.transform.position = this.transform.position;
            hitGround = true;
        }

        MonsterAI.DoAOEAttack(transform.position, ExplosionRadius, ExplosionForce, originMonster);
    }

    void ProjectileCollision(GameObject collObj)
    {
        DestroyTarget();
        setToDestroy = true;

        QuestObject questObj = collObj.GetComponent<QuestObject>();
        if(questObj != null)
        {
            questObj.takeDamage(1, false);

            MonsterAI m = questObj.gameObject.GetComponent<MonsterAI>();
            if (m != null && m.GetType() == typeof(SheepAI))
            {
                //let player know objective is attacked
                originMonster.playerAction.SheepAttacked(originMonster);
            }
            else
            {
                //let player know objective is attacked
                originMonster.playerAction.ObjectiveAttacked(originMonster);
            }
        }

        //monster should make daamge not the projectile??
        if (collObj.CompareTag("Player"))
        {
            collObj.GetComponent<PlayerActionController>().PlayerAttacked(originMonster);
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
