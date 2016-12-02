using System;
using System.Collections.Generic;
using UnityEngine;

class PlayerBehaviour : ScorePublisher
{

    public int ScoreChange { get; set; }
    public string ScoreHandle { get; set; }

    private DummyManager dummyManager;

    public GameObject RepGainParticle;
    public GameObject RepLossParticle;

    public PlayerBehaviour(string handle)
    {
        dummyManager = DummyManager.dummyManager;

        switch (handle)
        {
            case "rep":
                try
                {
                    dummyManager.ReputationHandler.Subscribe(this);
                }
                catch (NullReferenceException)
                {
                    Debug.LogWarning("System manager not found!");
                    
                }
                break;
            case "susp":
                Debug.Log("NOT SUPPORTED"); break;
            case "days":
                Debug.Log("NOT SUPPORTED"); break;
            default:
                break;
        }
    }

    public void ChangeRepScore(int score)
    {
        if (dummyManager == null) return;

        if (score < 0)
        {
            RepLossParticle.SetActive(false);

            ScoreChange = dummyManager.GetComboMultiplier(score);

            //increase combo
            dummyManager.IncreaseCombo();
            //reset cooldown
            dummyManager.resetCooldown();

            //@@HARDCODED 
            if (dummyManager.GetComboValue() > 7)
                WwiseInterface.Instance.PlayRewardSound(RewardHandle.ComboStart);
            else if ((ScoreChange * -1) > 200)
                WwiseInterface.Instance.PlayRewardSound(RewardHandle.ComboBoost); // Previously "RewardHandle.Big"
            else
                WwiseInterface.Instance.PlayRewardSound(RewardHandle.Small);



            //particle effect
            RepLossParticle.SetActive(true);
        }
        else
        {
            RepGainParticle.SetActive(false);
            //particle effect
            RepGainParticle.SetActive(true);
            ResetCombo();

            WwiseInterface.Instance.PlayRewardSound(RewardHandle.Fail);
            ScoreChange = dummyManager.GetComboMultiplier(score);
           
        }
        
    }

    //reset combo
    public void ResetCombo()
    {
        if(dummyManager != null)
        {
            dummyManager.ResetCombo();
        }
    }

    public void Invoke()
    {
        OnChangeScoreEvent(new ScoreEventArgs(ScoreChange));
    }




}
