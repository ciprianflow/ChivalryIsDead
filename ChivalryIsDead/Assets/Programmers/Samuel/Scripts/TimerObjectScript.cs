using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class TimerObjectScript : MonoBehaviour, IObjectiveTarget {

    int id = 31;

    float timer = 0;
    float maxTime = 120;

    float halfTime;
    public static bool isReminded;

    Image timerImage;
    GameObject Dsystem;
    public static TimerObjectScript Instance;

    void Awake()
    {
        timerImage = GameObject.Find("Canvas").transform.FindChild("TimerBG").FindChild("Timer").GetComponent<Image>();
    }

    void Start()
    {
        Instance = this;
        halfTime = maxTime / 2;
        isReminded = false;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= maxTime)
            StaticIngameData.mapManager.CheckObjectives(this);

        if (timerImage != null)
            timerImage.fillAmount = 1 - timer / maxTime;

        if (!isReminded && timer > halfTime)
        {
            Dsystem = GameObject.FindGameObjectWithTag("DialogSystem");
            Dsystem.GetComponent<Gameplay_Dialog>().HalfTime();
            isReminded = true;
        }

    }

    public float GetMaxTime()
    {
        return maxTime;
    }

    public float GetTimer()
    {
        return timer;
    }
    public float GetElapsedTime()
    {

        return 1 - timer / maxTime;
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
