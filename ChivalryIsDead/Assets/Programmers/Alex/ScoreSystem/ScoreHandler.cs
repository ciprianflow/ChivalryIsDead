using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Base class for handling score changing events, such as taking damage, completing quests, 
/// or acting suspicious.
/// 
/// Any script that needs capability to change reputation, health or suspicion
/// (Health scripts, quest scripts, etc.) should inherit the ScorePublisher class 
/// and subscribe to this behaviour.
/// </summary>
public class ScoreHandler : MonoBehaviour {
    public int Score;

    public void Subscribe(ScorePublisher repPub)
    {
        repPub.ChangeScoreEvent += HandleScoreChange;
    }

    private void HandleScoreChange(object sender, ScoreEventArgs e)
    {
        Score += e.ScoreChange;
    }
}
