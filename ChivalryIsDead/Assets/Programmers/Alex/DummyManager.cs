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

    public float ComboCooldown = 10f;
    public int[] ComboMultiplier;


    public Text ComboHandler;

    public ScoreHandler ReputationHandler;
    
    public ScoreHandler SuspicionHandler;
    public ScoreHandler DaysRemaining;

    private float comboTimeStamp = 0;

    private int combo = 0;


    void Awake()
    {
        StaticIngameData.dummyManager = this;
        DummyManager.dummyManager = this;
    }


    void Update()
    {
        //reset combo on cooldown
        if (getCoolDown())
        {
            ResetCombo();
        }

        ComboHandler.text = combo.ToString();
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
            Debug.Log("Combo increased: " + combo);
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
        comboTimeStamp = Time.time + ComboCooldown;
    }

    internal float GetGlobalScore()
    {

        float repGain = ReputationHandler.Score;
        float repGainDivision = 50;
        return repGain  / repGainDivision;

    }
}
