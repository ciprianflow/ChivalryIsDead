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

    public int targetID { get; private set; }

    public BaseObjective(int targetID)
    {
        this.targetID = targetID;
    }

    public virtual bool CheckTarget(IObjectiveTarget gObj)
    {
        if (IsChecked)
            throw new Exception("Checked objective is being checked again! Undefined behaviour.");

        return IsChecked = gObj.ID == targetID;
    }
}
