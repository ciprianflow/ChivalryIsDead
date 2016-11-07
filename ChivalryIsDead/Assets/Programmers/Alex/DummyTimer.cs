using UnityEngine;
using System.Collections;

public class DummyTimer : ScoreTextHandler {

    public int MaxTime;

    private ControlBehaviour daysBehaviour;
    private ControlBehaviour idleBehaviour;
    private ControlBehaviour idleRepBehaviour;

    void Start()
    {
        daysBehaviour = new ControlBehaviour("days") { ScoreChange = -1 };
        idleBehaviour = new ControlBehaviour("susp") { ScoreChange = 12 };
        idleRepBehaviour = new ControlBehaviour("rep") { ScoreChange = -8 };
        StartCoroutine(RunTimer());
    }

    private IEnumerator RunTimer()
    {
        Score = MaxTime;
        while (Score > 0) {
            yield return new WaitForSeconds(1f);
            Score--;
            idleBehaviour.Invoke();
            idleRepBehaviour.Invoke();
        }
        daysBehaviour.Invoke();
        StartCoroutine(RunTimer());
    }
}
