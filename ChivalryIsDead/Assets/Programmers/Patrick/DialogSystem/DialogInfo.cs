using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[CreateAssetMenu("Assets\Menu")]
[CreateAssetMenu(fileName = "Dialog", menuName = "Dialog", order = 1)]
public class DialogInfo : ScriptableObject
{

    //public List<string> spawnPattern;


    //public List<Dialog> test;

    public int Dialog;

    public string[] Name;
    public string[] Text;
    public float[] Wait;

    



}
