using UnityEngine;
using System.Collections;

public enum PlayerState
{
    IDLE, HIT, ATTACKING, TAUNTING
}

public class PlayerActionController : MonoBehaviour {

    private AggroAction aggroAction;
    private TauntAction tauntAction;
    private AttackAction attackAction;
    private OverreactAction overreactAction;

    public float TauntRadius = 5f;
    public float TauntDuration = 3f;

    public float AggroRadius = 4f;

    //duration for the overreact mechanic
    public float AttackedDuration = 1.5f;


    private float attackRange = 35f;
    private float attackRadius = 120f;

    //used for overreacting
    private PlayerState playerState;

    

    void OnDrawGizmos()
    {

        float visibilityDistance = 2f;

        Gizmos.color = Color.red;

        //cone attack radius
        Gizmos.color = Color.magenta;
        //Gizmos.DrawRay(transform.position, Quaternion.AngleAxis(-30f, transform.up) * transform.forward * visibilityDistance);
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


        aggroAction = gameObject.GetComponent<AggroAction>();
        tauntAction = this.gameObject.GetComponent<TauntAction>();
        overreactAction = this.gameObject.GetComponent<OverreactAction>();


        attackAction = new AttackAction();

        
    }

	// Use this for initialization
	void Start () {

        playerState = PlayerState.IDLE;

        //init for taunt
        tauntAction.TauntDuration = TauntDuration;
        tauntAction.TauntRadius = TauntRadius;

        aggroAction.AggroRadius = AggroRadius;

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
    public void Attack()
    {
        //attackAction.NormalAttack(TauntRadius, this.transform);
        attackAction.ConeAttack();
    }

    public void Attacked()
    {
        //AttackedDuration second unlock overreact
        StartCoroutine(releaseAttacked());
        //can overreact
        playerState = PlayerState.HIT;
        Debug.Log("player hit");
    }


    private IEnumerator releaseAttacked()
    {
        yield return new WaitForSeconds(AttackedDuration);

        playerState = PlayerState.IDLE;
    }

}
