using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class DummyManager : MonoBehaviour
{
    public static DummyManager dummyManager;

    [Header("Combo values")]

    public float[] ComboCooldown;
    public int[] ComboMultiplier;
    public int[] ComboActionModifier;

    [Header("Anti AFK")]
    public int StartAFKSeconds = 1;
    public int AFKPointsDec = 10;

    public Text ComboHandler;

    public ScoreHandler ReputationHandler;
    
    public ScoreHandler SuspicionHandler;
    public ScoreHandler DaysRemaining;

    [Header("Other stuff")]
    public Gameplay_Dialog GameDialogUI;
    public GameObject ComboBaseParticle;
    public GameObject ComboUpwardParticle;

    private float comboTimeStamp = 0;

    private int combo = 0;
    //used to track actions that modify the combo
    private int comboModifierActions = 0;


    private float antiAfkTimestamp;
    private int antiAfkPoints = 10;
    private int antiAFKTime;


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
        handleAFK(antiAfkTimestamp);

    }

    private void handleAFK(float timestamp)
    {
        int secondsAFK = (int) Math.Floor(timestamp);

        if (GameDialogUI != null && secondsAFK == StartAFKSeconds)
        {
            GameDialogUI.WakeUp();
        }

        if (antiAFKTime < secondsAFK && secondsAFK > StartAFKSeconds)
        {
            antiAFKTime = secondsAFK;
            ReputationHandler.Score += antiAfkPoints;
        
        }

    }

    public int GetComboMultiplier(int score)
    {
        //combo multiplier
        return score * (ComboMultiplier[combo] / 100 + 1);
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


    }

    private void handleComboChange(int comboValue)
    {

        if (comboValue < ComboMultiplier.Length - 1)
        {
            //if (ComboActionModifier[ComboActionModifier])
            combo = comboValue;

            ComboBaseParticle.GetComponent<ParticleSystem>().startSize = combo;

            ComboUpwardParticle.GetComponent<ParticleSystem>().startSize = 0.1f + (combo * 2f) / 100f;
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
        antiAFKTime = 0;
        antiAfkTimestamp = 0;
        antiAfkPoints = AFKPointsDec;
    }


    internal float GetGlobalScore()
    {

        float repGain = ReputationHandler.Score;

        /* bonus 
        //get time from  quest timer
        float time = 0.5f;
        float bonus = repGain * Mathf.Exp(time * 3) * 0.15f;

        if (time == 0 || repgain > 0)
        {
            bonus = 0;
        }

        float repGain = repGain + bonus;
        */

        float repGainDivision = 50;
        return repGain  / repGainDivision;

    }

    public int GetComboValue()
    {
        return combo;
    }
}
