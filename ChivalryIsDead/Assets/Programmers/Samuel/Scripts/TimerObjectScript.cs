using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class TimerObjectScript : MonoBehaviour, IObjectiveTarget {

    int id = 31;

    float timer = 0;
    float maxTime = 120;

    Image timerImage;
    GameObject Dsystem;

    void Awake()
    {
        timerImage = GameObject.Find("Canvas").transform.FindChild("TimerBG").FindChild("Timer").GetComponent<Image>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= maxTime)
            StaticIngameData.mapManager.CheckObjectives(this);

        if (timerImage != null)
            timerImage.fillAmount = 1 - timer / maxTime;

        if (timer > maxTime / 2)
        {
            Dsystem = GameObject.FindGameObjectWithTag("DialogSystem");
            Dsystem.GetComponent<Gameplay_Dialog>().HalfTime();
        }
    }

    public int ID
    {
        get
        {
            return id;
        }
    }

    public int Health
    {
        get
        {
            return (int)(maxTime - timer);
        }
    }

    public int MaxHealth
    {
        get
        {
            return (int)maxTime;
        }
    }

    public bool IsChecked
    {
        get ; set ;
    }

}
