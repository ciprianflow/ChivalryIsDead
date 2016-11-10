using System.Linq;
using UnityEngine;
using UnityEngine.UI;
//using UnityEditor;
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
    public List<IQuest> AvailableQuests
    {
        get {
            if (currentHubData == null) {
                Debug.LogWarning("Attempted to access HubData.AvailableQuests without a HubData object being present.");
                return new List<IQuest>();
            }
            return currentHubData.AvailableQuests;
        }
    }
    #endregion

    public int MaximumReputation = 2000;
    public int TotalDays = 14;

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
        hubData.AvailableQuests = new List<IQuest>();

        // TODO : New quest spawning system??
        QuestGenerator QG = new QuestGenerator();
        QuestData QD = new QuestData();
        MultiQuest MQ = QG.GenerateMultiQuest(out QD);
        //MQ.Description = new QuestDescription("ok", "this is dog", Difficulty.Easy);
        hubData.AvailableQuests.Add(MQ);

        //AssetDatabase.SaveAssets();
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
        //AssetDatabase.SaveAssets();
        currentHubData = hubData;
    }

    private HubData LoadHubData()
    {
        HubData hubData = null;
        //var hubData = AssetDatabase.LoadAssetAtPath<HubData>(hubDataPath);
        if (hubData == null) {
            hubData = ScriptableObject.CreateInstance<HubData>();
           // AssetDatabase.CreateAsset(hubData, hubDataPath);
        }
        return hubData;
    }


    /// <summary>
    /// TODO: Delete and replace with correct UI data, preferably this should be removed entirely.
    /// </summary>
    #region UI Specific
    private void ClearQuestUIElements()
    {
        foreach (Transform gObj in ContentPane.GetComponentsInChildren<Transform>()) {
            if (gObj.name != ContentPane.name && gObj.name != QuestTitle.name)
                GameObject.Destroy(gObj.gameObject);
        }
    }

    // TODO: Dummy method, shouldn't make it into the final game. Update to generic or UI specific alternative.
    private void CreateQuestUIElements()
    {
        var curQIdx = 0;
        foreach (IObjective o in AvailableQuests) {
            var oAsQuest = o as BaseQuest;
            Text newQuestText = Instantiate<Text>(QuestTitle);
            newQuestText.rectTransform.SetParent(ContentPane.transform);
            newQuestText.rectTransform.anchoredPosition = new Vector2(95, 0);
            newQuestText.text = oAsQuest.Description.Title;
            newQuestText.transform.localPosition += new Vector3(0, -28 * curQIdx++, 0);
            newQuestText.gameObject.SetActive(true);
        }
        var contentTransform = ContentPane.transform as RectTransform;
        contentTransform.sizeDelta = new Vector2(contentTransform.sizeDelta.x, 29 * AvailableQuests.Count);
    }

    // TODO: Semi-Dummy, completes a quest. Should be refactored to enter "Quest Mode".
    public void SelectQuest(Text questName)
    {
        var selectedQ = AvailableQuests.FirstOrDefault(q => q.Description.Title == questName.text);
        if (selectedQ != null) { 
            Debug.Log("Found quest with title '" + selectedQ.Description.Title + "'");
            //CompleteQuest(selectedQ);
            LoadQuest(selectedQ);
        }
        else
            Debug.LogWarning("Didn't find selected quest!");
    }
    #endregion

    private void LoadQuest(IQuest quest)
    {

        QuestManager.currQuest = (MultiQuest)quest;
        UnityEngine.SceneManagement.SceneManager.LoadScene("SandraCopy");

    }

    // TODO: Needs refactoring; Reputation change behaviour not specified properly.
    private void CompleteQuest(IQuest quest)
    {
        var qSuccessRating = quest.SuccessRating;
        int repChange;
        if (qSuccessRating > 0.5f) {
            repChange = (int)(qSuccessRating * (float)quest.Description.Difficulty);
        } else {
            repChange = -1 * (int)((1 - qSuccessRating) * (float)quest.Description.Difficulty);
        }

        // Save changes.
        ClearQuestUIElements();
        PushToHubData(repChange);
        CreateQuestUIElements();
        QueueText.text = currentHubData.QueueLength.ToString();
    }
}
