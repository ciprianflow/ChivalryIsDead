using System;

public class ScoreEventArgs : EventArgs {

    public int ScoreChange { get; set; }

    public ScoreEventArgs(int change) { ScoreChange = change; }
}
