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

    public void Invoke()
    {
        OnChangeScoreEvent(new ScoreEventArgs(ScoreChange));
    }



}
