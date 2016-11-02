using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    bool setToDestroy = false;

	void OnCollisionEnter(Collision col)
    {
        if (setToDestroy || col.transform.CompareTag("Projectile"))
            return;

        ProjectileCollision();

    }

    void ProjectileCollision()
    {
        DestroyTarget();
        setToDestroy = true;
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
