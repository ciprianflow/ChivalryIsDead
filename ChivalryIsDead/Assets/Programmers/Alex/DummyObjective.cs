using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class DummyObjective : IObjective
{
    Random rGen = new Random();

    public bool IsCompleted { get { return SuccessRating > 0; } }
    public bool IsChecked { get; private set; }
    public float SuccessRating
    {
        get { return 1f; }
    }
    public bool IsInvalid { get; set; }

    public bool CheckTarget(IObjectiveTarget gObj)
    {
        return true;
    }

    public void ForceCheck(IEnumerable<IObjectiveTarget> gObjs)
    {
        throw new NotImplementedException();
    }
}
