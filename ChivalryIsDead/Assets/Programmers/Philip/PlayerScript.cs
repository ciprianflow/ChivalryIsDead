using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

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
    public bool scaring = false;
    public bool attackReachedFull = false;
    public bool overreacting = false;
    public bool Attackended = false;
    public int currentAttack = 1;
    Vector2 LastXY = new Vector2(0, 0);

    private Coroutine attackCR;
    private Coroutine tauntCR;
    private Coroutine overreactCR;

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
        AnimDic.Add("scaring", 5);
        AnimDic.Add("overreacting", 7);

        StaticIngameData.player = this.transform;

    }

    public void move(float x, float y) {

        FixedPosition(x, y);

    }

    public bool canDoAction(PlayerActions action)
    {
        switch(action)
        {
            case PlayerActions.ATTACK:
                if (taunting || overreacting)
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
        //Debug.Log(taunting + " - " + attacking + " - " + overreacting);
        
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

        //float camRot = Mathf.Deg2Rad * Camera.main.transform.eulerAngles.y;

        //float worldX = (x * Mathf.Cos(camRot)) - (y * Mathf.Sin(camRot));
        //float worldY = (x * Mathf.Sin(camRot)) + (y * Mathf.Cos(camRot));
        //worldX = (x * Mathf.Cos(Mathf.PI/4)) - (y * Mathf.Sin(Mathf.PI / 4));
        //worldY = (x * Mathf.Sin(Mathf.PI / 4)) + (y * Mathf.Cos(Mathf.PI / 4));



        //Debug.Log(Mathf.Atan2(worldY, worldX));
        //Debug.Log("X: " + (Mathf.Rad2Deg * worldX) + " Y " + (Mathf.Rad2Deg * worldY));

        //float turnValue = SignedAngle(LastXY, new Vector2(x, y));
        //turnMag += turnValue;
        LastXY = new Vector2(x, y);
        if (zVel < LastXY.magnitude - 0.05f) {
            zVel += speedAcc;
        }
        else if (zVel > LastXY.magnitude) //TODO: sqrmagnitude
        {
            zVel -= speedAcc;
        }

        //transform.eulerAngles = new Vector3(0, -Mathf.Rad2Deg * Mathf.Atan2(worldY, worldX) + 90, 0);


        //float desiredFwd = (-Mathf.Rad2Deg * Mathf.Atan2(worldY, worldX));
        if (!staticControls) {
            float desiredFwd = (Mathf.Rad2Deg * Mathf.Atan2(x, y)) + Camera.main.transform.eulerAngles.y;
            if (desiredFwd < 0) {
                desiredFwd += 360f;
            }

            float currentFwd = transform.eulerAngles.y;
            float diffTurn = desiredFwd - currentFwd;

            if (diffTurn > 180f) {
                diffTurn -= 360f;
            }
            if (diffTurn < -180f) {
                diffTurn += 360f;
            }



            Debug.Log("desiredFwd " + desiredFwd + " currentfwd " + currentFwd + " diffturn " + diffTurn);



            ////anim.SetFloat("Turn", turnMag);

            //Debug.Log(turnValue);
            //transform.position += new Vector3(worldX * maxSpeed * zVel, 0, worldY * maxSpeed * zVel);
            //transform.eulerAngles = new Vector3(0, (Mathf.Rad2Deg * Mathf.Atan2(x, y)) + Camera.main.transform.eulerAngles.y, 0);
            if (diffTurn > 0.001f || diffTurn < -0.001f) {
                float turnAmount = diffTurn / 5;
                transform.eulerAngles = new Vector3(0, currentFwd + turnAmount, 0);
                //anim.SetFloat("Turn", turnAmount / 1.5f);

            }
        }
        else {
            transform.eulerAngles = new Vector3(0, (Mathf.Rad2Deg * Mathf.Atan2(x, y)) + Camera.main.transform.eulerAngles.y, 0);
        }
        transform.Translate(0, 0, new Vector2(x, y).magnitude * maxSpeed * Time.deltaTime);

        anim.SetFloat("Speed", zVel * 2f);

        //Debug.Log( "CAMERA " + Camera.main.transform.eulerAngles.y);
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
        //if (Input.GetButtonDown("Jump"))
        //{
        //    attack();
        //}

        //Debug.Log(anim.GetCurrentAnimatorStateInfo(1).normalizedTime);


        //if (attacking)
        //{
        //    animate( ref attacking, "attacking");
        //}
        //if (taunting)
        //{
        //    animate( ref taunting, "taunting");
        //}

        //if (overreacting)
        //{
        //    animate(ref overreacting, "overreacting");
        //}
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
            //Debug.Log("does this even happen");
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
            Debug.Log("STATE ENDED");
        }
    }

    //public void animate(ref bool animState, String animName)
    //{
    //    if (animName != "attacking" && anim.GetCurrentAnimatorStateInfo(AnimDic[animName]).normalizedTime > 0.8f)
    //    {
    //        if (UpperWeight > 0)
    //        {
    //            UpperWeight -= 0.05f;
    //            anim.SetLayerWeight(AnimDic[animName], UpperWeight);
    //        }
    //        if (LowerWeight > 0)
    //        {
    //            LowerWeight -= 0.05f;
    //            anim.SetLayerWeight(AnimDic[animName] + 1, LowerWeight);
    //        }
    //        if(UpperWeight < 0 && LowerWeight < 0)
    //        {

    //            //Debug.Log("ENDEDANIM");
    //            animState = false;
    //        }
    //        return;
    //    }
    //    else if (animName == "attacking" && anim.GetCurrentAnimatorStateInfo(1).IsTag("Exit") && attackReachedFull)
    //    {
            
    //        if (UpperWeight > 0)
    //        {
    //            UpperWeight -= 0.025f;
    //            anim.SetLayerWeight(AnimDic[animName], UpperWeight);
    //        }
    //        if (LowerWeight > 0)
    //        {
    //            LowerWeight -= 0.025f;
    //            anim.SetLayerWeight(AnimDic[animName] + 1, LowerWeight);
    //        }
    //        if (UpperWeight < 0 && LowerWeight < 0)
    //        {
    //            SwordTrail.SetActive(false);
    //            animState = false;
    //        }
    //        return;
    //    }
        
    //    if (zVel == 0)
    //    {
    //        if (LowerWeight < 1)
    //        {
    //            LowerWeight += 0.05f;
    //            anim.SetLayerWeight(AnimDic[animName] + 1, LowerWeight);
    //        }
    //    }
    //    else if (LowerWeight > 0)
    //    {
    //        LowerWeight -= 0.05f;
    //        anim.SetLayerWeight(AnimDic[animName] + 1, LowerWeight);
    //    }

    //    if (UpperWeight < 1)
    //    {
    //        UpperWeight += 0.1f;
    //        anim.SetLayerWeight(AnimDic[animName], UpperWeight);
    //    }
    //    else
    //        attackReachedFull = true;
    //    //else if (UpperWeight > 0)
    //    //{
    //    //    UpperWeight -= 0.1f;
    //    //    anim.SetLayerWeight(layer-1, UpperWeight);
    //    //}

       
    //}

    public void toggleControls() {
        staticControls = !staticControls;
    }

    public void attack()
    {
        anim.SetLayerWeight(3, 0);
        anim.SetLayerWeight(4, 0);
        anim.SetLayerWeight(7, 0);
        cancelCoroutines();

        Attackended = false;

        Debug.Log("isattacking = " + attacking);
        if (overreacting)
            return;

        if(attackCR != null)
            StopCoroutine(attackCR);

        attackCR = StartCoroutine(animateAttack());

        //anim.Play("Hero_Attack1", 2, 0);

            SwordTrail.SetActive(true);
        

        if (!attacking)
        {
            currentAttack = 1;

            anim.Play("Attack Transition", 1, 0);
            currentAttack = 1;
            int r = (int)UnityEngine.Random.Range(0, 3);
            //if (r == 0) {
            //    anim.SetTrigger("Attack1");
            //}
            //else if (r == 1) {
            //    anim.SetTrigger("Attack12");
            //}
            //else {
            //    anim.SetTrigger("Attack13");
            //}
            anim.SetTrigger("Attack1");


            cancelAnim(ref taunting, "taunting");
            cancelAnim(ref scaring, "scaring");
            attacking = true;
            return;
        }



        AnimatorStateInfo ASI = anim.GetCurrentAnimatorStateInfo(1);
        //AnimatorStateInfo ASI = anim.GetNextAnimatorStateInfo(1);

        //Debug.Log(ASI.normalizedTime / ASI.length);

        if (ASI.IsTag("Attack1"))
        {
            currentAttack = 2;
            int r = (int)UnityEngine.Random.Range(0, 3);
            if (r == 0) {
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
            if (r == 0) {
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
        cancelAnim(ref scaring, "scaring");
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
    }
    public void overreact() {
        cancelCoroutines();

        anim.SetLayerWeight(3, 0);
        anim.SetLayerWeight(4, 0);
        anim.SetLayerWeight(7, 0);

        SwordTrail.SetActive(false);
        //anim.Play("Scare", 5, 0);


        if((int)UnityEngine.Random.Range(0,2) == 0) {
            anim.SetTrigger("OverreactTrig1");
        }
        else {
            anim.SetTrigger("OverreactTrig2");
        }
        cancelAnim(ref attacking, "attacking");
        cancelAnim(ref taunting, "taunting");
        cancelAnim(ref scaring, "scaring");
        overreacting = true;
        overreactCR = StartCoroutine(animateOverreact());

    }

    void cancelAnim(ref bool animState, String animName)
    {
        
        UpperWeight = 0;
        LowerWeight = 0;
        animState = false;
        anim.SetLayerWeight(AnimDic[animName], 0);
        anim.SetLayerWeight(AnimDic[animName] + 1, 0);

    }
    //[Header("Variables")]
    //public float maxSpeed = 0.5f;

    //public void move(float x, float y) {

    //    FixedPosition(x, y);

    //}

    //void FixedPosition(float x, float y)
    //{

    //    Vector3 dir = new Vector3(x, 0, y);

    //    Vector3 fwd = Camera.main.transform.forward;
    //    fwd.y = transform.position.y;

    //    //float angle = Vector3.Angle(fwd, Vector3.forward) * Mathf.Deg2Rad;
    //    float angle = CalculateAngle(fwd, Vector3.forward) * Mathf.Deg2Rad;
    //    //angle = Camera.main.transform.rotation.y * Mathf.Deg2Rad;

    //    float worldX = (x * Mathf.Cos(angle)) - (y * Mathf.Sin(angle));
    //    float worldY = (x * Mathf.Sin(angle)) + (y * Mathf.Cos(angle));

    //    //dir = Quaternion.AngleAxis(angle, Vector3.up) * dir;

    //    //Debug.Log(Mathf.Atan2(worldY, worldX));
    //    //transform.eulerAngles = new Vector3(0, -Mathf.Rad2Deg * Mathf.Atan2(worldY, worldX), 0);

    //    transform.LookAt(transform.position - new Vector3(worldX * maxSpeed, 0, worldY * maxSpeed));

    //    transform.position -= new Vector3(worldX * maxSpeed, 0, worldY * maxSpeed);

    //    //transform.position += dir * maxSpeed;
    //}

    //public static float CalculateAngle(Vector3 from, Vector3 to)
    //{
    //    return Quaternion.FromToRotation(Vector3.up, to - from).eulerAngles.z;
    //}



}
