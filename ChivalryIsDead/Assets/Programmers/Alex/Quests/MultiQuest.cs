using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Used to create quests in which one of multiple quests can be completed.
/// A single objective completed in the MultiQuest will trigger a ForceCheck of all objectives on the MultiQuest.
/// </summary>
public class MultiQuest : BaseQuest
{
    public new bool IsChecked { get { return Objectives.Any(o => o.IsChecked); } }

    public MultiQuest() : this("", "", Difficulty.Easy) { }
    public MultiQuest(string title, string description, Difficulty difficulty) : base(title, description, difficulty)
    { }

    public override bool CheckTarget(IObjectiveTarget gObj)
    {
        var innerObjIsChecked = base.CheckTarget(gObj);

        if (innerObjIsChecked && IsChecked) {
            ForceCheck(new List<IObjectiveTarget>()); // TODO: URGENT: Should be the actual in-game list of IObjectiveTargets.
        }

        // Consider the return logic.
        // Should maybe return whether the quest itself is checked.
        return innerObjIsChecked;
    }
}
