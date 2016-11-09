using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TimerObjective : BaseObjective
{
    public TimerObjective(int targetID) : base(targetID) { }

    #region Interface methods
    public override bool CheckTarget(IObjectiveTarget gObj)
    {
        IsChecked = gObj.IsChecked = gObj.Health <= 0;
        // Should a completed timer have SuccessRating 1 or 0??
        SuccessRating = (Mathf.Clamp(gObj.Health, 0, gObj.MaxHealth) / gObj.MaxHealth);
        return IsChecked;
    }
    #endregion
}
