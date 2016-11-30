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

    public Sprite SheepSprite;
    public Sprite meleeSprite;
    public Sprite rangedSprite;
    public Sprite suicideSprite;

    public GameObject FadeOut;


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
        if (data.Type == QuestType.Destroy)
        {

            //questDesc.Append(string.Format("Destroy quest!" + Environment.NewLine));

            //questDesc.Append(string.Format(" - 0/{0} enemies slain." + Environment.NewLine, data.EnemyCount, PresentMonsters(data.PresentEnemies)));
        }
        else if (data.Type == QuestType.Protect)
        {
            //questDesc.Append(string.Format("Protect the objective!" + Environment.NewLine));

            //questDesc.Append(string.Format(" - 0/{0} enemies slain." + Environment.NewLine, data.EnemyCount, PresentMonsters(data.PresentEnemies)));
            //questDesc.Append(string.Format(" - 0/{0} sheep to protect." + Environment.NewLine, data.FriendlyCount));
        }


        questDesc.Append(string.Format("YOU FAILED..." + Environment.NewLine));
        questDesc.Append(string.Format("Reputation gained: " + StaticIngameData.dummyManager.GetLocalScore()));



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
                case "MeleeAI":
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

        int poz = 0;
        int pozIncrease = 150;
        if (totalSheep > 0)
        {
            GameObject hh = Instantiate(killLine, killLine.transform.parent, killLine.transform) as GameObject;
            hh.transform.Translate(new Vector3(0, -poz, 0));
            poz += pozIncrease;
            hh.GetComponentInChildren<Text>().text = "X " + deadSheep;
            hh.GetComponentInChildren<Image>().sprite = SheepSprite;
            hh.SetActive(true);
        }
        if (totalFishmen > 0)
        {
            GameObject hh = Instantiate(killLine, killLine.transform.parent, killLine.transform) as GameObject;
            hh.transform.Translate(new Vector3(0, -poz, 0));
            poz += pozIncrease;
            hh.GetComponentInChildren<Text>().text = "X " + deadFishmen;
            hh.GetComponentInChildren<Image>().sprite = meleeSprite;
            hh.SetActive(true);
        }

        if (totalGnomes > 0)
        {
            GameObject hh = Instantiate(killLine, killLine.transform.parent, killLine.transform) as GameObject;
            hh.transform.Translate(new Vector3(0, -poz, 0));
            poz += pozIncrease;
            hh.GetComponentInChildren<Text>().text = "X " + deadGnomes;
            //hh.GetComponentInChildren<Image>().sprite =;
            hh.SetActive(true);
        }

        if (totalTrolls > 0)
        {
            GameObject hh = Instantiate(killLine, killLine.transform.parent, killLine.transform) as GameObject;
            hh.transform.Translate(new Vector3(0, -poz, 0));
            poz += pozIncrease;
            hh.GetComponentInChildren<Text>().text = "X " + deadTrolls;
            hh.GetComponentInChildren<Image>().sprite = rangedSprite;
            hh.SetActive(true);
        }

        
	}

    public void GoToHubWorld()
    {
        StaticIngameData.mapManager.LoadHubArea();
    }


}
