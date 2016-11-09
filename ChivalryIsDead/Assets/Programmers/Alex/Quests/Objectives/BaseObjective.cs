using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BaseObjective : IObjective
{
    public float SuccessRating { get; protected set; }
    public bool IsCompleted { get { return SuccessRating > 0; } }
    public bool IsChecked { get; protected set; }
    public bool IsInvalid { get; protected set; }

    public int targetID { get; private set; }

    public BaseObjective(int targetID)
    {
        this.targetID = targetID;
    }

    public virtual bool CheckTarget(IObjectiveTarget gObj)
    {
        // As long as this is accessed through ForceCheck(), neither of the below can be true.
        if (IsChecked)
            return true;
        if (gObj.IsChecked)
            return false;

        // Flags the target and the objective as checked.
        return IsChecked = gObj.IsChecked = gObj.ID == targetID;
    }

    public virtual void ForceCheck(IEnumerable<IObjectiveTarget> gObjs)
    {
        var objTargetEnum = gObjs.GetEnumerator();

        while (!IsChecked && objTargetEnum.MoveNext() && !objTargetEnum.Current.IsChecked) {
            CheckTarget(objTargetEnum.Current);
        }
        
        if (!IsChecked) {
            IsChecked = true;
            IsInvalid = true;
        }
    }
}
