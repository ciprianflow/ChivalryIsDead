using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using UnityEditor;
using System.Collections.Generic;
using System;

public class HubDataManager : MonoBehaviour {

    private static string hubDataPath;
    private HubData currentHubData;

    int currSelectedQuestIndex = -1;

    #region HubData properties
    public float CurrentReputation {
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

    public PeasantLineScript peasantLineScript;

    //public int MaximumReputation = 2000;
    //public int TotalDays = 14;

    public GameObject DLCPane;
    public GameObject ContentPane;
    public GameObject QuestButton;
    public GameObject QuestLetter;
    public GameObject WinScreen;
    public Text DaysLeftText;
    public Image RingImg;

    void Awake()
    {
        hubDataPath = Application.persistentDataPath + "/HubData.json";

        if (StaticData.currQuest == null)
            UpdateQuests();
        else
            PushToHubData(StaticData.currQuest.ReputationChange);
    }

    void Start () {

        checkForWin();
        peasantLineScript.FillPeasantLine();
        UpdateUIText();
        UpdateUI();
        CreateQuestUIElements();

    }

    /// <summary>
    /// Updates the HubData instance in the HubDataManager.
    /// This will expose available quests, but not change the quest generation seed of the HubData.
    /// Should be used when game is restarted or booted.
    /// </summary> 
    public void UpdateQuests()
    {
        var hubData = LoadHubData();
        RefreshData(hubData);
    }

    /// <summary>
    /// Updates the HubData instance in the HubDataManager.
    /// This will expose available quests, and change the quest generation seed of the HubData.
    /// Should be used when player returns from a quest.
    /// </summary>
    /// <param name="repChange"></param>
    public void PushToHubData(float repChange) { PushToHubData(repChange, -1); }
    public void PushToHubData(float repChange, int dayChange)
    {
        var hubData = LoadHubData();
        hubData.GlobalReputation += repChange;
        hubData.DaysLeft += dayChange;
        hubData.RandomSeed = UnityEngine.Random.Range(0, int.MaxValue);

        RefreshData(hubData);
    }

    public void RefreshData(HubData hubData)
    {
        currentHubData = hubData;
        StaticData.Reputation = CurrentReputation;
        StaticData.daysLeft = DaysLeft;
        hubData.GenerateQuests();

        SaveHubData(hubData);
    }

    public static void ResetHubData()
    {
        var hubData = new HubData();
        SaveHubData(hubData);
    }

    #region Quest Generation
    private void ClearQuestUIElements()
    {
        foreach (Transform gObj in ContentPane.GetComponentsInChildren<Transform>()) {
            if (gObj.name != ContentPane.name && gObj.name != QuestButton.name)
                GameObject.Destroy(gObj.gameObject);
        }
    }


    // TODO: Dummy method, shouldn't make it into the final game. Update to generic or UI specific alternative.
    private void CreateQuestUIElements()
    {
        for(int i = 0; i < AvailableQuests.Count; i++) { 
        //foreach (IObjective o in AvailableQuests) {
            BaseQuest oAsQuest = (BaseQuest)AvailableQuests[i];
            GameObject QuestButtonObj = Instantiate(QuestButton);
            QuestButtonObj.transform.SetParent(ContentPane.transform);

            Text newQuestText = QuestButtonObj.transform.GetComponentInChildren<Text>();
            // TODO : Name doesnt load from quest description
            //newQuestText.text = oAsQuest.Description.Title;
            newQuestText.text = "Quest";

            Button b = newQuestText.transform.parent.GetComponent<Button>();
            int newI = i;
            b.onClick.AddListener(() => SelectQuest(newI));

            peasantLineScript.PushQuestToPeasant(i, newI, oAsQuest);
        }

        GenerateDLCQuest();
    }

    private void GenerateDLCQuest()
    {
        GameObject QuestButtonObj = Instantiate(QuestButton);
        QuestButtonObj.transform.SetParent(ContentPane.transform);
        Text newQuestText = QuestButtonObj.transform.GetComponentInChildren<Text>();
        newQuestText.text = "Most awesome quest ever!";

        Button b = newQuestText.transform.parent.GetComponent<Button>();
        b.onClick.AddListener(() => SetDLCPopUp(true));
    }

    public void SelectQuest()
    {
        if (currSelectedQuestIndex == -1)
            return;

        int index = currSelectedQuestIndex;
        var selectedQ = AvailableQuests[index];
        if (selectedQ != null)
        {
            Debug.Log("Found quest with title '" + selectedQ.Description.Title + "'");
            //CompleteQuest(selectedQ);
            LoadQuest(selectedQ);
        }
        else
            Debug.LogWarning("Didn't find selected quest!");
    }

    // TODO: Semi-Dummy, completes a quest. Should be refactored to enter "Quest Mode".
    public void SelectQuest(int index)
    {
        Debug.Log(index);
        var selectedQ = AvailableQuests[index];
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

        StaticData.currQuest = (MultiQuest)quest;
        SceneManager.LoadScene(UnityEngine.Random.Range(4, 10));
        //SceneManager.LoadScene(7);

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
    }

    void checkForWin()
    {
        if(StaticData.Reputation <= 0)
        {
            StaticData.Reputation = StaticData.MaxReputation;
            StartCoroutine(StaticData.PlayStreamingVideo("ending good.mp4"));
            WinScreen.SetActive(true);
        }
    }

    #region Static methods
    private static void SaveHubData(HubData hubData) { SaveJson(JsonUtility.ToJson(hubData)); }

    private static HubData LoadHubData()
    {
        HubData hubData = LoadJson();

        if (hubData == null)
            hubData = new HubData() { RandomSeed = UnityEngine.Random.Range(0, int.MaxValue) };

        return hubData;
    }

    private static HubData LoadJson()
    {
        if (!File.Exists(hubDataPath))
            return null;

        HubData retData;
        using (StreamReader reader = new StreamReader(hubDataPath)) {
            var jsonObject = reader.ReadToEnd();
            retData = JsonUtility.FromJson<HubData>(jsonObject);
            reader.Close();
        }
        return retData;
    }

    private static void SaveJson(string jsonObject)
    {
        using (StreamWriter writer = new StreamWriter(hubDataPath)) {
            writer.Write(jsonObject);
            writer.Flush();
            writer.Close();
        }
    }
    #endregion

    #region UI

    void UpdateUIText()
    {
        DaysLeftText.text = StaticData.daysLeft.ToString();
    }

    void UpdateUI()
    {
        RingImg.fillAmount = (float)StaticData.daysLeft / (float)StaticData.maxDaysLeft;
    }

    public void SetDLCPopUp(bool b)
    {
        DLCPane.SetActive(b);
    }

    public void SetQuestLetter(int i)
    {
        BaseQuest quest = (BaseQuest)AvailableQuests[currSelectedQuestIndex];
        QuestLetter.GetComponent<TextGeneration>().SetQuestText(quest.Description.Description, quest.Description.Title, quest.Description.Difficulty.ToString());
        QuestLetter.SetActive(Convert.ToBoolean(i));
        GameObject.FindGameObjectWithTag("HandCanvas").GetComponent<Animator>().SetTrigger("handhub");

    }

    public void setCurrSelectedQuest(int i)
    {
        currSelectedQuestIndex = i;
    }

    public BaseQuest GetQuest(int i)
    {
        return (BaseQuest)AvailableQuests[i];
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    #endregion
}
