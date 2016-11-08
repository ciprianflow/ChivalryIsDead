// https://gamedevelopment.tutsplus.com/tutorials/shuffle-bags-making-random-feel-more-random--gamedev-1249

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShuffleBag
{
    private Random random = new Random();
    private List<string> data;

    private string currentItem;
    private int currentPosition = -1;

    private int Capacity { get { return data.Capacity; } }
    public int Size { get { return data.Count; } }

    public ShuffleBag(int initCapacity)
    {
        data = new List<string>(initCapacity);
    }

    public void Add(string item, int amount)
    {
        for (int i = 0; i < amount; i++)
            data.Add(item);
       
        currentPosition = Size - 1;
    }

    public string Next()
    {
        if (currentPosition < 1)
        {
            currentPosition = Size - 1;
            currentItem = data[0];

            return currentItem;
        }

        //int pos = random.Next(currentPosition);
        int pos = Random.Range(0, currentPosition);

        currentItem = data[pos];
        data[pos] = data[currentPosition];
        data[currentPosition] = currentItem;
        currentPosition--;

        return currentItem;
    }
}