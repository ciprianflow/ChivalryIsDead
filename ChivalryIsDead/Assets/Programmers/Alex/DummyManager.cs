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

    [Header("Anti AFK")]
    public int StartAFKSeconds = 1;
    public int AFKPointsDec = 10;

    public Text ComboHandler;

    public ScoreHandler ReputationHandler;
    
    public ScoreHandler SuspicionHandler;
    public ScoreHandler DaysRemaining;

    public Gameplay_Dialog GameDialogUI;

    private float comboTimeStamp = 0;

    private int combo = 0;

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

        if (GameDialogUI != null)
        {
            //GameDialogUI.getCom
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
        if (combo < ComboMultiplier.Length - 1)
        {
            combo++;
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
        combo = 0;

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
        float repGainDivision = 50;
        return repGain  / repGainDivision;

    }

    public int GetComboValue()
    {
        return combo;
    }
}
