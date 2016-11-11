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
    Vector2 LastXY = new Vector2(0, 0);

    private bool isSlowingDown = false;

    private bool staticControls = true;
    public float UpperWeight = 0;
    public float LowerWeight = 0;

    public GameObject SwordTrail;

    Dictionary<String, int> AnimDic = new Dictionary<String, int>();


    void Awake()
    {

        //Time.timeScale = 0.1f;

        AnimDic.Add("attacking", 1);
        AnimDic.Add("taunting", 3);
        AnimDic.Add("scaring", 5);

        StaticIngameData.player = this.transform;

    }

    public void move(float x, float y) {

        FixedPosition(x, y);

    }

    void FixedPosition(float x, float y) {
        if (x == 0 && y == 0) {
            isSlowingDown = true;
            return;
        }
        else {
            isSlowingDown = false;
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
        if (zVel < LastXY.magnitude) {
            zVel += speedAcc;
        }
        else if (zVel > LastXY.magnitude)
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
        if (Input.GetButtonDown("Jump"))
        {
            attack();
        }



        if (attacking)
        {
            animate( ref attacking, "attacking");
        }
        if (taunting)
        {
            animate( ref taunting, "taunting");
        }
        if (scaring)
        {
            animate(ref scaring, "scaring");
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            taunt();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            scare();
        }
    }

    public void animate(ref bool animState, String animName)
    {
        if (animName != "attacking" && anim.GetCurrentAnimatorStateInfo(AnimDic[animName]).normalizedTime > 0.8f)
        {
            if (UpperWeight > 0)
            {
                UpperWeight -= 0.05f;
                anim.SetLayerWeight(AnimDic[animName], UpperWeight);
            }
            if (LowerWeight > 0)
            {
                LowerWeight -= 0.05f;
                anim.SetLayerWeight(AnimDic[animName] + 1, LowerWeight);
            }
            if(UpperWeight < 0 && LowerWeight < 0)
            {


                animState = false;
            }
            return;
        }
        else if (animName == "attacking" && anim.GetCurrentAnimatorStateInfo(1).IsTag("Exit"))
        {
            if (UpperWeight > 0)
            {
                UpperWeight -= 0.025f;
                anim.SetLayerWeight(AnimDic[animName], UpperWeight);
            }
            if (LowerWeight > 0)
            {
                LowerWeight -= 0.025f;
                anim.SetLayerWeight(AnimDic[animName] + 1, LowerWeight);
            }
            if (UpperWeight < 0 && LowerWeight < 0)
            {
                SwordTrail.SetActive(false);
                animState = false;
            }
            return;
        }
        
        if (zVel == 0)
        {
            if (LowerWeight < 1)
            {
                LowerWeight += 0.05f;
                anim.SetLayerWeight(AnimDic[animName] + 1, LowerWeight);
            }
        }
        else if (LowerWeight > 0)
        {
            LowerWeight -= 0.05f;
            anim.SetLayerWeight(AnimDic[animName] + 1, LowerWeight);
        }

        if (UpperWeight < 1)
        {
            UpperWeight += 0.1f;
            anim.SetLayerWeight(AnimDic[animName], UpperWeight);
        }
        //else if (UpperWeight > 0)
        //{
        //    UpperWeight -= 0.1f;
        //    anim.SetLayerWeight(layer-1, UpperWeight);
        //}

       
    }

    public void toggleControls() {
        staticControls = !staticControls;
    }

    public void attack()
    {
        anim.Play("Hero_Attack1", 2, 0);

            SwordTrail.SetActive(true);
        

        if (!attacking)
        {
            anim.Play("Attack Transition", 1, 0);
            cancelAnim(ref taunting, "taunting");
            cancelAnim(ref scaring, "scaring");
            attacking = true;
            return;
        }

        attacking = true;


        AnimatorStateInfo ASI = anim.GetCurrentAnimatorStateInfo(1);
        //AnimatorStateInfo ASI = anim.GetNextAnimatorStateInfo(1);
        Debug.Log(anim.GetCurrentAnimatorStateInfo(1).ToString());
        Debug.Log(   anim.GetNextAnimatorStateInfo(1).ToString());

        //Debug.Log(ASI.normalizedTime / ASI.length);

        if (ASI.IsName("Attack1") || (ASI.IsName("Attack 1 exit tran") && (ASI.normalizedTime / ASI.length > 0.5f)))
        {
            anim.SetTrigger("Attack2");
        }
        else
        {
            anim.SetTrigger("Attack1");
        }
    }

    public void taunt()
    {
        SwordTrail.SetActive(false);

        //anim.Play("Taunt", 3, 0);
        anim.SetTrigger("TauntTrig");
        cancelAnim(ref attacking, "attacking");
        cancelAnim(ref scaring, "scaring");
        taunting = true;
    }
    public void scare()
    {
        SwordTrail.SetActive(false);
        //anim.Play("Scare", 5, 0);
        anim.SetTrigger("ScareTrig");
        cancelAnim(ref attacking, "attacking");
        cancelAnim(ref taunting, "taunting");
        scaring = true;

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
