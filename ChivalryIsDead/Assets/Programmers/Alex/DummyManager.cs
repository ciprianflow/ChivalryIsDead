using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class DummyManager : MonoBehaviour
{
    public static DummyManager dummyManager;

    public int Combo = 1;
    public float ComboCooldown = 10f;

    public float comboTimeStamp = 0;

    
    public ScoreHandler ReputationHandler;

    public Text ComboHandler;


    public ScoreHandler SuspicionHandler;
    public ScoreHandler DaysRemaining;

    void Awake()
    {
        DummyManager.dummyManager = this;
    }


    void Update()
    {
        //reset combo on cooldown
        if (getCoolDown())
        {
            Combo = 1;  
        }

        ComboHandler.text = Combo.ToString();
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
}
