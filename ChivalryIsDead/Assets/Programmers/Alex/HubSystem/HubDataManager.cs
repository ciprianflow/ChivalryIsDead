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
    public GameObject QuestButton;
    public GameObject QuestLetter;
    public GameObject DampenLightObject;
    public GameObject WinScreen;
    public GameObject LoseScreen;
    public Text DaysLeftText;
    public Image RingImg;
    public bool isClicked;

    void Awake()
    {
        Time.timeScale = 1f;

        hubDataPath = Application.persistentDataPath + "/HubData.json";

        if (StaticData.currQuest == null)
            UpdateQuests();    
        else
            PushToHubData(StaticData.currQuest.ReputationChange);
    }

    void Start () {
        isClicked = false;
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
        hubData.GlobalReputation += Mathf.Clamp(repChange, -10, 10);
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
        SaveHubData(hubData, Application.persistentDataPath + "/HubData.json");
    }

    #region Quest Generation
    // TODO: Dummy method, shouldn't make it into the final game. Update to generic or UI specific alternative.
    private void CreateQuestUIElements()
    {
        for(int i = 0; i < 1; i++) { 
        //foreach (IObjective o in AvailableQuests) {
            BaseQuest oAsQuest = (BaseQuest)AvailableQuests[i];
            peasantLineScript.PushQuestToPeasant(i, i, oAsQuest);
        }

        //GenerateDLCQuest();
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
        var allObjectives = StaticData.currQuest.GetAllObjectives().ToList();
        var hasHouse = allObjectives.Any(o => (o as BaseObjective).targetID == 22);
        var isBakeryOrFarmHouse = StaticData.currQuest.Data.PresentFriends & (FriendlyTypes.Bakery | FriendlyTypes.Farmhouse);

        List<int> houseIdxs;
        if (HasFlag(isBakeryOrFarmHouse, (int)FriendlyTypes.Bakery) && HasFlag(isBakeryOrFarmHouse, (int)FriendlyTypes.Farmhouse)) {
            // Reduce to one or the other
            if (Convert.ToBoolean(UnityEngine.Random.Range(0, 2))) {
                houseIdxs = new List<int>() { 6 };
            } else {
                houseIdxs = new List<int>() { 4 };
            }
        } else if (HasFlag(isBakeryOrFarmHouse, (int)FriendlyTypes.Bakery) || HasFlag(isBakeryOrFarmHouse, (int) FriendlyTypes.Farmhouse)) {
            if (HasFlag(isBakeryOrFarmHouse, (int)FriendlyTypes.Bakery))
                houseIdxs = new List<int>() { 4 };
            else
                houseIdxs = new List<int>() { 6 };
        } else {
            houseIdxs = new List<int>() { 1, 2, 3, 5 };
        } 

        var mapIdx = UnityEngine.Random.Range(0, houseIdxs.Count);
        var mapNum = houseIdxs[mapIdx];
        
        int mapIndex = UnityEngine.Random.Range(1, 7);
        SceneManager.LoadScene("0" + mapIndex.ToString() + "UR");
        //SceneManager.LoadScene(7);

    }

    void checkForWin()
    {
        if(StaticData.Reputation <= 0)
        {
            StaticData.Reputation = StaticData.MaxReputation;
            StartCoroutine(StaticData.PlayStreamingVideo("ending good.mp4"));
            WinScreen.SetActive(true);

            return;
        }

        /* UNCOMMENT TO HAVE THE LOSE SCREEN */
        if (StaticData.daysLeft < 1)
        {            
            StartCoroutine(StaticData.PlayStreamingVideo("ending bad.mp4"));
            LoseScreen.SetActive(true);
        }
    }

    #region Static methods
    private static void SaveHubData(HubData hubData, string path) { SaveJson(JsonUtility.ToJson(hubData), path); }
    private static void SaveHubData(HubData hubData) { SaveJson(JsonUtility.ToJson(hubData)); }

    private static HubData LoadHubData()
    {
        HubData hubData = LoadJson();

        if (hubData == null)
            hubData = new HubData() { RandomSeed = UnityEngine.Random.Range(0, int.MaxValue) };

        return hubData;
    }

    private static void SaveJson(string jsonObject) { SaveJson(jsonObject, hubDataPath); }
    private static void SaveJson(string jsonObject, string path) {
        using (StreamWriter writer = new StreamWriter(path)) {
            writer.Write(jsonObject);
            writer.Flush();
            writer.Close();
        }
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
    #endregion

    #region UI

    void UpdateUIText()
    {
        DaysLeftText.text = StaticData.daysLeft.ToString();
    }

    void UpdateUI()
    {
        RingImg.fillAmount = 1 - (float)StaticData.daysLeft / (float)StaticData.maxDaysLeft;
    }

    public void SetDLCPopUp(bool b)
    {
        DLCPane.SetActive(b);
    }

    public void SetQuestLetter(int i)
    {
        BaseQuest quest = (BaseQuest)AvailableQuests[currSelectedQuestIndex];
        QuestLetter.GetComponent<TextGeneration>().SetQuestText(quest.Description, quest.Data);

        bool boolean = Convert.ToBoolean(i);
        //QuestLetter.GetComponent<TextGeneration>().SetQuestText(quest.Description.Description, quest.Description.Title, quest.Description.Difficulty.ToString());
        QuestLetter.SetActive(boolean);
        DampenLightObject.SetActive(boolean);
        //GameObject.FindGameObjectWithTag("HandCanvas").GetComponent<Animator>().SetTrigger("handhub");
        isClicked = boolean;
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

    private bool HasFlag(FriendlyTypes e, int value)
    {
        return (e & (FriendlyTypes)value) == e;
    }
}
