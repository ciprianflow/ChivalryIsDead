using UnityEngine;
using UnityEngine.SceneManagement;
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
    public Gameplay_Dialog GameDialogUI;
    public GameObject RepGainParticle;
    public GameObject RepLossParticle;
    public GameObject ComboParticle;
    public GameObject ComboUpwardParticle;

    public GameObject OverreactGreatParticle;
    public GameObject OverreactOkParticle;

    [HideInInspector]
    public static List<MonsterAI> monstersInScene;

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
    private IEnumerator releaseAttackedCoroutine;

    public GameObject hitParticle;

    private int countAttacks = 0;
    private int countAttackedMonstr;
    private bool isNeverAttacked;
    private bool isNeverTaunt;
    private bool isNeverOverreact;
    private bool noSheepKilled;
    private float countTime;
    private bool isTutorial;

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
        countAttackedMonstr = 0;
        isNeverAttacked = true;
        isNeverTaunt = true;
        isNeverOverreact = true;
        noSheepKilled = true;
        if(SceneManager.GetActiveScene().name == "IntroLevel" || SceneManager.GetActiveScene().name == "Tutorial_02" || SceneManager.GetActiveScene().name == "Tutorial_03" || SceneManager.GetActiveScene().name == "Introlevel" || SceneManager.GetActiveScene().name == "SandraCopy")
        {
            isTutorial = true;
        }
        //subscribe to the reputation system
        pb = new PlayerBehaviour("rep");

        //load particles for all levels but intro level
        if(SceneManager.GetActiveScene() != SceneManager.GetSceneByName("IntroLevel"))
        {
            pb.RepGainParticle = RepGainParticle;
            pb.RepLossParticle = RepLossParticle;

            DummyManager.dummyManager.ComboBaseParticle = ComboParticle;
            DummyManager.dummyManager.ComboUpwardParticle = ComboUpwardParticle;
        }


        pb.ResetCombo();


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

        //init monsters
    }

    void Update()
    {

        overreactTimestamp += Time.deltaTime;

        if((isNeverAttacked || isNeverTaunt || isNeverOverreact || noSheepKilled) && !isTutorial)
        {
            countTime += Time.deltaTime;
            if (countTime > 10)
            {
                if (isNeverAttacked)
                {
                    GameDialogUI.StartCoroutine("NoGettingHit");
                    isNeverAttacked = false;
               }
            }
            if(countTime > 15)
            {
                if (noSheepKilled)
                {
                    GameDialogUI.StartCoroutine("NoSheepKilled");
                    noSheepKilled = false;
                }
                
            }
             if(countTime > 20)
            {
                if (isNeverTaunt)
                {
                    GameDialogUI.StartCoroutine("NoTaunting");
                    isNeverTaunt = false;
                }
            }               
            if(countTime > 25)
            {
                if (isNeverOverreact)
                {
                    GameDialogUI.StartCoroutine("NoOverreacting");
                    isNeverOverreact = false;
                }
            }

        }

    }


    /// <summary>
    /// Handle Taunt Button
    /// </summary>
    public void HandleTaunt()
    {
        isNeverTaunt = false;
        //otherwhise taunt
        tauntAction.Taunt();
    }

    //REFACTOR ACTIONS
    public void HandleOverreact()
    {
        isNeverOverreact = false;
        //Player overreacted checks cooldown from the action
        if (overreactAction.Overreact()) {

            // if attacked the player can receive points based on time
            if (playerState == PlayerState.HIT && lastMonsterAttacked != null) {

                int points = (int)((AttackedDuration - overreactTimestamp) * 100);
                //Debug.Log("Overreact points:" + -points + " Attack dur: " + AttackedDuration + " - timestamp: " + overreactTimestamp);
                //@@HARDCODED
                // perfect overreact
                if (points > 99)
                {
                    WwiseInterface.Instance.PlayKnightCombatVoiceSound(KnightCombatVoiceHandle.OverreactPerfect, this.gameObject);
                    if (OverreactGreatParticle != null)
                    {
                        OverreactGreatParticle.SetActive(false);
                        OverreactGreatParticle.SetActive(true);
                    }
                        

                }
                // ok overreact
                else
                {
                    WwiseInterface.Instance.PlayKnightCombatVoiceSound(KnightCombatVoiceHandle.OverreactGreat, this.gameObject);
                    if (OverreactOkParticle != null)
                    {
                        OverreactOkParticle.SetActive(false);
                        OverreactOkParticle.SetActive(true);
                        
                    }
                        
                }
                    


                pb.AddRepScore(-points);
                Debug.Log("Overreact points:" + -points);
                //pb.Invoke();
                //change player state to IDLE after overreacting
                playerState = PlayerState.IDLE;
            }
            //if overreacts without reason
            else
            {
                WwiseInterface.Instance.PlayKnightCombatVoiceSound(KnightCombatVoiceHandle.OverreactOk, this.gameObject);
                if(GameDialogUI != null)
                    GameDialogUI.StartCoroutine("WrongOverreact");
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
        countAttacks++;
       
        //add reputation
        foreach (Collider enemy in enemiesInRange)
        {
            MonsterAI monster = enemy.GetComponent<MonsterAI>();

            if (monster.GetType() == typeof(SheepAI))
            {
                if (GameDialogUI != null)
                    GameDialogUI.StartCoroutine("YouHitSheep");
            }
            else
            {
                countAttackedMonstr++;
                if (countAttackedMonstr > 2 && countAttacks > 2 && GameDialogUI != null)
                {
                    GameDialogUI.StartCoroutine("StopAttacking");
                    countAttackedMonstr = 0;
                    countAttacks = 0;
                }
            }
            Vector3 midVec = Vector3.Normalize(transform.position - monster.transform.position);
            Vector3 hitPoint = monster.transform.position + (midVec * 0.2f);
            GameObject hP = Instantiate(hitParticle) as GameObject;
            hP.transform.position = hitPoint;
            //EditorApplication.isPaused = true;

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
        noSheepKilled = false;
        pb.ChangeRepScore(monster.GetSheepAttackReputation());
        pb.Invoke();

    }

    
    //MONSTER ATTACKS PLAYER
    public void PlayerAttacked(MonsterAI monster)
    {
        isNeverAttacked = false;
        //AttackedDuration second unlock overreact
        releaseAttackedCoroutine = releaseAttacked();
        StartCoroutine(releaseAttackedCoroutine);

        overreactTimestamp = 0;

        //can overreact
        playerState = PlayerState.HIT;
        //Debug.Log("HIT");
        //GetComponentInChildren<Animator>().SetTrigger("TakeDamage");
        //GetComponentInChildren<Animator>().SetLayerWeight(8, 1);
        GetComponent<PlayerScript>().takeDamage();
        //Player attacked add reputation according to monster base damage
        if (monster)
        {
            StopCoroutine(releaseAttackedCoroutine);
            pb.ChangeRepScore(monster.GetAttackReputation());

            //save last monster attacked
            lastMonsterAttacked = monster;

            pb.Invoke();
        }
    }

    private IEnumerator releaseAttacked()
    {
        yield return new WaitForSeconds(AttackedDuration);

        //wait for all attacks to submit change in reputation
        pb.Invoke();
        playerState = PlayerState.IDLE;
        Debug.Log("PLYAER IDLE NOW");
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
