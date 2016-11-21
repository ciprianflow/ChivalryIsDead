using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using UnityEditor;
using System.Collections.Generic;
using System;

public class HubDataManager : MonoBehaviour {

    private const string hubDataPath = "Assets/HubData.asset";
    private HubData currentHubData;

    int currSelectedQuestIndex = -1;

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

    public PeasantLineScript peasantLineScript;

    public int MaximumReputation = 2000;
    public int TotalDays = 14;

    public GameObject DLCPane;
    public GameObject ContentPane;
    public GameObject QuestButton;
    public GameObject QuestLetter;
    public GameObject WinScreen;
    public Text DaysLeftText;
    public Image RingImg;

    void Start () {

        checkForWin();
        peasantLineScript.FillPeasantLine();
        UpdateQuests();
        UpdateUIText();
        UpdateUI();
        CreateQuestUIElements();

    }
	
    // Should be used when game is restarted or booted.
    public void UpdateQuests()
    {
        var hubData = LoadHubData();
        hubData.AvailableQuests = new List<IQuest>();

        // TODO : New quest spawning system??
        QuestGenerator QG = new QuestGenerator(StaticData.daysLeft, (int)StaticData.Reputation);
        QuestData QD = new QuestData();
        MultiQuest MQ = QG.GenerateMultiQuest(out QD);
        //MQ.Description = new QuestDescription("ok", "this is dog", Difficulty.Easy);
        hubData.AvailableQuests.Add(MQ);

        //QG = new QuestGenerator(StaticData.daysLeft, (int)StaticData.Reputation);
        //QD = new QuestData();
        //MQ = QG.GenerateMultiQuest(out QD);
        //MQ.Description = new QuestDescription("ok", "this is dog", Difficulty.Easy);
        //hubData.AvailableQuests.Add(MQ);

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

            //Text newQuestText = QuestButtonObj.transform.GetComponentInChildren<Text>();
            // TODO : Name doesnt load from quest description
            //newQuestText.text = oAsQuest.Description.Title;
            //newQuestText.text = "Quest";

            //Button b = newQuestText.transform.parent.GetComponent<Button>();
            int newI = i;
            //b.onClick.AddListener(() => SelectQuest(newI));

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

        //

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
            WinScreen.SetActive(true);
        }
    }

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
