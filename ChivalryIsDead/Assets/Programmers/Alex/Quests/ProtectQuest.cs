using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProtectQuest : IObjective
{
    public string Title;
    public string Description;
    public List<IObjective> Objectives = new List<IObjective>();

    private float SuccessRating { get { return Objectives.Average(o => o.GetSuccessRating()); } }

    public float GetSuccessRating() { return SuccessRating; }

    public bool IsCompleted() {
        if (Objectives.Any(o => o.IsCompleted()))
            return true;
        else
            return false;
    }
}
