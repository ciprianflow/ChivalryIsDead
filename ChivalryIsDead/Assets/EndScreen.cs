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

    private int deadFishmen = 0, deadTrolls = 0, deadGnomes = 0, deadSheep = 0;
    private int totalFishmen = 0, totalTrolls = 0, totalGnomes = 0, totalSheep = 0;

    private QuestData data;

    void Awake()
    {

    }
    // Use this for initialization
    void Start () {
        

        if (StaticData.currQuest == null) return;
        data = StaticData.currQuest.Data;

        var questDesc = new StringBuilder();
        if (data.Type == QuestType.Destroy)
        {

            questDesc.Append(string.Format("Destroy quest!" + Environment.NewLine));

            //questDesc.Append(string.Format(" - 0/{0} enemies slain." + Environment.NewLine, data.EnemyCount, PresentMonsters(data.PresentEnemies)));
        }
        else if (data.Type == QuestType.Protect)
        {
            questDesc.Append(string.Format("Protect the objective!" + Environment.NewLine));

            //questDesc.Append(string.Format(" - 0/{0} enemies slain." + Environment.NewLine, data.EnemyCount, PresentMonsters(data.PresentEnemies)));
            //questDesc.Append(string.Format(" - 0/{0} sheep to protect." + Environment.NewLine, data.FriendlyCount));

        }

        questDesc.Append(string.Format("Reputation gained: " + StaticIngameData.dummyManager.ReputationHandler.Score));



        info.text = questDesc.ToString();
        showMonsters();
    }
	
	// Update is called once per frame
	void showMonsters () {


        //Debug.Log("IMP!---------------");
        List<MonsterAI> list = StaticIngameData.mapManager.OM.GetMonsters();
            
        Debug.Log(list.Count);
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
        if (totalSheep > 0)
        {
            GameObject hh = Instantiate(killLine, killLine.transform.parent, killLine.transform) as GameObject;
            hh.transform.Translate(new Vector3(0, -poz, 0));
            poz += 50;
            hh.GetComponentInChildren<Text>().text = deadSheep + "/" + totalSheep + " dead sheep";
            hh.SetActive(true);
        }
        if (totalFishmen > 0)
        {
            GameObject hh = Instantiate(killLine, killLine.transform.parent, killLine.transform) as GameObject;
            hh.transform.Translate(new Vector3(0, -poz, 0));
            poz += 50;
            hh.GetComponentInChildren<Text>().text = deadFishmen + "/" + totalFishmen + " dead fishmen";
            hh.SetActive(true);
        }

        if (totalGnomes > 0)
        {
            GameObject hh = Instantiate(killLine, killLine.transform.parent, killLine.transform) as GameObject;
            hh.transform.Translate(new Vector3(0, -poz, 0));
            poz += 50;
            hh.GetComponentInChildren<Text>().text = deadGnomes + "/" + totalGnomes + " dead gnomes";
            hh.SetActive(true);
        }

        if (totalTrolls > 0)
        {
            GameObject hh = Instantiate(killLine, killLine.transform.parent, killLine.transform) as GameObject;
            hh.transform.Translate(new Vector3(0, -poz, 0));
            poz += 50;
            hh.GetComponentInChildren<Text>().text = deadTrolls + "/" + totalTrolls + " dead trolls";
            hh.SetActive(true);
        }

        //hh.GetComponentInChildren<Image>().sprite;


        Debug.Log(totalTrolls + " - " + deadTrolls);
        
	}

    public void GoToHubWorld()
    {
        StaticIngameData.mapManager.LoadHubArea();
    }


    private string PresentMonsters(EnemyTypes enemyTypes)
    {
        var enemyList = new List<string>();
        var hasMelee = (enemyTypes & EnemyTypes.HasMelee) == EnemyTypes.HasMelee;
        var hasRanged = (enemyTypes & EnemyTypes.HasRanged) == EnemyTypes.HasRanged;
        var hasSuicide = (enemyTypes & EnemyTypes.HasSuicide) == EnemyTypes.HasSuicide;

        if (hasMelee) enemyList.Add("Fishmen");
        if (hasRanged) enemyList.Add("Trolls");
        if (hasSuicide) enemyList.Add("Gnomes");

        return string.Join(", ", enemyList.ToArray());
    }
}
