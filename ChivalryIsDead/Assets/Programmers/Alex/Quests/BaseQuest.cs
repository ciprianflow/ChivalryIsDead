using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class BaseQuest : IQuest
{
    #region Interface properties
    #region IQuest
    public QuestData Data { get; set; }
    public QuestDescription Description { get; private set; }
    private List<IObjective> objectives = new List<IObjective>();
    public List<IObjective> Objectives { get { return objectives; } }
    public float ReputationChange { get; set; }
    #endregion

    #region IObjective
    public float SuccessRating { get { return Objectives.Average(o => o.SuccessRating); } }
    public bool IsCompleted { get { return SuccessRating > 0; } }
    public bool IsChecked { get { return Objectives.All(o => o.IsChecked); } }
    public bool IsInvalid { get { return Objectives.All(o => o.IsInvalid); } }
    #endregion
    #endregion


    public BaseQuest() : this("", "", Difficulty.Easy) { }
    public BaseQuest(string title, string description, Difficulty difficulty)
    {
        Description = new QuestDescription(title, description, difficulty);
    }

    public IEnumerable<IObjective> GetAllObjectives()
    {
        foreach (IObjective o in Objectives) {
            BaseQuest oAsBase;
            if ((oAsBase = (o as BaseQuest)) == null) {
                yield return o;
                //currentList.Add(o);
            } else {
                foreach (IObjective inner_o in oAsBase.GetAllObjectives())
                    yield return inner_o;
                //currentList.AddRange(oAsBase.GetAllObjectives(currentList));
            }
        }
        //return currentList;
    }

    public virtual bool CheckTarget(IObjectiveTarget gObj)
    {
        var objEnumerator = Objectives.GetEnumerator();

        while (objEnumerator.MoveNext())
            objEnumerator.Current.CheckTarget(gObj);

        return IsChecked;
    }

    public virtual void ForceCheck(IEnumerable<IObjectiveTarget> gObjs)
    {
        foreach (IObjective o in Objectives) {
            o.ForceCheck(gObjs);
        }
    }
}