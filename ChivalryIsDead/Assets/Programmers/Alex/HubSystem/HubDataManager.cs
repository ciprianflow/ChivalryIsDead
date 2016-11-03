using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections.Generic;

public class HubDataManager : MonoBehaviour {

    private const string hubDataPath = "Assets/HubData.asset";
    private HubData currentHubData;

    #region HubData properties
    public int CurrentReputation {
        get {
            if (currentHubData == null) {
                Debug.LogWarning("Attempted to access HubData.CurrentReputation without a HubData object being present.");
                return 0;
            }
            return currentHubData.GlobalReputation;
        }
    }
    public int DaysLeft {
        get {
            if (currentHubData == null) {
                Debug.LogWarning("Attempted to access HubData.DaysLeft without a HubData object being present.");
                return 0;
            }
            return currentHubData.DaysLeft;
        }
    }
    public List<IObjective> AvailableQuests
    {
        get {
            if (currentHubData == null) {
                Debug.LogWarning("Attempted to access HubData.AvailableQuests without a HubData object being present.");
                return new List<IObjective>();
            }
            return currentHubData.AvailableQuests;
        }
    }
    #endregion

    public Text QueueText;
    public GameObject ContentPane;
    public Text QuestTitle;

    void Start () {
        UpdateQuests();
        QueueText.text = currentHubData.QueueLength.ToString();
        CreateQuestUIElements();
	}
	
    // Should be used when game is restarted or booted.
    public void UpdateQuests()
    {
        var hubData = LoadHubData();
        hubData.AvailableQuests = DummyQuestGenerator.GenerateMultipleQuests(hubData.QueueLength);
        AssetDatabase.SaveAssets();
        currentHubData = hubData;
    }

    // Should be used when player returns from a quest.
    public void PushToHubData(int repChange) { PushToHubData(repChange, -1); }
    public void PushToHubData(int repChange, int dayChange)
    {
        var hubData = LoadHubData();
        hubData.GlobalReputation += repChange;
        hubData.DaysLeft += dayChange;
        hubData.AvailableQuests = DummyQuestGenerator.GenerateMultipleQuests(hubData.QueueLength);
        AssetDatabase.SaveAssets();
        currentHubData = hubData;
    }

    private HubData LoadHubData()
    {
        var hubData = AssetDatabase.LoadAssetAtPath<HubData>(hubDataPath);
        if (hubData == null) {
            hubData = ScriptableObject.CreateInstance<HubData>();
            AssetDatabase.CreateAsset(hubData, hubDataPath);
        }
        return hubData;
    }

    // TODO: Dummy method, shouldn't make it into the final game. Update to generic alternative.
    private void CreateQuestUIElements()
    {
        var curQIdx = 0;
        foreach (IObjective o in AvailableQuests) {
            var oAsQuest = o as ProtectQuest; // Change so that there's an interface between Quests and IObjectives.
            Text newQuestText = Instantiate<Text>(QuestTitle);
            newQuestText.rectTransform.SetParent(ContentPane.transform);
            newQuestText.text = oAsQuest.Title;
            newQuestText.transform.localPosition += new Vector3(165, -28 * curQIdx++, 0);
            newQuestText.gameObject.SetActive(true);
        }
        var contentTransform = ContentPane.transform as RectTransform;
        contentTransform.sizeDelta = new Vector2(contentTransform.sizeDelta.x, 29 * AvailableQuests.Count);
    }
}
