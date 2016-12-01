using System.Collections;
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

    private GameObject playerTauntBubble;

    void Awake()
    {
        playerTauntBubble = GameObject.FindGameObjectWithTag("PlayerTauntBubble");
        playerBase = GetComponent<PlayerScript>();
    }

    private float cooldownTimeStamp;


    void Start()
    {
        playerTauntBubble.SetActive(false);
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
        //Collider[] hitColliders = Physics.OverlapSphere(position, radius);
        Collider[] hitColliders = GetTauntedColliders();
        List<GameObject> exmarks = new List<GameObject>();
        playerTauntBubble.SetActive(true);
        int i = 0;
        while (i < hitColliders.Length)
        {
            
            if (hitColliders[i].CompareTag("Enemy"))
            {
                checkStateAndTaunt(hitColliders[i].GetComponent<MonsterAI>());
                exmarks.Add(hitColliders[i].GetComponent<ExclamationMark>().ExclaMark());
            }

            i++;
        }
        StartCoroutine("NoExclMarks", exmarks);
    }

    IEnumerator NoExclMarks(List<GameObject> exmarks)
    {
        yield return new WaitForSeconds(2f);
        playerTauntBubble.SetActive(false);
        foreach (GameObject exmark in exmarks)
            exmark.SetActive(false);
    }


    public Collider[] GetTauntedColliders()
    {
        return Physics.OverlapSphere(this.transform.position, currentTauntRadius); 
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
            WwiseInterface.Instance.PlayKnightCombatVoiceSound(KnightCombatVoiceHandle.Taunt, this.gameObject);
            
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
    /*
    private void shrinkTauntArea()
    {

        overTime += (0.01f / TauntDuration) * Time.deltaTime;

        currentTauntRadius = Mathf.Lerp(currentTauntRadius, TauntRadius, overTime);

        if (Mathf.Abs(currentTauntRadius - TauntRadius) < 0.1)
        {
            alreadyTaunting = false;
        }

    }
    */
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
