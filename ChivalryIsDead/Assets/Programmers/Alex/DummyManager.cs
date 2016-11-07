using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class DummyManager : MonoBehaviour
{
    public static DummyManager dummyManager;

    public ScoreHandler ReputationHandler;
    public ScoreHandler SuspicionHandler;
    public ScoreHandler DaysRemaining;

    void Awake()
    {
        DummyManager.dummyManager = this;
    }
}
