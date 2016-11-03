using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class DummyObjective : IObjective
{
    Random rGen = new Random();

    public float GetSuccessRating()
    {
        if (IsCompleted())
            return (float)rGen.NextDouble();
        else return 0f;
    }

    public bool IsCompleted()
    {
        return Convert.ToBoolean(rGen.Next(2));
    }
}
