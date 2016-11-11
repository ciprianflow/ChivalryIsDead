using UnityEngine;
using System.Collections.Generic;
using System;

[Flags]
public enum monsterType {Ranged = 12, Melee = 11, Suicide = 13, Sheep = 21}

[System.Serializable]
public class AreaProperty
{
    [SerializeField] public Color AreaColor;
    [SerializeField] public monsterType SpawnType;
    [SerializeField] public int MaxSpawn;

    public AreaProperty(Color c, monsterType t, int n)
    {
        AreaColor = c;
        SpawnType = t;
        MaxSpawn = n;
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
        properties.Add(new AreaProperty(new Color(0.4f, 0.9f, 0.1f, 1f), monsterType.Melee, 0));
    }

    public void ResetAll()
    {
        Areas = new List<Rect>();
        properties = new List<AreaProperty>();
    }

    public Dictionary<int, List<Rect>> GetSortedAreas()
    {
        Dictionary<int, List<Rect>> sortedAreas = new Dictionary<int, List<Rect>>();
        int[] monsterIDs = (int[])Enum.GetValues(typeof(monsterType));

        for (int i = 0; i < monsterIDs.Length; i++)
        {
            sortedAreas.Add(monsterIDs[i], new List<Rect>());
        }

        for(int i = 0; i < Areas.Count; i++)
        {
            int ID = (int)properties[i].SpawnType;
            if (sortedAreas.ContainsKey(ID))
                sortedAreas[ID].Add(Areas[i]);
        }
        return sortedAreas;
    }

    internal Dictionary<int, List<int>> GetSortedMaxSpawn()
    {
        Dictionary<int, List<int>> sortedMaxSpawn = new Dictionary<int, List<int>>();
        int[] monsterIDs = (int[])Enum.GetValues(typeof(monsterType));

        for (int i = 0; i < monsterIDs.Length; i++)
        {
            sortedMaxSpawn.Add(monsterIDs[i], new List<int>());
        }

        for (int i = 0; i < Areas.Count; i++)
        {
            int ID = (int)properties[i].SpawnType;
            if (sortedMaxSpawn.ContainsKey(ID))
                sortedMaxSpawn[ID].Add(properties[i].MaxSpawn);
        }
        return sortedMaxSpawn;
    }

    public Vector3 GetRandomPointOnRect(Rect r)
    {

        float x = UnityEngine.Random.Range(0, r.width);
        float y = UnityEngine.Random.Range(0, r.height);
        return new Vector3(x + r.position.x, 0, y + r.position.y);

    }

}
