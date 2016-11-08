using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class TextGeneration : MonoBehaviour {

    public StringBuilder sb = new StringBuilder();

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

    Text debugText;

    // Use this for initialization
    void Start() {

       

        //debugText = GameObject.FindWithTag("DebugText").GetComponent<Text>();
        debugText = transform.GetChild(2).GetComponent<Text>();
        //debugText = gameObject.GetComponent<Text>();
        debugText.text = "";
       
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


        initTextBags(NewBagInitializer);

        // temporary place - Remember
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
        sb = new StringBuilder();
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

    //public StringBuilder TextGenerator(ShuffleBag shuffleBag)
    void TextGenerator(ShuffleBag shuffleBag)
    {
       
        sb.Append(shuffleBag.Next());
        debugText.text = sb.ToString();
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

    public void NumberTextUpdate(string tempString)
    {
        shuffleBags[3] = new ShuffleBag(1);
        shuffleBags[3] = LoadShuffleBag(shuffleBags[3], tempString, 1);
    } 
}
