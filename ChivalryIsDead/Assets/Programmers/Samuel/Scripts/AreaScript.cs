using UnityEngine;
using System.Collections.Generic;

public class AreaScript : MonoBehaviour {

    //EDITOR VARIABLES
    [SerializeField]
    public List<Rect> Areas;
    [SerializeField]
    public List<Color> AreaColor = new List<Color>();

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
        AreaColor.RemoveAt(index);
    }

    public void AddArea()
    {
        Debug.Log("Adding SpawnArea");
        Areas.Add(new Rect(0, 0, 2f, 2f));
        AreaColor.Add(new Color(0.4f, 0.9f, 0.1f, 1f));
    }

    public void ResetAll()
    {
        Areas = new List<Rect>();
        AreaColor = new List<Color>();
    }

}
