using UnityEngine;
using System.Collections;

public class PlayerActionController : MonoBehaviour {

    private TauntAction tauntAction;
    private AttackAction attackAction;
    private OverreactAction overreactAction;

    public float TauntRadius = 5f;
    public float TauntDuration = 3f;

    public float AggroRadius = 4f;


    private float attackRange = 35f;
    private float attackRadius = 120f;

    //used for overreacting
    private bool attacked = false;
    

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

        //add taunt
        gameObject.AddComponent<TauntAction>();
        tauntAction = this.gameObject.GetComponent<TauntAction>();


        attackAction = new AttackAction();
        overreactAction = new OverreactAction();
        
    }

	// Use this for initialization
	void Start () {

        //init for taunt
        tauntAction.AggroRadius = AggroRadius;
        tauntAction.TauntDuration = TauntDuration;
        tauntAction.TauntRadius = TauntRadius;
    }

    /// <summary>
    /// Handle Taunt Button
    /// </summary>
    public void HandleTaunt()
    {
        // if attacked the player can overreact
        if (attacked)
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
        
        //can overreact
        Debug.Log("player hit");
    }


}
