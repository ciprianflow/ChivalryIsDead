using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IQuest : IObjective
{
    QuestData Data { get; }
    QuestDescription Description { get; }
    List<IObjective> Objectives { get; }
    float ReputationChange { get; }
}
