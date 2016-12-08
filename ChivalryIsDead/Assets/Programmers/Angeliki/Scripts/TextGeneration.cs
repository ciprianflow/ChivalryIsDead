using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class TextGeneration : MonoBehaviour {

    public StringBuilder sbTitle = new StringBuilder();
    public StringBuilder sbText = new StringBuilder();
    public StringBuilder sbEndTitle = new StringBuilder();

    public int textSize;
    public int numFiles;
    public string killString;

    [HideInInspector]
    public bool[] NewBagInitializer;
   
    int amount;

    [HideInInspector]
    public List<TextAsset> sentences = new List<TextAsset>();
    public DialogInfo[] dialogEnglish;
    //public GameObject TooltipText;
   
    private List<String> scrObjects = new List<String>();


    [HideInInspector]
    public List<ShuffleBag> shuffleBags = new List<ShuffleBag>();
    //public List<string> filesNames = new List<string>();

    Text QuestTitleText;
    Text title;
    Text mainText;
    Text Description;
    Text OurDescription;
    Text endTitle;
    Text pQuestTitle;
    Text ourQuestTitle;

    StringBuilder swordQuestDesc = new StringBuilder();

    List<int> logSequenceStart = new List<int>();
    List<int> logSequenceEnd = new List<int>();

    GameObject HubDataMan;
    //int QuestNum;

    private string QuestTitle = "";
    private string QuestDifficulty = "";
    private string QuestText = "";
    private string SwordQuestText = "";
    QuestType questType;

    public void SetQuestText(QuestDescription desc, QuestData data)
    {
        QuestText = CreateQuestText(data);
        SwordQuestText = swordQuestDesc.ToString();
        QuestTitle = desc.Title;
        QuestDifficulty = desc.Difficulty.ToString();
    }

    public string CreateQuestText(QuestData data)
    {
        var questDesc = new StringBuilder();
        questType = data.Type;
        if (PlayerPrefs.GetString("Language") == "English")
        {
            if (data.Type == QuestType.Destroy)
            { //Dræb de {0} fjender. (Der er 2 __)
                //questDesc.Append(string.Format("• Destroy the {0} enemies. ({1})" + Environment.NewLine,
                //    data.EnemyCount,
                //    string.Join(", ", data.GetEnemies().ToArray()))
                //);                                     //Brug de {0} fjender til at miste omdømme
                //swordQuestDesc.Append(string.Format("• Use the {0} enemies ({1}) to lose reputation" + Environment.NewLine,
                //    data.EnemyCount,
                //    string.Join(", ", data.GetEnemies().ToArray()))
                //);
                questDesc.Append(string.Format("Peacefully resolve the monster conflict in the area."));
                swordQuestDesc.Append(string.Format("Make the monsters kill each other in the area!"));
            }
            else if (data.Type == QuestType.Protect)
            {
                questDesc.Append(string.Format("Kill the invading monsters and protect the building in the area."));
                swordQuestDesc.Append(string.Format("Don’t kill the invading monsters and destroy the building in the area!"));
                //questDesc.Append(   //Dræb de {0} fjender. (Der er {1})
                //    string.Format("• Destroy the {0} enemies ({1})." + Environment.NewLine,
                //        data.EnemyCount,
                //        string.Join(", ", data.GetEnemies().ToArray()))
                //);
                //swordQuestDesc.Append( //Brug de {0} fjender
                //    string.Format("• Use the {0} enemies ({1})" + Environment.NewLine,
                //        data.EnemyCount,
                //        string.Join(", ", data.GetEnemies().ToArray()))
                //);
                //questDesc.Append(   //Beskyt de {0} venlige. (Der er {1})
                //    string.Format("• Protect the {0} sheep and the objective." + Environment.NewLine,
                //        data.FriendlyCount-1,
                //        string.Join(", ", data.GetFriends().ToArray()))
                //);
                //swordQuestDesc.Append(  //til at ødelægge {0} {1}                   og miste ømdømme
                //    string.Format("to kill the {0} sheep and destroy the objective" + Environment.NewLine + " to lose reputation" + Environment.NewLine,
                //        data.FriendlyCount-1,
                //        string.Join(", ", data.GetFriends().ToArray()))
                //);
            }                //Du har 150 sekunder.
            //questDesc.Append("You have 150 seconds." + Environment.NewLine);
        }
        else
        {
            if (data.Type == QuestType.Destroy)
            { //Dræb de {0} fjender. (Der er 2 __)
                //questDesc.Append(string.Format("• Dræb de {0} fjender." + Environment.NewLine,
                //    data.EnemyCount,
                //    string.Join(", ", data.GetEnemies().ToArray()))
                //);                                     //Brug de {0} fjender til at miste omdømme
                //swordQuestDesc.Append(string.Format("• Brug de {0} fjender til at miste omdømme" + Environment.NewLine,
                //    data.EnemyCount)
                //);
                questDesc.Append(string.Format("Fredfyldt løs konflikterne med monstrene"));
                swordQuestDesc.Append(string.Format("Få monstrene i området til at dræbe hinanden!"));
            }
            else if (data.Type == QuestType.Protect)
            {
                questDesc.Append(string.Format("Dræb de invaderende monstre og beskyt bygningen i området"));
                swordQuestDesc.Append(string.Format("Dræb ikke de ivnaderende monstre og ødelæg bygningen i området!"));
                //questDesc.Append(   //Dræb de {0} fjender. (Der er {1})
                //    string.Format("• Dræb de {0} fjender." + Environment.NewLine,
                //        data.EnemyCount,
                //        string.Join(", ", data.GetEnemies().ToArray()))
                //);
                //swordQuestDesc.Append( //Brug de {0} fjender
                //    string.Format("• Brug de {0} fjender" + Environment.NewLine,
                //        data.EnemyCount)
                //);
                //questDesc.Append(   //Beskyt de {0} venlige. (Der er {1})
                //    string.Format("• Beskyt de {0} venlige." + Environment.NewLine,
                //        data.FriendlyCount,
                //        string.Join(", ", data.GetFriends().ToArray()))
                //);
                //swordQuestDesc.Append(  //til at ødelægge {0} {1}                   og miste ømdømme
                //    string.Format("til at ødelægge {0} venlige" + Environment.NewLine + " og miste ømdømme" + Environment.NewLine,
                //        data.FriendlyCount,
                //        string.Join(", ", data.GetFriends().ToArray()))
                //);
            }                //Du har 150 sekunder.
            //questDesc.Append("Du har 150 sekunder." + Environment.NewLine);
        }
        
        //questDesc.Append(Environment.NewLine + "NOTE FROM SWORD: Remember that you wanna lose the quest, not win it!");
        return questDesc.ToString();
    }

    public void SetQuestText(string description, string title, string difficulty)
    {
        QuestText = description;
        QuestTitle = title;
        QuestDifficulty = difficulty;
        Debug.Log(QuestText + " " +  QuestTitle + " " +  QuestDifficulty);
    }

    // Use this for initialization
    void Start() {
        ClearText();
        //if (PlayerPrefs.GetString("Language") == "English")
        //{
        //    TooltipText.GetComponent<Text>().text = "Days until the wedding";         
        //}
        //else
        //{
        //    TooltipText.GetComponent<Text>().text = "Dage indtil brylluppet";
        //}
        if (gameObject.tag == "IntroLetter")
        {
            HubDataMan = GameObject.FindGameObjectWithTag("HubDataManager");
            //QuestNum = HubDataMan.GetComponent<AngHubDataManager>().tempSelectedQuest;
        }
        //debugText = GameObject.FindWithTag("DebugText").GetComponent<Text>();
        QuestTitleText = transform.FindChild("QuestName").GetComponent<Text>();
        title = transform.FindChild("Title").GetComponent<Text>();
        mainText = transform.FindChild("Info").GetComponent<Text>();
        Description = transform.FindChild("QuestDescription").GetComponent<Text>();
        OurDescription = transform.FindChild("OURQuest").GetComponent<Text>();
        endTitle = transform.FindChild("EndTitle").GetComponent<Text>();
       
        pQuestTitle = transform.FindChild("QuestDescriptionTitle").GetComponent<Text>();
        ourQuestTitle = transform.FindChild("OURQuestTitle").GetComponent<Text>();


        QuestTitleText.text = QuestTitle;
        Description.text = QuestText;
        OurDescription.text = SwordQuestText;
    
        mainText.text = "";

  
        for(int i = 0; i<dialogEnglish.Length;i++)
        {

            scrObjects.Add(dialogEnglish[i].Text[0]);
        }

        foreach (String textFile in scrObjects)
        {
            shuffleBags.Add(new ShuffleBag(textFile.Split('/').Length));
        }

        for (int i = 0; i < shuffleBags.Count; i++)
        {
            shuffleBags[i] = LoadShuffleBag(shuffleBags[i], scrObjects[i], 1);
        }

        if(PlayerPrefs.GetString("Language") == "English")
        {
            if (SceneGetter.Instance.isHubWorld())
            {
                pQuestTitle.text = "Peasant Quest";
                ourQuestTitle.text = "Our Quest";
            }           
            TitleGenerator(shuffleBags[0]);
            TitleGenerator(shuffleBags[1]);
        }
        else
        {
            if (SceneGetter.Instance.isHubWorld())
            {
                pQuestTitle.text = "Bondemission";
                ourQuestTitle.text = "Vores mission";
            }             
            TitleGenerator(shuffleBags[7]);
            TitleGenerator(shuffleBags[8]);
        }
        
        
        //EndTitleGenerator(shuffleBags[3]);

        if (gameObject.tag == "EndLetter")
            initTextBags(NewBagInitializer);
        else
            CallTxtChooserStartQuest();

        // temporary - Remember!!!!
        killString = "12" + " ";
        NumberTextUpdate(killString);

    }

    // Update is called once per frame
    void Update () {
      
    }

    public void ClearText()
    {
        sbTitle = new StringBuilder();
        sbText = new StringBuilder();
        sbEndTitle = new StringBuilder();

        //debugText = transform.GetChild(2).GetComponent<Text>();
        //debugText.text = sb.ToString();
    }

    //ShuffleBag LoadShuffleBag(ShuffleBag shuffleBag, TextAsset sentences, int amount)
    ShuffleBag LoadShuffleBag(ShuffleBag shuffleBag, string sentStr, int amount)
    {
        foreach (string sent in sentStr.Split('/'))
        {
            shuffleBag.Add(sent, amount);

        }
        return shuffleBag;
    }
    void TitleGenerator(ShuffleBag shuffleBag)
    {

        sbTitle.Append(shuffleBag.Next());
        title.text = sbTitle.ToString();
        //return sb;
    }
    //public StringBuilder TextGenerator(ShuffleBag shuffleBag)
    void TextGenerator(ShuffleBag shuffleBag)
    {

        sbText.Append(shuffleBag.Next());
        mainText.text = sbText.ToString();
        //return sb;
    }
    void EndTitleGenerator(ShuffleBag shuffleBag)
    {

        sbEndTitle.Append(shuffleBag.Next());
        endTitle.text = sbEndTitle.ToString();
        //return sb;
    }

    public void initTextBags(bool[] NewBagInitializer)
    {     
        for (int x = 0; x < textSize; x++)
        {
            for(int y = 0; y < numFiles; y++)
            {
                if (NewBagInitializer[x + (y * textSize)])
                {
                    TextGenerator(shuffleBags[y]);   
                }
            }
        }
    }
    
    public void CallTxtChooserStartQuest()
    {
        logSequenceStart = TxtChooserStartQuest(QuestDifficulty);
        foreach (int seq in logSequenceStart)
        {
            TextGenerator(shuffleBags[seq]);

        }
    }

    List<int> TxtChooserStartQuest(String diff)
    {
        List<int> sequence = new List<int>();
        if(PlayerPrefs.GetString("Language") == "English")
        {
            if(questType == QuestType.Protect)
            {
                if (diff == "Easy")
                {
                    sequence.Add(2);
                }
                else if (diff == "Medium")
                {
                    sequence.Add(3);
                }
                else
                {
                    sequence.Add(4);
                }
            }
            else
            {
                if (diff == "Easy")
                {
                    sequence.Add(2);
                }
                else if (diff == "Medium")
                {
                    sequence.Add(5);
                }
                else
                {
                    sequence.Add(6);
                }
            }
            
        }
        else
        {
            if (questType == QuestType.Protect)
            {
                if (diff == "Easy")
                {
                    sequence.Add(9);
                }
                else if (diff == "Medium")
                {
                    sequence.Add(10);
                }
                else
                {
                    sequence.Add(11);
                }
            }
            else
            {
                if (diff == "Easy")
                {
                    sequence.Add(9);
                }
                else if (diff == "Medium")
                {
                    sequence.Add(12);
                }
                else
                {
                    sequence.Add(13);
                }
            }

                
        }
        

        return sequence;
    }

    //public void CallTxtChooserEndQuest()
    //{
    //    logSequenceEnd = TxtChooserEndQuest(quest);
    //    foreach (int seq in logSequenceEnd)
    //    {
    //        TextGenerator(shuffleBags[seq]);

    //    }
    //}

    //List<int> TxtChooserEndQuest(String quest)
    //{
    //    List<int> sequence = new List<int>();
    //    sequence.Add(0);

    //    sequence.Add(1);
    //    sequence.Add(9);

    //    return sequence;
    //}

    private int TranslateVector(int x, int y, int sideLength)
    {
        return x + (y * sideLength);
    }

    public void NumberTextUpdate(string tempString)
    {
        shuffleBags[4] = new ShuffleBag(1);
        shuffleBags[4] = LoadShuffleBag(shuffleBags[4], tempString, 1);
    } 
}
