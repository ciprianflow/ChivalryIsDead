using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class BaseQuest : IQuest
{
    public QuestDescription Description;
    public List<IObjective> Objectives = new List<IObjective>();

    public BaseQuest() : this("", "", Difficulty.Easy) { }
    public BaseQuest(string title, string description, Difficulty difficulty)
    { Description = new QuestDescription(title, description, difficulty); }

    public QuestDescription GetDescription() { return Description; }

    public float GetSuccessRating() { return Objectives.Average(o => o.GetSuccessRating()); }

    public bool IsCompleted()
    {
        if (Objectives.Any(o => o.IsCompleted()))
            return true;
        else
            return false;
    }

}