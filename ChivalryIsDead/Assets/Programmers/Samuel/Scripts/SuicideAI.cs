using UnityEngine;
using System.Collections;
using System;

public class SuicideAI : MonsterAI
{

    [Header("Suicide Specific Variables")]
    public float tauntTime = 5f;
    public float deSpawnRange = 8f;
    [Space]
    public float explosionForce = 750f;
    public float explosionForcePlayer = 15000f;
    public float explosionRange = 4f;
    public float explosionTriggerRange = 1f;
    public GameObject explosionObject;

    bool taunted = false;

    void Start()
    {
        if(PlayerPrefs.GetInt("SuicideLevel") == 0)
        {
            PlayerPrefs.SetInt("SuicideTut", 1);
            PlayerPrefs.SetInt("SuicideLevel", 1);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, explosionRange * 0.5f);
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
                if (!RangeCheck(deSpawnRange))
                    ToMove();
                else
                    MoveToIdle();
                taunted = false;
            }
        }
    }

    public override void Init()
    {
        GameObject obj = new GameObject("ExplosionTrigger");
        obj.transform.SetParent(this.transform, false);
        SphereCollider col = obj.AddComponent<SphereCollider>();
        col.center = new Vector3(0, 0);
        col.radius = explosionTriggerRange;
        col.isTrigger = true;
    }

    public override void KillThis()
    {

        Debug.Log(transform.name + " : Has died");
        Explode();

    }

    public override void Move() {

        if (RangeCheck(deSpawnRange))
        {
            MoveToIdle();
        }

        if (RangeCheckNavMesh()) {
            UpdateNavMeshPathDelayed();
        }else {
            MoveToAttack();
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
        if (state == State.Death)
            return;

        //Plays Taunt sound
        WwiseInterface.Instance.PlayGeneralMonsterSound(MonsterHandle.Suicide, MonsterAudioHandle.Taunted, this.gameObject);

        anim.SetTrigger("Taunted");
        taunted = true;
        ResetTimer();
        ToIdle();
    }

    void Explode()
    {
        Debug.Log("I'm exploding");

        if(explosionObject != null)
        {
            Instantiate(explosionObject, transform.position, Quaternion.identity);
        }

        //Checks for collision with different agents
        Collider[] Colliders = new Collider[0];
        Colliders = Physics.OverlapSphere(transform.position, explosionRange);
        for (int i = 0; i < Colliders.Length; i++)
        {
            QuestObject QO = Colliders[i].gameObject.GetComponent<QuestObject>();

            if (Colliders[i].tag == "Player")
            {
                //Player
                Debug.Log("This on is a player");
                Vector3 vectorToCollider = (Colliders[i].transform.position - transform.position).normalized;

                Rigidbody body = Colliders[i].transform.GetComponent<Rigidbody>();
                if (body)
                {
                    body.AddExplosionForce(explosionForcePlayer, transform.position, explosionRange, 1f);
                }
                    
                base.playerAction.PlayerAttacked(this);

            }else if(Colliders[i].tag == "Enemy")
            {
                MonsterAI m = Colliders[i].gameObject.GetComponent<MonsterAI>();

                if(m != null)
                {
                    if(m.GetType().Equals(typeof(SheepAI)))
                    {
                        //Sheeps
                        Debug.Log("I HIT A SHEEP");
                        HitSheep(QO, m, Colliders[i].gameObject, explosionForce, true, this);

                    }else
                    {
                        //Other monsters
                        m.Hit(1);
                        Rigidbody body = Colliders[i].transform.GetComponent<Rigidbody>();
                        if (body)
                        {
                            body.AddExplosionForce(explosionForcePlayer * 4, transform.position, explosionRange + 2, 0f);
                        }
                    }
                }
            }
            else
            {
                //Static object
                if (QO != null)
                {
                    Debug.Log("Hit static quest object");
                    QO.takeDamage(5, true);
                    base.playerAction.ObjectiveAttacked(this);
                }
            }
        }

        

        //Plays attack sound
        WwiseInterface.Instance.PlayGeneralMonsterSound(MonsterHandle.Suicide, MonsterAudioHandle.Attack, this.gameObject);
        base.Hit(99);
    }

    void OnTriggerEnter(Collider coll)
    {
        //Suicide should not be able to collide with other suicides
        MonsterAI m = coll.GetComponent<MonsterAI>();
        if (m != null && m.GetType().Equals(typeof(SuicideAI)))
            return;

        //EXPLODE WITH EVERYTHING
        if (!coll.CompareTag("Ground") && state != State.Idle || state == State.Utility)
            Explode();

        Debug.Log("Collided with something");

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

    void MoveToIdle()
    {
        state = State.Idle;
        stateFunc = Idle;
        StopNavMeshAgent();
        aggroed = false;
        anim.SetTrigger("Taunted");
    }

    public override void Turn()
    {
        throw new NotImplementedException();
    }
}