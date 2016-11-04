using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class DestroyTargetObjective : BaseObjective
{
    public DestroyTargetObjective(int targetID) : base(targetID)
    { }

    #region Interface methods
    public override bool CheckTarget(IObjectiveTarget gObj)
    {
        if (base.CheckTarget(gObj)) {
            SuccessRating = 1 - (Mathf.Clamp(gObj.Health, 0, gObj.MaxHealth) / gObj.MaxHealth);
            return true;
        }
        else {
            return false;
        }
    }
    #endregion
}
