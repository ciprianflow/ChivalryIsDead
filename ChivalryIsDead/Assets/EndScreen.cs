using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Text;
using System.Collections.Generic;

public class EndScreen : MonoBehaviour {

    public GameObject loadingScreen;

    public GameObject killLine;
    public Text title;
    public Text info;

    public Sprite SheepSprite;
    public Sprite meleeSprite;
    public Sprite rangedSprite;
    public Sprite suicideSprite;
    public Sprite timeSprite;

    public GameObject Grid;
    public GameObject FadeOut;
    public GameObject Timer;


    private QuestData data;

    void Awake()
    {

    }
    // Use this for initialization
    void Start () {

        if (FadeOut != null)
            FadeOut.SetActive(true);

        if (StaticData.currQuest == null) return;
        data = StaticData.currQuest.Data;

        var questDesc = new StringBuilder();

        int localScore = StaticIngameData.dummyManager.GetLocalScore();

        if (TimerObjectScript.Instance.GetElapsedTime() <= 0.01)
        {
            title.text = "CONDOLENCES!";
            questDesc.Append(string.Format("YOU WERE TOO SLOW!" + Environment.NewLine));

        }
        else if (localScore >= 0)
        {
            title.text = "CONDOLENCES!";
            questDesc.Append(string.Format("YOU DIDN'T FAIL!" + Environment.NewLine));

        }
        else
        {
            title.text = "CONGRATULATIONS!";
            questDesc.Append(string.Format("YOU FAILED!" + Environment.NewLine));

        }

        //            questDesc.Append(string.Format("Reputation gained: " + localScore));



        info.text = questDesc.ToString();
        showMonsters();

    }
	
    void Update()
    {
        
    }

	// Update is called once per frame
	void showMonsters () {

        List<MonsterAI> list = StaticIngameData.mapManager.GetObjectiveManager().GetMonsters();


        int deadFishmen = 0, deadTrolls = 0, deadGnomes = 0, deadSheep = 0;
        int totalFishmen = 0, totalTrolls = 0, totalGnomes = 0, totalSheep = 0;
        //better ways
        foreach (MonsterAI monster in list)
        {
            
            switch (monster.GetType().ToString())
            {
                
                case "RangedAI":
                    totalTrolls++;
                    if (monster.getState() == State.Death)
                    {
                        deadTrolls++;
                    }
                    break;
                case "SuicideAI":
                    totalGnomes++;
                    if (monster.getState() == State.Death)
                    {
                        deadGnomes++;
                    }
                    break;
                case "MeleeAI2":
                    totalFishmen++;
                    if (monster.getState() == State.Death)
                    {
                        deadFishmen++;
                    }
                    break;
                case "SheepAI":
                    totalSheep++;
                    if (monster.getState() == State.Death)
                    {
                        deadSheep++;
                    }
                    break;
            }
        }


        if (TimerObjectScript.Instance != null)
        {
            float timer = TimerObjectScript.Instance.GetTimer();

            Timer.GetComponentInChildren<Text>().text = secondsToMinutes((int) Math.Round(timer));
        }

        if (totalSheep > 0)
        {
            GameObject hh = Instantiate(killLine, killLine.transform.parent, killLine.transform) as GameObject;
            hh.GetComponentInChildren<Text>().text = "X " + deadSheep;
            hh.GetComponentInChildren<Image>().sprite = SheepSprite;
            hh.SetActive(true);
        }
        if (totalFishmen > 0)
        {
            GameObject hh = Instantiate(killLine, killLine.transform.parent, killLine.transform) as GameObject;

            hh.GetComponentInChildren<Text>().text = "X " + deadFishmen;
            hh.GetComponentInChildren<Image>().sprite = meleeSprite;
            hh.SetActive(true);
        }

        if (totalGnomes > 0)
        {
            GameObject hh = Instantiate(killLine, killLine.transform.parent, killLine.transform) as GameObject;
            hh.GetComponentInChildren<Text>().text = "X " + deadGnomes;
            //hh.GetComponentInChildren<Image>().sprite =;
            hh.SetActive(true);
        }

        if (totalTrolls > 0)
        {
            GameObject hh = Instantiate(killLine, killLine.transform.parent, killLine.transform) as GameObject;
            hh.GetComponentInChildren<Text>().text = "X " + deadTrolls;
            hh.GetComponentInChildren<Image>().sprite = rangedSprite;
            hh.SetActive(true);
        }



    }

    public void GoToHubWorld()
    {
        loadingScreen.SetActive(true);
        StaticIngameData.mapManager.LoadHubArea();
    }

    private string secondsToMinutes(int seconds)
    {
        string format;
        int min = Math.Abs(seconds / 60);
        format = min.ToString();
        if ( min < 10)
        {
            format = "0" + format;
        }
        int sec = Math.Abs(60 * min - seconds);
        
        if (sec < 10)
        {
            format = format + ":0" + sec;
        }
        else
        {
            format = format + ":" + sec;
        }

        return format;

    }

}
