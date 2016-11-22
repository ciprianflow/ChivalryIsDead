using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum Difficulty
{
    Easy, Medium, Hard
}

public struct QuestDescription
{
    public string Title;
    public string Description;
    public Difficulty Difficulty;

    public QuestDescription(string title, string description, Difficulty difficulty)
    {
        Title = title;
        Description = description;
        Difficulty = difficulty;
    }
}
