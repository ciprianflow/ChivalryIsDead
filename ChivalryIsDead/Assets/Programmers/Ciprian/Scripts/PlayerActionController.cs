using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public enum PlayerState
{
    IDLE, HIT, ATTACKING, TAUNTING
}

public enum PlayerActions
{
    ATTACK, OVERREACT, TAUNT
};

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


    [Header("Cooldown values")]
    public float AttackCooldown = 0.3f;
    public float OverreactCooldown = 2.5f;
    public float TauntCooldown = 5f;

    [Header("Other stuff")]
    public GameObject RepGainParticle;
    public GameObject RepLossParticle;
    public GameObject ComboParticle;
    public GameObject ComboUpwardParticle;


    private float attackRange = 35f;
    private float attackRadius = 120f;

    //used for overreacting
    private PlayerState playerState;
    private float overreactTimestamp;

    private AggroAction aggroAction;
    private TauntAction tauntAction;
    private AttackAction attackAction;
    private OverreactAction overreactAction;

   
    private PlayerBehaviour pb;
    private MonsterAI lastMonsterAttacked;

    void OnDrawGizmos()
    {

        Gizmos.color = Color.red;

        //cone attack radius
        Gizmos.color = Color.magenta;

        Gizmos.DrawRay(transform.position, Quaternion.AngleAxis(52.5f, transform.up) * transform.forward * 3f );
        Gizmos.DrawRay(transform.position, Quaternion.AngleAxis(-52.5f, transform.up) * transform.forward * 3f);


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


        aggroAction = gameObject.GetComponent<AggroAction>();
        tauntAction = gameObject.GetComponent<TauntAction>();
        overreactAction = gameObject.GetComponent<OverreactAction>();
        attackAction = gameObject.GetComponent<AttackAction>();

        StaticIngameData.playerAction = this;
    }

	// Use this for initialization
	void Start () {

        //subscribe to the reputation system
        pb = new PlayerBehaviour("rep");

        pb.RepGainParticle = RepGainParticle;
        pb.RepLossParticle = RepLossParticle;
        pb.ComboBaseParticle = ComboParticle;
        pb.ComboUpwardParticle = ComboUpwardParticle;

        pb.Reset();

       
        //ComboParticle.GetComponent<Particle>().Play();

        //player state
        playerState = PlayerState.IDLE;

        //init for taunt
        tauntAction.TauntDuration = TauntDuration;
        tauntAction.TauntRadius = TauntRadius;
        tauntAction.TauntCooldown = TauntCooldown;

        //init aggro
        aggroAction.AggroRadius = AggroRadius;

        //init for attack

        attackAction.AttackDamage = AttackDamage;
        attackAction.AttackAngle = AttackAngle;
        attackAction.AttackRange = AttackRange;
        attackAction.AttackCooldown = AttackCooldown;

        //init for overreact
        overreactAction.OverreactCooldown = OverreactCooldown;

        
    }

    void Update()
    {

        overreactTimestamp += Time.deltaTime;

    }


    /// <summary>
    /// Handle Taunt Button
    /// </summary>
    public void HandleTaunt()
    {

        //otherwhise taunt
        tauntAction.Taunt();
    }

    //REFACTOR ACTIONS
    public void HandleOverreact()
    {
        
        //Player overreacted checks cooldown from the action
        if (overreactAction.Overreact()) {

            // if attacked the player can receive points based on time
            if (playerState == PlayerState.HIT && lastMonsterAttacked != null) {

                
                //pb.ChangeRepScore((int)((AttackedDuration - overreactTimestamp) * -10));
                int points = (int)((AttackedDuration - overreactTimestamp) * 10);

                //@@HARDCODED
                if (points > 30)
                    WwiseInterface.Instance.PlayKnightCombatVoiceSound(KnightCombatVoiceHandle.OverreactPerfect, this.gameObject);
                else
                    WwiseInterface.Instance.PlayKnightCombatVoiceSound(KnightCombatVoiceHandle.OverreactGreat, this.gameObject);

                Debug.Log("Overreact points:" + -points);
                pb.ChangeRepScore(-points);

                //pb.Invoke();
                //change player state to IDLE after overreacting
                playerState = PlayerState.IDLE;
            }
            //if overreacts without reason
            else
            {
                WwiseInterface.Instance.PlayKnightCombatVoiceSound(KnightCombatVoiceHandle.OverreactOk, this.gameObject);

                //ASK JONAHTAN 0 POINTS IF OUT OF ATTACKED TIME FRAME
                Debug.Log("Overreact points: 0");
                pb.ChangeRepScore(0);
                pb.Invoke();
            }

        }
    }

    //Attacks
    public void HandleAttack()
    {
        //attackAction.NormalAttack(TauntRadius, this.transform);
        List<Collider> enemiesInRange = attackAction.ConeAttack();

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
        Debug.Log("Objective attacked " + monster.name);

        pb.ChangeRepScore(monster.GetObjectiveAttackReputation());
        pb.Invoke();

    }

    //sheep attacked by monsters
    public void SheepAttacked(MonsterAI monster)
    {
        Debug.Log("Sheep attacked " + monster.name);

        pb.ChangeRepScore(monster.GetSheepAttackReputation());
        pb.Invoke();

    }

    
    //MONSTER ATTACKS PLAYER
    public void PlayerAttacked(MonsterAI monster)
    {
        //AttackedDuration second unlock overreact
        StartCoroutine(releaseAttacked());

        overreactTimestamp = 0;

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

    public PlayerState GetPlayerState()
    {
        return playerState;
    }
        

    /* used for cooldown feedback */

    public float GetAttackActionCooldown()
    {
        return attackAction.elapsedCooldown();
    }

    public float GetOverreactActionCooldown()
    {

        return overreactAction.elapsedCooldown();
    }


    public float GetTauntActionCooldown()
    {

        return tauntAction.elapsedCooldown();
    }

    /* used for testing only */

    public void SetTauntCooldown(float val)
    {
        tauntAction.TauntCooldown = val;
    }



}
