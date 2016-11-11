using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public enum PlayerState
{
    IDLE, HIT, ATTACKING, TAUNTING
}


public class PlayerActionController : MonoBehaviour
{

    
    [Header("Attack values")]
    public int AttackDamage = 1;
    public float AttackRange = 2f;
    public float AttackAngle = 0.6f;
    

    [Header("Taunt values")]
    public float TauntRadius = 5f;
    public float TauntDuration = 3f;

    [Header("Aggro values")]
    public float AggroRadius = 4f;

    [Header("Overreact values")]
    //duration for the overreact mechanic
    public float AttackedDuration = 1.5f;
    public float OverreactCooldown = 2.5f;

    [Header("Scare values")]
    public float ScareRadius = 4f;
    

    private float attackRange = 35f;
    private float attackRadius = 120f;

    //used for overreacting
    private PlayerState playerState;

    private AggroAction aggroAction;
    private TauntAction tauntAction;
    private AttackAction attackAction;
    private OverreactAction overreactAction;
    private ScareAction scareAction;

   
    private PlayerBehaviour pb;
    private MonsterAI lastMonsterAttacked;

    void OnDrawGizmos()
    {

        Gizmos.color = Color.red;

        //cone attack radius
        Gizmos.color = Color.magenta;

        Gizmos.DrawRay(transform.position, Quaternion.AngleAxis(35f, transform.up) * transform.forward * 2f );
        Gizmos.DrawRay(transform.position, Quaternion.AngleAxis(-35f, transform.up) * transform.forward * 2f);


        //aggro radius
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(this.transform.position, AggroRadius);

    }

    void Awake()
    {

        //aggro taunt overreact
        gameObject.AddComponent<AggroAction>();
        gameObject.AddComponent<TauntAction>();
        gameObject.AddComponent<OverreactAction>();
        gameObject.AddComponent<AttackAction>();
        gameObject.AddComponent<ScareAction>();


        aggroAction = gameObject.GetComponent<AggroAction>();
        tauntAction = gameObject.GetComponent<TauntAction>();
        overreactAction = gameObject.GetComponent<OverreactAction>();
        attackAction = gameObject.GetComponent<AttackAction>();
        scareAction = gameObject.GetComponent<ScareAction>();

        StaticIngameData.playerAction = this;
    }

	// Use this for initialization
	void Start () {

        //player state
        playerState = PlayerState.IDLE;

        //init for taunt
        tauntAction.TauntDuration = TauntDuration;
        tauntAction.TauntRadius = TauntRadius;

        //init aggro
        aggroAction.AggroRadius = AggroRadius;

        //init for attack

        attackAction.AttackDamage = AttackDamage;
        attackAction.AttackAngle = AttackAngle;
        attackAction.AttackRange = AttackRange;

        //init for overreact
        overreactAction.OverreactCooldown = OverreactCooldown;

        //subscribe to the reputation system
        pb = new PlayerBehaviour("rep");

        pb.Reset();
        
    }

    /// <summary>
    /// Handle Taunt Button
    /// </summary>
    public void HandleTaunt()
    {

        //otherwhise taunt
        tauntAction.Taunt();
    }

    public void HandleOverreact()
    {
        // if attacked the player can overreact
        if (playerState == PlayerState.HIT)
        {
            //Player overreacted add reputation
            if (overreactAction.Overreact() && lastMonsterAttacked != null)
            {

                pb.ChangeRepScore(lastMonsterAttacked.GetOverreactReputation());
                //pb.Invoke();
            }
        }
    }


    //Attacks
    //REMOVE LOGIC FROM THIS CLASS!!
    public void HandleAttack()
    {
        //attackAction.NormalAttack(TauntRadius, this.transform);
        //if no enemies in range SCARE
        List<Collider> enemiesInRange = attackAction.GetConeRange();

        attackAction.ConeAttack(enemiesInRange);

        //add reputation
        foreach (Collider enemy in enemiesInRange)
        {
            MonsterAI monster = enemy.GetComponent<MonsterAI>();

            pb.ChangeRepScore(monster.PlayerAttackReputation());
            pb.Invoke();

        }

    }

    //objective attacked by monsters
    public void ObjectiveAttacked(MonsterAI monster)
    {
        Debug.Log("Objective attacked" + monster.name);

        pb.ChangeRepScore(monster.GetObjectiveAttackReputation());
        pb.Invoke();

    }

    //sheep attacked by monsters
    public void SheepAttacked(MonsterAI monster)
    {
        Debug.Log("Sheep attacked" + monster.name);

        pb.ChangeRepScore(monster.GetSheepAttackReputation());
        pb.Invoke();

    }

    
    //MONSTER ATTACKS PLAYER
    public void PlayerAttacked(MonsterAI monster)
    {
        //AttackedDuration second unlock overreact
        StartCoroutine(releaseAttacked());
        //can overreact
        playerState = PlayerState.HIT;

        //Player attacked add reputation according to monster base damage
        if (monster)
        {
            pb.ChangeRepScore(monster.GetAttackReputation());

            //save last monster attacked
            lastMonsterAttacked = monster;

            //pb.Invoke();
        }


    }


    private IEnumerator releaseAttacked()
    {
        yield return new WaitForSeconds(AttackedDuration);

        //wait for all attacks to submit change in reputation
        pb.Invoke();
        playerState = PlayerState.IDLE;
    }


}
