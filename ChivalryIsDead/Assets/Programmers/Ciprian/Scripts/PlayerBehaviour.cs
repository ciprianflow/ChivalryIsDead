using System;
using System.Collections.Generic;
using UnityEngine;

class PlayerBehaviour : ScorePublisher
{

    public int ScoreChange { get; set; }
    public string ScoreHandle { get; set; }

    public PlayerBehaviour(string handle)
    {
        switch (handle)
        {
            case "rep":
                DummyManager.dummyManager.ReputationHandler.Subscribe(this); break;
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
            ScoreChange = score * DummyManager.dummyManager.Combo;
            DummyManager.dummyManager.Combo++;
            Debug.Log("Combo increased: " + DummyManager.dummyManager.Combo);


            DummyManager.dummyManager.comboTimeStamp = Time.time + DummyManager.dummyManager.ComboCooldown;
        }
        else
        {
            ScoreChange = score;
            Reset();
        }
    }

    
    //reset combo
    public void Reset()
    {
        DummyManager.dummyManager.Combo = 1;
    }

    public void Invoke()
    {

        OnChangeScoreEvent(new ScoreEventArgs(ScoreChange));
    }




}
