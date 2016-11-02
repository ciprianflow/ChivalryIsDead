using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class TargetObjective : IObjective
{
    public IQuestTarget QuestTarget;

    public bool IsCompleted() { return QuestTarget.GetHealth() > 0; }
    public float GetSuccessRating() { return (float)QuestTarget.GetHealth() / (float)QuestTarget.GetMaxHealth(); }
}
