using System;
using UnityEngine;

// Inherit from this class to easily communicate with the ScoreHandler.
public class ScorePublisher : IScorePublisher
{
    public event EventHandler<ScoreEventArgs> ChangeScoreEvent;

    public virtual void OnChangeScoreEvent(ScoreEventArgs e) // NOT PUBLIC
    {
        // Simplification has been avoided for thread safety and debugging.
        EventHandler<ScoreEventArgs> temp = ChangeScoreEvent;
        if (temp != null)
            temp(this, e);
        else
            Debug.LogWarning("ChangeScore event fired with no receiver. Subscribe the ReputationPublisher to a ReputationHandler.");
    }
}
