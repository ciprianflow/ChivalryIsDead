using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

public class DummyCreateQuestParameters
{
    private const string dataFolderPath = @"Assets/Resources/data/";
    private const string assetFileName = "QuestParameters.asset";

    [MenuItem("Assets/Create QuestParameters")]
    public static void CreateQP()
    {
        var QP = ScriptableObject.CreateInstance<QuestParameters>();
        AssetDatabase.CreateAsset(QP, dataFolderPath + assetFileName);
    }
}
#endif