using UnityEngine;
using System.Collections;
using System.Text;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class UIQuestObjective : MonoBehaviour {
    private QuestData data;

    // Use this for initialization
    void Start () {

        if (StaticData.currQuest == null) return;
        data = StaticData.currQuest.Data;
        
        var questDesc = new StringBuilder();
        if (data.Type == QuestType.Destroy)
        {

            questDesc.Append(string.Format("Destroy quest!" + Environment.NewLine));

            questDesc.Append(string.Format(" - 0/{0} enemies slain." + Environment.NewLine, data.EnemyCount, PresentMonsters(data.PresentEnemies)));
        }
        else if (data.Type == QuestType.Protect)
        {
            questDesc.Append(string.Format("Protect the objective!" + Environment.NewLine));

            questDesc.Append(string.Format(" - 0/{0} enemies slain." + Environment.NewLine, data.EnemyCount, PresentMonsters(data.PresentEnemies)));
            questDesc.Append(string.Format(" - 0/{0} sheep to protect." + Environment.NewLine, data.FriendlyCount));
           
        }
        

        GetComponent<Text>().text = questDesc.ToString();
    }
	
	// Update is called once per frame
	void Update () {

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
