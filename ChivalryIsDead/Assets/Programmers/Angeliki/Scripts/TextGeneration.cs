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
        shuffleBagPal = new ShuffleBag(sentencesHello.text.Length);
        shuffleBagState = new ShuffleBag(sentencesState.text.Length);

        foreach (string sent in sentencesHello.text.Split('.'))
        {
            amount = 1;
            shuffleBagHello.Add(sent, amount);

        }
        foreach (string sent in sentencesPal.text.Split('.'))
        {
            amount = 1;
            shuffleBagPal.Add(sent, amount);

        }
        foreach (string sent in sentencesState.text.Split('.'))
        {
            amount = 1;
            shuffleBagState.Add(sent, amount);

        }

        sb.Append(shuffleBagHello.Next());
        sb.Append(shuffleBagPal.Next());
        for (int i = 0; i < 5; i++)
        {
            sb.Append(shuffleBagHello.Next());
            sb.Append(shuffleBagPal.Next());
            sb.Append(shuffleBagState.Next());

        }
        debugText.text = sb.ToString();

    }

    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            sb = new StringBuilder();
            sb.Append(shuffleBagHello.Next());
            sb.Append(shuffleBagPal.Next());
            for (int i = 0; i < 5; i++)
            {
                sb.Append(shuffleBagHello.Next());
                sb.Append(shuffleBagPal.Next());
                sb.Append(shuffleBagState.Next());

            }
            debugText.text = sb.ToString();
        }



    }
}
