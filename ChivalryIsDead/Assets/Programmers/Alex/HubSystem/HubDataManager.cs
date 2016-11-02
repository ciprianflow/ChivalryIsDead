using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;

public class HubDataManager : MonoBehaviour {

    private const string hubDataPath = "Assets/HubData.asset";
    private HubData currentHubData;

    public Text QueueText;

    // Use this for initialization
    void Start () {
        UpdateHubData();
        QueueText.text = currentHubData.QueueLength.ToString();
	}
	
    public void UpdateHubData()
    {
        var hubData = AssetDatabase.LoadAssetAtPath<HubData>(hubDataPath);
        if (hubData == null) {
            hubData = ScriptableObject.CreateInstance<HubData>();
            AssetDatabase.CreateAsset(hubData, hubDataPath);
        } else {
            hubData.AvailableQuests = DummyQuestGenerator.GenerateMultipleQuests(hubData.QueueLength);
        }
        AssetDatabase.SaveAssets();
        currentHubData = hubData;
    }
}
