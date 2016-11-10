using System;
using System.Collections.Generic;
using UnityEngine;

class PlayerBehaviour : ScorePublisher
{

    public int ScoreChange { get; set; }
    public string ScoreHandle { get; set; }

    private DummyManager dummyManager;

    public PlayerBehaviour(string handle)
    {
        dummyManager = DummyManager.dummyManager;

        switch (handle)
        {
            case "rep":
                dummyManager.ReputationHandler.Subscribe(this); break;
            case "susp":
                Debug.Log("NOT SUPPORTED"); break;
            case "days":
                Debug.Log("NOT SUPPORTED"); break;
            default:
                break;
        }
    }

    public void ChangeRepScore(int score)
    {
        if (score < 0)
        {
            ScoreChange = dummyManager.GetComboMultiplier(score);
            
            //increase combo
            dummyManager.IncreaseCombo();
            //reset cooldown
            dummyManager.resetCooldown();
        }
        else
        {      
            Reset();
            ScoreChange = dummyManager.GetComboMultiplier(score);
        }
    }
    
    //reset combo
    public void Reset()
    {
        dummyManager.ResetCombo();
    }

    public void Invoke()
    {
        OnChangeScoreEvent(new ScoreEventArgs(ScoreChange));
    }




}
