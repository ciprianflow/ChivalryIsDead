using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ProtectTargetObjective : BaseObjective
{
    public ProtectTargetObjective(int targetID) : base(targetID)
    { }

    #region Interface methods
    public override bool CheckTarget(IObjectiveTarget gObj)
    {
        if (base.CheckTarget(gObj)) {
            SuccessRating = (Mathf.Clamp(gObj.Health, 0, gObj.MaxHealth) / gObj.MaxHealth);
            return true;
        }
        else {
            return false;
        }
    }
    #endregion
}
