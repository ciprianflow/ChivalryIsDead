using UnityEngine;

public interface IObjective
{
    bool IsCompleted { get; }
    float SuccessRating { get; }
    bool IsChecked { get; }

    bool CheckTarget(IObjectiveTarget gObj);
}
