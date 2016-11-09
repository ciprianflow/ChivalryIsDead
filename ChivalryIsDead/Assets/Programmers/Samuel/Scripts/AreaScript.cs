using UnityEngine;
using System.Collections.Generic;

public enum monsterType {Ranged, Melee, Suicide, Sheep}

[System.Serializable]
public class AreaProperty
{
    [SerializeField] public Color AreaColor;
    [SerializeField] public monsterType SpawnType;

    public AreaProperty(Color c, monsterType t)
    {
        AreaColor = c;
        SpawnType = t;
    }
}

public class AreaScript : MonoBehaviour {

    //EDITOR VARIABLES
    [SerializeField]
    public List<Rect> Areas;
    [SerializeField]
    public List<AreaProperty> properties = new List<AreaProperty>();

    public List<Rect> GetAllAreas()
    {
        return Areas;
    }

    public Rect GetArea(int index)
    {
        return Areas[index];
    }

    public void RemoveArea(int index)
    {
        if (index > Areas.Count - 1)
            return;

        Areas.RemoveAt(index);
        properties.RemoveAt(index);
    }

    public void AddArea()
    {
        Debug.Log("Adding SpawnArea");
        Areas.Add(new Rect(0, 0, 2f, 2f));
        properties.Add(new AreaProperty(new Color(0.4f, 0.9f, 0.1f, 1f), monsterType.Melee));
    }

    public void ResetAll()
    {
        Areas = new List<Rect>();
        properties = new List<AreaProperty>();
    }

}
