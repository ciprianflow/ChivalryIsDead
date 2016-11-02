using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;
using System;
public class TextGeneration : MonoBehaviour {

    ShuffleBag shuffleBagHello;
    ShuffleBag shuffleBagPal;
    ShuffleBag shuffleBagState;

    StringBuilder sb = new StringBuilder();

    int amount;
    //string sentencesHello;
    //string sentencesPal;
    //string sentencesState;

    TextAsset sentencesHello;
    TextAsset sentencesPal;
    TextAsset sentencesState;

    Text debugText;

    // Use this for initialization
    void Start () {

        debugText = GameObject.FindWithTag("DebugText").GetComponent<Text>();
        debugText.text = "";
        //sentencesHello = " hi. goodmorning. good evening. Hello. How are you?";
        //sentencesPal = " dude. friend. boy. bro. man";
        //sentencesState = " You won!. You lose!. You are the best knight!. You suck";

        sentencesHello = Resources.Load("txts/Hello") as TextAsset;
        sentencesPal = Resources.Load("txts/Pal") as TextAsset;
        sentencesState = Resources.Load("txts/State") as TextAsset;

        shuffleBagHello = new ShuffleBag(sentencesHello.text.Length);
        shuffleBagPal = new ShuffleBag(sentencesPal.text.Length);
        shuffleBagState = new ShuffleBag(sentencesState.text.Length);

        shuffleBagHello = LoadShuffleBag(shuffleBagHello, sentencesHello, 1);
        shuffleBagPal = LoadShuffleBag(shuffleBagPal, sentencesPal, 1);
        shuffleBagState = LoadShuffleBag(shuffleBagState, sentencesState, 1);


        sb = TextGenerator(shuffleBagHello);
        sb = TextGenerator(shuffleBagPal);
        for (int i = 0; i < 5; i++)
        {
            sb = TextGenerator(shuffleBagHello);
            sb = TextGenerator(shuffleBagPal);
            sb = TextGenerator(shuffleBagState);

        }
        debugText.text = sb.ToString();

    }

    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            
            sb = new StringBuilder();
            sb = TextGenerator(shuffleBagHello);
            sb = TextGenerator(shuffleBagPal);
            for (int i = 0; i < 5; i++)
            {
                sb = TextGenerator(shuffleBagHello);
                sb = TextGenerator(shuffleBagPal);
                sb = TextGenerator(shuffleBagState);

            }

            debugText.text = sb.ToString();
        }



    }

    ShuffleBag LoadShuffleBag(ShuffleBag shuffleBag, TextAsset sentences, int amount)
    {
        foreach (string sent in sentences.text.Split('/'))
        {
            shuffleBag.Add(sent, amount);

        }
        return shuffleBag;
    }

    StringBuilder TextGenerator(ShuffleBag shuffleBag)
    {
        sb.Append(shuffleBag.Next());
        return sb;
    }
}
