using System;

public interface IScorePublisher
{
    event EventHandler<ScoreEventArgs> ChangeScoreEvent;

    void OnChangeScoreEvent(ScoreEventArgs e);
}
