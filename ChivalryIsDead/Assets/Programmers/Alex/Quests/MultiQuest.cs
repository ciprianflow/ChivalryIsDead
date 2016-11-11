using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

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

        if (innerObjIsChecked && IsChecked)
        {
            Debug.LogWarning("CheckTarget with only 1 object was called on MultiQuest object! All remaining objectives will be marked invalid.");
            // TODO: URGENT: Should be the actual in-game list of IObjectiveTargets.
            ForceCheck(new List<IObjectiveTarget>()); 
        }

        // Consider the return logic.
        // Should maybe return whether the quest itself is checked.
        return innerObjIsChecked;
    }

    public bool CheckTarget(IObjectiveTarget initTarget, IEnumerable<IObjectiveTarget> gObjs)
    {
        var innerObjIsChecked = base.CheckTarget(initTarget);

        if (IsChecked) { // innerObjIsChecked && IsChecked) {
            //Debug.LogWarning("CheckTarget with only 1 object was called on MultiQuest object! All remaining objectives will be marked invalid.");
            // TODO: URGENT: Should be the actual in-game list of IObjectiveTargets.
            ForceCheck(gObjs);
        }

        // Consider the return logic.
        // Should maybe return whether the quest itself is checked.
        return innerObjIsChecked;

        //var innerObjIsChecked = false;
        //foreach (IObjectiveTarget ot in gObjs)
        //{
        //    innerObjIsChecked = base.CheckTarget(ot);
        //    if (innerObjIsChecked && IsChecked)
        //    {
        //        ForceCheck(gObjs);
        //    }
        //}

        //return innerObjIsChecked;
    }
}
