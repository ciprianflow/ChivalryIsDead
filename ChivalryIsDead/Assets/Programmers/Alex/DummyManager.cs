using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DummyManager : MonoBehaviour
{
    public static DummyManager dummyManager;

    [Header("Combo values")]

    public float[] ComboCooldown;
    public int[] ComboMultiplier;
    public int[] ComboActionModifier;

    [Header("Anti AFK")]
    public int StartAFKSeconds = 20;
    public int AFKPointsDec = 20;

    public Text ComboHandler;

    public ScoreHandler ReputationHandler;
    
    public ScoreHandler SuspicionHandler;
    public ScoreHandler DaysRemaining;

    [Header("Other stuff")]
    public Gameplay_Dialog GameDialogUI;
    public GameObject ComboBaseParticle;
    public GameObject ComboUpwardParticle;

    [HideInInspector]
    private float repGain = 0;
    private float comboTimeStamp = 0;

    private int combo = 0;
    private int oldCombo = 0;
    //used to track actions that modify the combo
    private int comboModifierActions = 0;


    private float antiAfkTimestamp;
    private int antiAfkPoints = 10;
    private int antiAFKTime;
    private bool firstTimeAFK;
    private bool lowCombo;

    void Awake()
    {
        StaticIngameData.dummyManager = this;
        DummyManager.dummyManager = this;

        antiAfkPoints = AFKPointsDec;
    }

    void Start()
    {
        if(ComboCooldown.Length != ComboMultiplier.Length)
        {
            Debug.LogError("Combo cooldown must have the same size as Combo Multiplier");
        }
        firstTimeAFK = true;
        lowCombo = false;
    }


    void Update()
    {
        //reset combo on cooldown
        if (getCoolDown())
        {
            ResetCombo();
        }

        ComboHandler.text = combo.ToString();

        antiAfkTimestamp += Time.deltaTime;
        if (antiAfkTimestamp > StartAFKSeconds)
        {
            handleAFK(antiAfkTimestamp);
        }

        if(!lowCombo)
        {
            if (combo < 3)
            {
                lowCombo = true;
                if (GameDialogUI != null)
                    GameDialogUI.StartCoroutine("LowCombo");
            }
            
        }

    }

    private void handleAFK(float timestamp)
    {
        int secondsAFK = (int) Math.Floor(timestamp);
        if (GameDialogUI != null && firstTimeAFK && secondsAFK == StartAFKSeconds)
        {
            GameDialogUI.WakeUp();
            firstTimeAFK = false;
        }

        if (antiAFKTime < secondsAFK && secondsAFK > StartAFKSeconds)
        {
            antiAFKTime = secondsAFK;
            ReputationHandler.Score += antiAfkPoints;
            
        }

    }

    public int GetComboMultiplier(int score)
    {
        float rep = score * (ComboMultiplier[combo] / 100f + 1f);
        return (int) rep;
    }

    public void IncreaseCombo()
    {
        comboModifierActions++;

        if (comboModifierActions < ComboActionModifier.Length - 1)
        {
            try
            {
                handleComboChange(ComboActionModifier[comboModifierActions]);
            }
            catch (IndexOutOfRangeException e)
            {
                Debug.LogError(e.Message + " Combo Modifier Actions -> check DummyManager prefab");
            }
        }

        //if(combo < 3)
        //    if(GameDialogUI != null)
        //        GameDialogUI.StartCoroutine("LowCombo");


    }

    private void handleComboChange(int comboValue)
    {

        if (comboValue < ComboMultiplier.Length - 1)
        {
            //if (ComboActionModifier[ComboActionModifier])
            combo = comboValue;

            ComboBaseParticle.GetComponent<ParticleSystem>().startSize = combo;

            ComboUpwardParticle.GetComponent<ParticleSystem>().startSize = 0.1f + (combo * 2f) / 100f;


            if (oldCombo < combo)
            {

                switch(combo)
                {
                    case 1:
                        WwiseInterface.Instance.PlayRewardSound(RewardHandle.ComboStart);
                        break;
                    case 2:
                        WwiseInterface.Instance.PlayRewardSound(RewardHandle.ComboBoost);
                        break;
                    case 3:
                        WwiseInterface.Instance.PlayRewardSound(RewardHandle.ComboBoost2);
                        break;
                    case 4:
                        WwiseInterface.Instance.PlayRewardSound(RewardHandle.ComboBoost3);
                        break;
                }
                oldCombo = combo;
            }
        }
    }

    public void DecreaseCombo()
    {
        if ( combo == 0)
        {
            return;
        }

        combo--;
        Debug.Log("Combo decreased " + combo);
    }

    public void ResetCombo()
    {


        comboModifierActions = 0;
        combo = 0;
        ComboBaseParticle.GetComponent<ParticleSystem>().startSize = 0.1f;
        ComboUpwardParticle.GetComponent<ParticleSystem>().startSize = 0.01f;
        if (combo != oldCombo)
        {
            WwiseInterface.Instance.PlayRewardSound(RewardHandle.ComboEnd);
            oldCombo = combo;
        }
    }
    
    //cooldown
    private bool getCoolDown()
    {
        if (comboTimeStamp >= Time.time)
        {
            return false;
        }
        return true;
    }

    public void resetCooldown()
    {
        try
        {
            comboTimeStamp = Time.time + ComboCooldown[combo];
        }
        catch (IndexOutOfRangeException e)
        {
            Debug.LogError(e.Message);
        }
    }

    public void ActionPerformed()
    {
        //antiAFKTime = 0;
        //antiAfkTimestamp = 0;
        //antiAfkPoints = AFKPointsDec;
    }


    internal float GetGlobalScore()
    {
        //calculate bonus
        repGain = getLocalScore(ReputationHandler.Score);
        
        float repGainDivision = 50;
        return repGain / repGainDivision;

    }

    private float getLocalScore(float score)
    {

        float time = 0;
        if (TimerObjectScript.Instance != null)
            time = TimerObjectScript.Instance.GetElapsedTime();
        // bonus
        //get time from  quest timer
        float bonus = score * Mathf.Exp(time * 3) * 0.15f;

        if (time == 0 || score > 0)
        {
            bonus = 0;
        }

        //Debug.Log("REP: " + score + " BONUS: " + bonus + " time: " + time + " TOTAL REP GAINED: " + score + bonus);
        return (score + bonus);
    }

    public int GetLocalScore()
    {
        return (int) repGain;
    }

    public int GetComboValue()
    {
        return combo;
    }

    public void onTouchAction()
    {

        antiAFKTime = 0;
        antiAfkTimestamp = 0;
        antiAfkPoints = AFKPointsDec;
        firstTimeAFK = true;
    }
}
