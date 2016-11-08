using UnityEngine;
using System.Collections.Generic;

public interface IObjective
{
    bool IsCompleted { get; }
    float SuccessRating { get; }
    bool IsChecked { get; }
    bool IsInvalid { get; }

    /// <summary>
    /// Checks the target against the objective. If the target 
    /// fits the objective, it saves the SuccessRating and sets
    /// IsChecked to true.
    /// </summary>
    /// <param name="gObj">An IObjectiveTarget that might complete the quest.</param>
    /// <returns>True if the check completes the objective. False otherwise. Note that this method will only return true once.</returns>
    bool CheckTarget(IObjectiveTarget gObj);

    /// <summary>
    /// Forces a check to be completed, using a list of potentielly
    /// valid IObjectiveTargets. If none of the targets provided
    /// can complete the objective, the IsInvalid flag is set to true.
    /// ForceCheck will never change IsChecked to false.
    /// </summary>
    /// <param name="gObjs">A list of potentially valid IObjectiveTarget objects</param>
    void ForceCheck(IEnumerable<IObjectiveTarget> gObjs);
}
