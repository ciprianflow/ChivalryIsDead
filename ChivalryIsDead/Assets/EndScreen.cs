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
    public Text reptutation;

    public Sprite SheepSprite;
    public Sprite meleeSprite;
    public Sprite rangedSprite;
    public Sprite suicideSprite;
    public Sprite timeSprite;

    public GameObject Grid;
    public GameObject FadeOut;
    public GameObject Timer;
    public GameObject Reputation;



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
        if (FadeOut != null)
            FadeOut.SetActive(true);
        
        if (StaticData.currQuest == null) return;
        data = StaticData.currQuest.Data;

        var questDesc = new StringBuilder();

        scoreWithoutBonus = StaticIngameData.dummyManager.GetLocalScoreWithoutBonus();
        localScore = StaticIngameData.dummyManager.GetLocalScore();



        if (TimerObjectScript.Instance.GetElapsedTime() <= 0.01)
        {
            if (PlayerPrefs.GetString("Language") == "English")
            {
                title.text = "CONDOLENCES!";
                questDesc.Append(string.Format("YOU WERE TOO SLOW!" + Environment.NewLine));
            }
            else
            {
                title.text = "DET ER JEG KED AF!";
                questDesc.Append(string.Format("DU VAR FOR LANGSOM!" + Environment.NewLine));
            }
        }
        else if (localScore >= 0)
        {
            if (PlayerPrefs.GetString("Language") == "English")
            {
                title.text = "CONDOLENCES!";
                questDesc.Append(string.Format("YOU DIDN'T FAIL!" + Environment.NewLine));
            }
            else
            {
                title.text = "DET ER JEG KED AF!";
                questDesc.Append(string.Format("DU FEJLEDE IKKE!" + Environment.NewLine));
            }
        }
        else
        {
            if (PlayerPrefs.GetString("Language") == "English")
            {
                title.text = "CONGRATULATIONS!";
                questDesc.Append(string.Format("YOU FAILED!" + Environment.NewLine));
            }
            else
            {
                title.text = "TILLYKKE!";
                questDesc.Append(string.Format("DU FEJLEDE!" + Environment.NewLine));
            }
            
        }
        
        info.text = questDesc.ToString();
        showMonsters();
    }
    // Use this for initialization
    void Start () {


        

    }

    private int currentSheep, currentRanged, currentMelee, currentSuicide = 0;
    private int deadFishmen, deadTrolls, deadGnomes, deadSheep = 0;
    private int totalFishmen, totalTrolls, totalGnomes, totalSheep = 0;

    private int score = 50;
    private float textBaseScoreIncreaseAnimationSpeed = 5.0f;
    private int scoreAddRound = 0;
    private float scoreMultiplier, scoreMultiplier2, scoreMultiplier3 = 0;
    private bool startTimer = false;
    private bool showReputation = false;

    void Update()
    {
        if (startMonsters)
        {

            scoreMultiplier += (Time.realtimeSinceStartup - timeNow) * 0.05f;

            float xsc = Mathf.Round(Mathf.Lerp(0, score, scoreMultiplier));
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

            //end loading monsters
            if (xsc >= 18)
            {
                startMonsters = false;
                Reputation.SetActive(true);
                showReputation = true;

                WwiseInterface.Instance.PlayRewardSound(RewardHandle.PointCounter);
                
                //reptutation.setA
            }
            
        }

        if(showReputation)
        {
            scoreMultiplier2 += (Time.realtimeSinceStartup - timeNow) * 0.2f;
            float tsc = Mathf.Round(Mathf.Lerp(0, scoreWithoutBonus, scoreMultiplier2));

            if (Mathf.Abs(scoreWithoutBonus) - Mathf.Abs(tsc) <= 0.1)
            {

                showReputation = false;
                StartCoroutine(startTimerBonus());

            }
            else
            {
                if (PlayerPrefs.GetString("Language") == "English")
                {
                    reptutation.text = "Reputation: " + tsc;
                }
                else
                {
                    reptutation.text = "Omdømme: " + tsc;
                }
            }


        }

        if (startTimer)
        {
            scoreMultiplier3 += (Time.realtimeSinceStartup - timeNow) * 0.4f;
            float bsc = Mathf.Round(Mathf.Lerp(scoreWithoutBonus, localScore, scoreMultiplier3));

            if (PlayerPrefs.GetString("Language") == "English")
            {
                reptutation.text = "Reputation: " + bsc;
            }
            else
            {
                reptutation.text = "Omdømme: " + bsc;
            }
        }

        timeNow = Time.realtimeSinceStartup;
    }

    private IEnumerator startTimerBonus()
    {
        yield return new WaitForSeconds(0.15f);
        Time.timeScale = 0f;
        startTimer = true;
        Timer.SetActive(true);

        if (TimerObjectScript.Instance != null)
        {
            float timer = TimerObjectScript.Instance.GetTimer();

            Timer.GetComponentInChildren<Text>().text = secondsToMinutes((int)Math.Round(timer));            
        }
        timeNow = Time.realtimeSinceStartup;
        WwiseInterface.Instance.PlayRewardSound(RewardHandle.PointCounter);
    }

	// Update is called once per frame
	void showMonsters () {

        List<MonsterAI> list = StaticIngameData.mapManager.GetObjectiveManager().GetMonsters();
        
        
        //better ways
        foreach (MonsterAI monster in list)
        {
            monster.GetComponent<ExclamationMark>().DisableMark();

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

        WwiseInterface.Instance.PlayRewardSound(RewardHandle.PointCounter);


        Debug.Log("startshowmosnters");

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
