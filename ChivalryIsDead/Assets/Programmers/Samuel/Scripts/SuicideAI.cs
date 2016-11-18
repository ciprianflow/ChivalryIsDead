using UnityEngine;
using System.Collections;
using System;

public class SuicideAI : MonsterAI
{

    [Header("Suicide Specific Variables")]
    public float tauntTime = 5f;
    [Space]
    public float explosionForce = 750f;
    public float explosionRange = 4f;
    public float deAggroRange = 8f;
    public GameObject explosionObject;

    bool taunted = false;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, explosionRange);
    }

    public override void Attack()
    {
        KillThis();
    }

    public override void Idle()
    {
        
        if (taunted)
        {
            //Debug.Log(t1 + " > " + tauntTime);
            if(t1 > tauntTime)
            {
                Debug.Log("Un-taunt");
                ToMove();
                taunted = false;
            }
        }
    }

    public override void Init() { }

    public override void KillThis()
    {

        Debug.Log(transform.name + " : Has died");
        Explode();

    }

    public override void Move() {

        if (RangeCheckNavMesh()) {
            UpdateNavMeshPathDelayed();
        }
        else {
            MoveToAttack();
        }

        if(Vector3.Distance(transform.position, GetTargetPosition()) > deAggroRange){
            MoveToIdle();
        }
    }

    public override void EnterUtilityState()
    {
        stateFunc = Utility;
        state = State.Utility;
        StopNavMeshAgent();    
    }

    public override void Utility() {}

    public override void Taunt()
    {
        anim.SetTrigger("Taunted");
        taunted = true;
        ResetTimer();
        ToIdle();
    }

    public void MoveToIdle()
    {
        //Debug.Log("MoveToIdle");
        StopNavMeshAgent();
        state = State.Idle;
        stateFunc = Idle;
        anim.SetTrigger("Taunted");
        aggroed = false;
    }

    void Explode()
    {
        if(explosionObject != null)
        {
            Instantiate(explosionObject, transform.position, Quaternion.identity);
        }

        Collider[] Colliders = new Collider[0];
        Colliders = Physics.OverlapSphere(transform.position, explosionRange);
        for (int i = 0; i < Colliders.Length; i++)
        {
            if (Colliders[i].tag == "Player")
            {
                Debug.Log("This on is a player");
                Vector3 vectorToCollider = (Colliders[i].transform.position - transform.position).normalized;

                Rigidbody body = Colliders[i].transform.GetComponent<Rigidbody>();
                if (body)
                    body.AddExplosionForce(explosionForce, transform.position - new Vector3(0, -5, 0), explosionRange);

                base.playerAction.PlayerAttacked(this);
                Debug.Log("Hit player");

            }else if(Colliders[i].tag == "Enemy")
            {
                MonsterAI m = Colliders[i].gameObject.GetComponent<MonsterAI>();

                if(m != null)
                {
                    if(m.GetType().Equals(typeof(SheepAI)))
                    {

                        Debug.Log("I HIT A SHEEP");
                        QuestObject QO = Colliders[i].gameObject.GetComponent<QuestObject>();
                        HitSheep(QO, m, Colliders[i].gameObject, explosionForce, true);
                        base.playerAction.SheepAttacked(this);

                    }
                }
            }
        }

        //Debug.LogError("ALLUH AKHBAR INFIDEL!!");
        base.Hit(99);
    }

    void OnTriggerEnter(Collider coll)
    {
        Debug.Log("Collided with somehthing");
        //Debug.Log("Collided with something exploding");
        if (state == State.Utility)
            Explode();

    }

    public override int GetAttackReputation()
    {
        int rep = AttackRep;
        //this means taunted..
        if (taunted)
        {
            rep *= 2;
        }

        return rep;
    }

    public override int GetObjectiveAttackReputation()
    {
        int rep = ObjectiveAttackRep;
        //this means taunted..
        if (taunted)
        {
            rep *= 2;
        }

        return rep;
    }

    public override void MoveEvent()
    {
        //Called every time AI goes into move state
    }

    public override void HitThis()
    {

        //Called when monster is hit but not killed

        anim.Play("Suicide_Flying");

        if (this.gameObject.activeSelf)
        {
            EnterUtilityState();
            StartCoroutine(DelayedExplosion());
        }
    }

    IEnumerator DelayedExplosion()
    {

        yield return new WaitForSeconds(1f);
        Explode();


    }
}