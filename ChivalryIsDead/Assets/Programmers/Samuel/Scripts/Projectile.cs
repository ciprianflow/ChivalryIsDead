using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    public MonsterAI originMonster;
    bool setToDestroy = false;

	void OnCollisionEnter(Collision col)
    {


        if (setToDestroy || col.transform.CompareTag("Projectile") || originMonster == null  || col.gameObject == originMonster.transform.gameObject)
            return;


        ProjectileCollision(col.gameObject);

    }

    void ProjectileCollision(GameObject collObj)
    {
        DestroyTarget();
        setToDestroy = true;

        QuestObject questObj = collObj.GetComponent<QuestObject>();
        if(questObj != null)
        {

            questObj.takeDamage(1, true);

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
