using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour {
    [Header("Variables")]
    public float maxSpeed = 6f;
    public float zVel = 0;
    public float turnMag = 0;
    public float speedAcc = 0.03f;
    public float speedDec = 0.03f;
    public Animator anim;
    float worldX = 0;
    float worldY = 0;
    public bool attacking = false;
    public bool taunting = false;
    public bool takedamaging = false;
    public bool attackReachedFull = false;
    public bool overreacting = false;
    public bool Attackended = false;
    public int currentAttack = 1;
    Vector2 LastXY = new Vector2(0, 0);

    private Coroutine attackCR;
    private Coroutine tauntCR;
    private Coroutine overreactCR;
    private Coroutine takedamageCR;

    private bool isSlowingDown = false;

    private bool staticControls = true;
    public float attackUpperWeight = 0;
    public float attackLowerWeight = 0;
    public float UpperWeight = 0;
    public float LowerWeight = 0;

    public GameObject SwordTrail;
    public GameObject DustParticle;

    Dictionary<String, int> AnimDic = new Dictionary<String, int>();


    void Awake()
    {
        //Time.timeScale = 1f;

        AnimDic.Add("attacking", 1);
        AnimDic.Add("taunting", 3);
        AnimDic.Add("overreacting", 7);
        AnimDic.Add("takedamaging", 8);

        StaticIngameData.player = this.transform;


        //AkSoundEngine.PostEvent("musicquest", gameObject);
        //AkSoundEngine.PostEvent("start_world_1_ambience", gameObject);
        //AkSoundEngine.PostEvent("start_hub_ambience", gameObject);
        //Debug.Log("WHY AM I HERE");

    }

    public void move(float x, float y) {

        FixedPosition(x, y);

    }

    public bool canDoAction(PlayerActions action)
    {
        switch(action)
        {
            case PlayerActions.ATTACK:
                if (overreacting)
                {
                    return false;
                }
                break;

            case PlayerActions.TAUNT:
                if (attacking || overreacting )
                {
                    return false;
                }
                break;

            case PlayerActions.OVERREACT:
                if (attacking || taunting)
                {
                    return false;
                }
                break;


            default:
                return true;
        }
        
        return true;
    }

    void FixedPosition(float x, float y) {
        if (overreacting)
            return;

        if (x == 0 && y == 0) {
            isSlowingDown = true;
            DustParticle.SetActive(false);
            return;
        }
        else {
            isSlowingDown = false;
            DustParticle.SetActive(true);
        }


        LastXY = new Vector2(x, y);
        if (zVel < LastXY.sqrMagnitude - 0.05f) {
            zVel += speedAcc;
        }
        else if (zVel > LastXY.sqrMagnitude) //TODO: sqrmagnitude
        {
            zVel -= speedAcc;
        }

        transform.eulerAngles = new Vector3(0, (Mathf.Rad2Deg * Mathf.Atan2(x, y)) + Camera.main.transform.eulerAngles.y, 0);
        transform.Translate(0, 0, new Vector2(x, y).magnitude * maxSpeed * Time.deltaTime);
        //GetComponent<Rigidbody>().AddRelativeForce(0, 0, 1000);
        anim.SetFloat("Speed", zVel * 2f);

    }

    float SignedAngle(Vector3 a, Vector3 b) {
        float angle = Vector3.Angle(a, b); // calculate angle
                                           // assume the sign of the cross product's Y component:
        return -angle * Mathf.Sign(Vector3.Cross(a, b).z);
    }

    void Update() {
        if (isSlowingDown) {
            if (zVel > 0) {
                transform.position += new Vector3(worldX * maxSpeed * zVel, 0, worldY * maxSpeed * zVel);
                zVel -= speedDec;
                anim.SetFloat("Speed", zVel * 2f);

            }
            else {
                zVel = 0;
                anim.SetFloat("Speed", zVel * 2f);
                isSlowingDown = false;
            }
        }
        if (turnMag > 0) {
            turnMag -= 5f;
            anim.SetFloat("Turn", turnMag);

        }
        if (Input.GetButtonDown("Jump")) {
            anim.SetLayerWeight(8, 0);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            taunt();
        }

    }

    IEnumerator animateTaunt() {
        bool reachedFullWeight = false;
        float upperWeight = 0;
        float lowerWeight = 0;

        while(reachedFullWeight == false) {
            upperWeight += 0.05f;
            if(zVel == 0 && lowerWeight < 1) {
                lowerWeight += 0.05f;
            }
            else if (zVel != 0 && lowerWeight > 0) {
                lowerWeight -= 0.05f;
            }
            if(upperWeight >= 1) {
                reachedFullWeight = true;
            }
            anim.SetLayerWeight(3, upperWeight);
            anim.SetLayerWeight(4, lowerWeight);
            yield return new WaitForSeconds(0.01f);
        }
        while (reachedFullWeight && anim.GetCurrentAnimatorStateInfo(3).normalizedTime < 0.8f) {
            if (zVel == 0 && lowerWeight < 1) {
                lowerWeight += 0.05f;
            }
            else if (zVel != 0 && lowerWeight > 0) {
                lowerWeight -= 0.05f;
            }
            anim.SetLayerWeight(4, lowerWeight);
            yield return new WaitForSeconds(0.01f);
        }
        while (reachedFullWeight && anim.GetCurrentAnimatorStateInfo(3).normalizedTime >= 0.8f) {
            upperWeight -= 0.05f;
            lowerWeight -= 0.05f;
            anim.SetLayerWeight(3, upperWeight);
            anim.SetLayerWeight(4, lowerWeight);
            if(lowerWeight < 0 && upperWeight < 0) {
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }
        taunting = false;
    }

    IEnumerator animateOverreact() {
        bool reachedFullWeight = false;
        float upperWeight = 0;

        while (reachedFullWeight == false) {
            upperWeight += 0.05f;

            if (upperWeight >= 1) {
                reachedFullWeight = true;
            }
            anim.SetLayerWeight(7, upperWeight);
            yield return new WaitForSeconds(0.01f);
        }
        while (reachedFullWeight && anim.GetCurrentAnimatorStateInfo(7).normalizedTime < 0.8f) {
            yield return new WaitForSeconds(0.01f);
        }
        while (reachedFullWeight && anim.GetCurrentAnimatorStateInfo(7).normalizedTime >= 0.8f) {
            upperWeight -= 0.05f;
            anim.SetLayerWeight(7, upperWeight);
            if (upperWeight < 0) {
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }
        overreacting = false;
    }


    IEnumerator animateTakeDamage() {
        bool reachedFullWeight = false;
        float upperWeight = 0;

        while (reachedFullWeight == false) {
            upperWeight += 0.05f;

            if (upperWeight >= 1) {
                reachedFullWeight = true;
            }
            anim.SetLayerWeight(8, upperWeight);
            yield return new WaitForSeconds(0.01f);
        }
        while (reachedFullWeight && anim.GetCurrentAnimatorStateInfo(8).normalizedTime < 0.8f) {
            yield return new WaitForSeconds(0.01f);
        }
        while (reachedFullWeight && anim.GetCurrentAnimatorStateInfo(8).normalizedTime >= 0.8f) {
            upperWeight -= 0.05f;
            anim.SetLayerWeight(8, upperWeight);
            if (upperWeight < 0) {
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }
        takedamaging = false;
    }

    IEnumerator animateAttack() {
        bool reachedFullWeight = false;

        while (reachedFullWeight == false) {
            attackUpperWeight += 0.05f;
            if (zVel == 0 && attackLowerWeight < 1) {
                attackLowerWeight += 0.05f;
            }
            else if (zVel != 0 && attackLowerWeight > 0) {
                attackLowerWeight -= 0.05f;
            }
            if (attackUpperWeight >= 1) {
                reachedFullWeight = true;
            }
            anim.SetLayerWeight(1, attackUpperWeight);
            anim.SetLayerWeight(2, attackLowerWeight);
            yield return new WaitForSeconds(0.01f);
        }
        while (reachedFullWeight && !Attackended) {
            if (zVel == 0 && attackLowerWeight < 1) {
                attackLowerWeight += 0.05f;
            }
            else if (zVel != 0 && attackLowerWeight > 0) {
                attackLowerWeight -= 0.05f;
            }
            anim.SetLayerWeight(2, attackLowerWeight);
            yield return new WaitForSeconds(0.01f);
        }
        while (!Attackended) {
            yield return new WaitForSeconds(0.01f);
        }

        while ((attackLowerWeight > 0 || attackUpperWeight > 0) && Attackended) {
            SwordTrail.SetActive(false);
            if (attackLowerWeight > 0) {
                attackLowerWeight -= 0.05f;
                anim.SetLayerWeight(2, attackLowerWeight);
            }
            if (attackUpperWeight > 0) {
                attackUpperWeight -= 0.05f;
                anim.SetLayerWeight(1, attackUpperWeight);
            }
            //attackLowerWeight -= 0.05f;
            //anim.SetLayerWeight(1, attackUpperWeight);
            //anim.SetLayerWeight(2, attackLowerWeight);

            yield return new WaitForSeconds(0.01f);
        }
        
        attacking = false;
        Attackended = false;

    }

    public void EndState(int a) {
        if (a == currentAttack) {
            Attackended = true;
        }
    }

    

    public void toggleControls() {
        staticControls = !staticControls;
    }

    public void attack() {
        //WwiseInterface.Instance.SetMusic(MusicHandle.MusicStop);
        //AkSoundEngine.PostEvent("musicStop", gameObject);

        anim.SetLayerWeight(3, 0);
        anim.SetLayerWeight(4, 0);
        anim.SetLayerWeight(7, 0);
        anim.SetLayerWeight(8, 0);
        cancelCoroutines();

        Attackended = false;

        if (overreacting)
            return;

        if(attackCR != null)
            StopCoroutine(attackCR);

        attackCR = StartCoroutine(animateAttack());

        //anim.Play("Hero_Attack1", 2, 0);

        SwordTrail.SetActive(true);

        //if (SceneGetter.Instance.isTutorial1()) {
        //    Debug.Log("THIS ISL TUT 1");
        //}
        //if (SceneGetter.Instance.isTutorial2()) {
        //    Debug.Log("THIS ISL TUT 2");
        //}
        //if (SceneGetter.Instance.isTutorial3()) {
        //    Debug.Log("THIS ISL TUT 3");
        //}
        //if (SceneGetter.Instance.isMainQuest()) {
        //    Debug.Log("THIS IS MAIN QUEST");
        //}


        if (!attacking)
        {
            currentAttack = 1;

            anim.Play("Attack Transition", 1, 0);
            currentAttack = 1;
            int r = (int)UnityEngine.Random.Range(0, 3);

            anim.SetTrigger("Attack1");


            cancelAnim(ref taunting, "taunting");
            cancelAnim(ref takedamaging, "takedamaging");
            attacking = true;
            return;
        }



        AnimatorStateInfo ASI = anim.GetCurrentAnimatorStateInfo(1);


        if (ASI.IsTag("Attack1"))
        {
            currentAttack = 2;
            int r = (int)UnityEngine.Random.Range(0, 3);
            //if (r == 0 || SceneManager.GetActiveScene().name == "Introlevel" || PlayerPrefs.GetInt("AttackLevel") == 0) {
            if (r == 0 || SceneGetter.Instance.isTutorial1()) {
                anim.SetTrigger("Attack2");
            }
            else if(r == 1) {
                anim.SetTrigger("Attack22");
            }
            else {
                anim.SetTrigger("Attack23");
            }
        }
        else
        {
            currentAttack = 1;
            int r = (int)UnityEngine.Random.Range(0, 3);
            if (r == 0 || SceneGetter.Instance.isTutorial1()) {
                anim.SetTrigger("Attack1");
            }
            else if (r == 1) {
                anim.SetTrigger("Attack12");
            }
            else {
                anim.SetTrigger("Attack13");
            }
        }

        attackReachedFull = false;
    }

    public void taunt()
    {
        if (overreacting)
            return;

        SwordTrail.SetActive(false);

        //anim.Play("Taunt", 3, 0);
        anim.SetTrigger("TauntTrig");
        cancelAnim(ref attacking, "attacking");
        cancelAnim(ref takedamaging, "takedamaging");

        if (tauntCR != null)
            StopCoroutine(tauntCR);
        if (attackCR != null)
            StopCoroutine(attackCR);
        if (overreactCR != null)
            StopCoroutine(overreactCR);
        cancelCoroutines();

        anim.SetLayerWeight(1, 0);
        anim.SetLayerWeight(2, 0);
        anim.SetLayerWeight(7, 0);
        anim.SetLayerWeight(8, 0);

        tauntCR = StartCoroutine(animateTaunt());

        taunting = true;
    }
    public void cancelCoroutines() {
        if (tauntCR != null)
            StopCoroutine(tauntCR);
        if (attackCR != null)
            StopCoroutine(attackCR);
        if (overreactCR != null)
            StopCoroutine(overreactCR);
        if (takedamageCR != null)
            StopCoroutine(takedamageCR);
    }
    public void overreact() {
        cancelCoroutines();

        anim.SetLayerWeight(3, 0);
        anim.SetLayerWeight(4, 0);
        anim.SetLayerWeight(7, 0);
        anim.SetLayerWeight(8, 0);

        SwordTrail.SetActive(false);



        if((int)UnityEngine.Random.Range(0,2) == 0) {
            anim.SetTrigger("OverreactTrig1");
        }
        else {
            anim.SetTrigger("OverreactTrig2");
        }
        cancelAnim(ref attacking, "attacking");
        cancelAnim(ref taunting, "taunting");
        cancelAnim(ref takedamaging, "takedamaging");
        overreacting = true;
        overreactCR = StartCoroutine(animateOverreact());

    }

    public void takeDamage() {
        //cancelCoroutines();
        if(attacking || taunting || overreacting || takedamaging) {
            return;
        }



        takedamaging = true;
        anim.SetTrigger("TakeDamage");
        takedamageCR = StartCoroutine(animateTakeDamage());

    }

    void cancelAnim(ref bool animState, String animName)
    {
        
        UpperWeight = 0;
        LowerWeight = 0;
        animState = false;
        anim.SetLayerWeight(AnimDic[animName], 0);
        anim.SetLayerWeight(AnimDic[animName] + 1, 0);

    }
}
