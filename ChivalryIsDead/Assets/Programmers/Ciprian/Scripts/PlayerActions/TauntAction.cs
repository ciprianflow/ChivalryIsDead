using System.Collections.Generic;
using UnityEngine;


class TauntAction: MonoBehaviour
{

    public float TauntDuration;
    public float TauntRadius;
    public float TauntCooldown = 5f;

    //used for cooldown i guess
    private bool alreadyTaunting = false;

    private float currentTauntRadius;
    private float currentTauntDuration;
    private float overTime;


    private PlayerScript playerBase;

    void Awake()
    {
        playerBase = GetComponent<PlayerScript>();
    }

    private float cooldownTimeStamp;


    void Start()
    {
        currentTauntRadius = TauntRadius;
    }

    void Update()
    {
        //Aggro
        /*
        if (getCoolDown())
        {
            startTaunt(currentTauntRadius, this.transform.position);
            //shrinkTauntArea();
        }
        */
    }

    private void startTaunt(float radius, Vector3 position)
    {

        cooldownTimeStamp = Time.time + TauntCooldown;

        //10 layer - Monster
        Collider[] hitColliders = Physics.OverlapSphere(position, radius);
        int i = 0;
        while (i < hitColliders.Length)
        {
            
            if (hitColliders[i].CompareTag("Enemy"))
            {
                checkStateAndTaunt(hitColliders[i].GetComponent<MonsterAI>());
            }

            i++;
        }
    }

    private void checkStateAndTaunt(MonsterAI monster)
    {
        //everything but idle
        if (monster.getState() != State.Idle || monster.GetType() == typeof(SheepAI))
        {
            monster.Taunt();
        }
    }


    //TAUNT Action
    public void Taunt()
    {

        //Debug.Log("TAUNT CAN: " + playerBase.canDoAction(PlayerActions.TAUNT));
        if (getCoolDown() && playerBase.canDoAction(PlayerActions.TAUNT))
        {
            WwiseInterface.Instance.PlayKnightCombatSound(KnightCombatHandle.Taunt, this.gameObject);
            
            startTaunt(currentTauntRadius, this.transform.position);
            playerBase.taunt();
            //shrinkTauntArea();
        }
        /*
        //just change aggro radius 
        if (!alreadyTaunting)
        {
            Debug.Log("dwjaio");
            playerBase.taunt();
            currentTauntDuration = TauntDuration;
            overTime = 0;
            alreadyTaunting = true;
        }
        */
    }

    //shrinkTauntArea
    private void shrinkTauntArea()
    {

        overTime += (0.01f / TauntDuration) * Time.deltaTime;

        currentTauntRadius = Mathf.Lerp(currentTauntRadius, TauntRadius, overTime);

        if (Mathf.Abs(currentTauntRadius - TauntRadius) < 0.1)
        {
            alreadyTaunting = false;
        }

    }

    //cooldown
    private bool getCoolDown()
    {
        if (cooldownTimeStamp >= Time.time)
        {
            return false;
        }
        return true;
    }

    public float elapsedCooldown()
    {
        if (cooldownTimeStamp < Time.time)
        {
            return 1;
        }

        return 1 - Mathf.Abs(((Time.time - cooldownTimeStamp)) / TauntCooldown);
    }

}
