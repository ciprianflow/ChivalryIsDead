using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Text;
using System.Collections.Generic;

public class EndScreen : MonoBehaviour {

    public GameObject killLine;
    public Text title;
    public Text info;
    public Text reptutation;

    public Sprite SheepSprite;
    public Sprite meleeSprite;
    public Sprite rangedSprite;
    public Sprite suicideSprite;
    public Sprite timeSprite;

    public GameObject Grid;
    public GameObject FadeOut;
    public GameObject Timer;



    private Text deadSheepText;
    private Text deadFishmenText;
    private Text deadGnomesText;
    private Text deadTrollsText;
    private bool startMonsters = false;

    private int scoreWithoutBonus = 0;
    private int localScore = 0;
    private float timeNow;
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

        scoreWithoutBonus = StaticIngameData.dummyManager.GetLocalScoreWithoutBonus();
        localScore = StaticIngameData.dummyManager.GetLocalScore();

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



        info.text = questDesc.ToString();
        showMonsters();

    }

    private int currentSheep, currentRanged, currentMelee, currentSuicide = 0;
    private int deadFishmen, deadTrolls, deadGnomes, deadSheep = 0;
    private int totalFishmen, totalTrolls, totalGnomes, totalSheep = 0;

    private int score = 50;
    private float textBaseScoreIncreaseAnimationSpeed = 5.0f;
    private int scoreAddRound = 0;
    private float scoreMultiplier, scoreMultiplier2, scoreMultiplier3 = 0;
    private bool startTimer = false;


    void Update()
    {        
        if (startMonsters)
        {
            //Debug.Log("realtiem: " + Time.realtimeSinceStartup + " - " + timeNow);
            scoreMultiplier += (Time.realtimeSinceStartup - timeNow) * 0.001f;

            float xsc = Mathf.Round(Mathf.Lerp(0, score, scoreMultiplier));
            //Debug.Log("deaded sheep: " + deadSheep);
            if (totalSheep > 0)
            {
                if (deadSheep >= xsc)
                {
                    deadSheepText.text = "X " + xsc;
                }

            }

            if (totalFishmen > 0)
            {
                if (deadFishmen >= xsc)
                {
                    deadFishmenText.text = "X " + xsc;
                }

            }
            if (totalGnomes > 0)
            {
                if (deadGnomes >= xsc)
                {
                    deadGnomesText.text = "X " + xsc;
                }

            }
            if (totalTrolls > 0)
            {
                if (deadTrolls >= xsc)
                {
                    deadTrollsText.text = "X " + xsc;
                }
            }

            scoreMultiplier2 += (Time.realtimeSinceStartup - timeNow) * 0.005f;
            float tsc = Mathf.Round(Mathf.Lerp(0, scoreWithoutBonus, scoreMultiplier2));
            reptutation.text = "Reputation: " + tsc;

            if(tsc >= scoreWithoutBonus || xsc >= 20f)
            {

                startMonsters = false;
                StartCoroutine(startTimerBonus());

            }
        }

        if (startTimer)
        {
            scoreMultiplier3 += (Time.realtimeSinceStartup - timeNow) * 0.004f;
            float bsc = Mathf.Round(Mathf.Lerp(scoreWithoutBonus, localScore, scoreMultiplier3));

            reptutation.text = "Reputation: " + bsc;
        }

    }

    private IEnumerator startTimerBonus()
    {
        yield return new WaitForSeconds(0.3f);
        Time.timeScale = 0f;
        startTimer = true;
        Timer.SetActive(true);

        if (TimerObjectScript.Instance != null)
        {
            float timer = TimerObjectScript.Instance.GetTimer();

            Timer.GetComponentInChildren<Text>().text = secondsToMinutes((int)Math.Round(timer));            
        }
        timeNow = Time.realtimeSinceStartup;
    }

	// Update is called once per frame
	void showMonsters () {

        List<MonsterAI> list = StaticIngameData.mapManager.GetObjectiveManager().GetMonsters();
        
        
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



        if (totalSheep > 0)
        {
            GameObject hh = Instantiate(killLine, killLine.transform.parent, killLine.transform) as GameObject;
            hh.SetActive(true);
            deadSheepText = hh.GetComponentInChildren<Text>();
            deadSheepText.text = "X 0";
            //hh.GetComponentInChildren<Text>().text = "X " + deadSheep;
            hh.GetComponentInChildren<Image>().sprite = SheepSprite;
            
        }
        if (totalFishmen > 0)
        {
            GameObject hh = Instantiate(killLine, killLine.transform.parent, killLine.transform) as GameObject;
            hh.SetActive(true);
            deadFishmenText = hh.GetComponentInChildren<Text>();
            deadFishmenText.text = "X 0";
            //hh.GetComponentInChildren<Text>().text = "X " + deadFishmen;
            hh.GetComponentInChildren<Image>().sprite = meleeSprite;

        }
        if (totalGnomes > 0)
        {
            GameObject hh = Instantiate(killLine, killLine.transform.parent, killLine.transform) as GameObject;
            hh.SetActive(true);
            deadGnomesText = hh.GetComponentInChildren<Text>();
            deadGnomesText.text = "X 0";
            //hh.GetComponentInChildren<Text>().text = "X " + deadGnomes;
            hh.GetComponentInChildren<Image>().sprite = suicideSprite;

        }
        if (totalTrolls > 0)
        {
            GameObject hh = Instantiate(killLine, killLine.transform.parent, killLine.transform) as GameObject;
            hh.SetActive(true);
            deadTrollsText = hh.GetComponentInChildren<Text>();
            deadTrollsText.text = "X 0";
            //hh.GetComponentInChildren<Text>().text = "X " + deadTrolls;
            hh.GetComponentInChildren<Image>().sprite = rangedSprite;

        }



        timeNow = Time.realtimeSinceStartup;
        startMonsters = true;



        Debug.Log("startshowmosnters");

    }


    public void GoToHubWorld()
    {
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
