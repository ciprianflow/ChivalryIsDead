using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public enum PlayerState
{
    IDLE, HIT, ATTACKING, TAUNTING
}

public class PlayerActionController : MonoBehaviour {

    
    [Header("Attack values")]
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


    void OnDrawGizmos()
    {

        float visibilityDistance = 2f;

        Gizmos.color = Color.red;

        //cone attack radius
        Gizmos.color = Color.magenta;
        //Gizmos.DrawRay(transform.position, transform.forward * 5);
        //Gizmos.DrawRay(transform.position, Quaternion.AngleAxis(30f, transform.up) * transform.forward * visibilityDistance);

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
        attackAction.AttackAngle = AttackAngle;
        attackAction.AttackRange = AttackRange;

}

    /// <summary>
    /// Handle Taunt Button
    /// </summary>
    public void HandleTaunt()
    {
        // if attacked the player can overreact
        if (playerState == PlayerState.HIT)
        {
            overreactAction.Overreact();
        }
        else
        {
            //otherwhise taunt
            tauntAction.Taunt();
        }
    }


    //Attacks
    //REMOVE LOGIC FROM THIS CLASS!!
    public void HandleAttack()
    {
        //attackAction.NormalAttack(TauntRadius, this.transform);
        //if no enemies in range SCARE
        List<Collider> enemiesInRange = attackAction.GetConeRange();
        if (enemiesInRange.Count > 0)
        {
            attackAction.ConeAttack(enemiesInRange);
        }
        else
        {
            scareAction.Scare(ScareRadius);
        }
    }

    public void Attacked()
    {
        //AttackedDuration second unlock overreact
        StartCoroutine(releaseAttacked());
        //can overreact
        playerState = PlayerState.HIT;
    }


    private IEnumerator releaseAttacked()
    {
        yield return new WaitForSeconds(AttackedDuration);

        playerState = PlayerState.IDLE;
    }

}
