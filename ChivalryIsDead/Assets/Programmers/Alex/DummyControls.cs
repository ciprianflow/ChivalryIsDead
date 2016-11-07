using System;
using System.Collections.Generic;
using UnityEngine;

public class ControlBehaviour : ScorePublisher
{
    public int ScoreChange { get; set; }
    public string ScoreHandle { get; set; }

    public ControlBehaviour(string handle)
    {
        switch (handle) {
            case "rep":
                DummyManager.dummyManager.ReputationHandler.Subscribe(this); break;
            case "susp":
                DummyManager.dummyManager.SuspicionHandler.Subscribe(this); break;
            case "days":
                DummyManager.dummyManager.DaysRemaining.Subscribe(this); break;
            default:
                break;
        }
    }

    public void Invoke()
    {
        OnChangeScoreEvent(new ScoreEventArgs(ScoreChange));
    }
}

public class DummyControls : MonoBehaviour
{
    private Dictionary<KeyCode, List<ControlBehaviour>> controlDict;

    void Start()
    {
        controlDict = new Dictionary<KeyCode, List<ControlBehaviour>>() {
            { KeyCode.Keypad1, new List<ControlBehaviour> {
                new ControlBehaviour("rep") { ScoreChange = 4 },
                new ControlBehaviour("susp") { ScoreChange = -12 }
            } },
            { KeyCode.Keypad2, new List<ControlBehaviour> {
                new ControlBehaviour("rep") { ScoreChange = -4 }
            } }
        };
    }

    void Update()
    {
        foreach (KeyCode key in controlDict.Keys) {
            if (Input.GetKeyDown(key)) {
                foreach (ControlBehaviour cb in controlDict[key]) { 
                    cb.Invoke();
                }
            }
        }
    }
}
