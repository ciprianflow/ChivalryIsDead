using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class DummyObjectiveTarget : IObjectiveTarget
{
    public int Health
    {
        get { return UnityEngine.Random.Range(0, 100); }
    }

    public int ID
    {
        get { return -666; }
    }

    public bool IsChecked
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public int MaxHealth
    {
        get { return 100; }
    }

    bool IObjectiveTarget.IsChecked
    {
        get
        {
            throw new NotImplementedException();
        }

        set
        {
            throw new NotImplementedException();
        }
    }
}
