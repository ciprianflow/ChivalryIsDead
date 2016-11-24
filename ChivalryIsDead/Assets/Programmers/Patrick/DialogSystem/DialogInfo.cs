using UnityEngine;

//[CreateAssetMenu("Assets\Menu")]
[CreateAssetMenu(fileName = "Dialog", menuName = "Dialog", order = 1)]
public class DialogInfo : ScriptableObject
{


    [HideInInspector]
    public int Dialog;

    public string[] Name;

    [TextArea(3, 10)]
    public string[] Text;

    public float[] Wait;

    public string[] Sound;

    


}





