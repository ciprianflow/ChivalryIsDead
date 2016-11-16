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
    [HideInInspector]
    public List<ShuffleBag> shuffleBags = new List<ShuffleBag>();
    //public List<string> filesNames = new List<string>();

    Text QuestTitleText;
    Text title;
    Text mainText;
    Text Description;
    Text endTitle;

    List<int> logSequenceStart = new List<int>();
    List<int> logSequenceEnd = new List<int>();

    GameObject HubDataMan;
    int QuestNum;

    private string QuestTitle = "";
    private string QuestDifficulty = "";
    private string QuestText = "";

    public void SetQuestText(string description, string title, string difficulty)
    {
        QuestText = description;
        QuestTitle = title;
        QuestDifficulty = difficulty;
        Debug.Log(QuestText + " " +  QuestTitle + " " +  QuestDifficulty);
    }

    // Use this for initialization
    void Start() {
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
        endTitle = transform.FindChild("EndTitle").GetComponent<Text>();

        QuestTitleText.text = QuestTitle;
        Description.text = QuestText;

        //debugText = gameObject.GetComponent<Text>();
        mainText.text = "";

        foreach (TextAsset textFile in Resources.LoadAll("txts", typeof(TextAsset)))
        {
            sentences.Add(textFile);
            //filesNames.Add(textFile.name);
        }

        foreach (TextAsset textFile in sentences)
        {
            shuffleBags.Add(new ShuffleBag(textFile.text.Split('/').Length));
        }
     
        for (int i = 0; i < shuffleBags.Count; i++)
        {
            shuffleBags[i] = LoadShuffleBag(shuffleBags[i], sentences[i].text, 1);
        }

        TitleGenerator(shuffleBags[0]);
        EndTitleGenerator(shuffleBags[shuffleBags.Count-1]);

        if (gameObject.tag == "EndLetter")
            initTextBags(NewBagInitializer);
        else
            CallTxtChooserStartQuest();

        // temporary - Remember!!!!
        killString = "12" + " ";
        NumberTextUpdate(killString);

        //sb = TextGenerator(shuffleBagHello);
        //sb = TextGenerator(shuffleBagPal);
        //for (int i = 0; i < 5; i++)
        //{
        //    sb = TextGenerator(shuffleBagHello);
        //    sb = TextGenerator(shuffleBagPal);
        //    sb = TextGenerator(shuffleBagState);

        //}
        //debugText.text = sb.ToString();

    }

    // Update is called once per frame
    void Update () {
        //if (Input.GetMouseButtonDown(0))
        //{

        //    sb = new StringBuilder();
        //    sb = TextGenerator(shuffleBagHello);
        //    sb = TextGenerator(shuffleBagPal);
        //    for (int i = 0; i < 5; i++)
        //    {
        //        sb = TextGenerator(shuffleBagHello);
        //        sb = TextGenerator(shuffleBagPal);
        //        sb = TextGenerator(shuffleBagState);

        //    }

        //    debugText.text = sb.ToString();
        //}
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
        logSequenceStart = TxtChooserStartQuest(QuestNum);
        foreach (int seq in logSequenceStart)
        {
            TextGenerator(shuffleBags[seq]);

        }
    }

    List<int> TxtChooserStartQuest(int quest)
    {
        List<int> sequence = new List<int>();
        sequence.Add(1);
        sequence.Add(2);

        if (quest == 0)
        {
            sequence.Add(8);
        }
        else if (quest == 1)
        {
            sequence.Add(9);
        }

        sequence.Add(10);

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
