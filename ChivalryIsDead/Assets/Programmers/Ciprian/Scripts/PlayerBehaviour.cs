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

    public GameObject ComboBaseParticle;
    public GameObject ComboUpwardParticle;

    public PlayerBehaviour(string handle)
    {
        dummyManager = DummyManager.dummyManager;

        switch (handle)
        {
            case "rep":
                dummyManager.ReputationHandler.Subscribe(this); break;
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

        if (score < 0)
        {
            RepLossParticle.SetActive(false);

            ScoreChange = dummyManager.GetComboMultiplier(score);

            //@@HARDCODED 
            if (dummyManager.GetComboValue() > 7)
                WwiseInterface.Instance.PlayRewardSound(RewardHandle.ComboStart);
            else if ((ScoreChange * -1) > 200)
                WwiseInterface.Instance.PlayRewardSound(RewardHandle.Big);
            else
                WwiseInterface.Instance.PlayRewardSound(RewardHandle.Small);

            //increase combo
            dummyManager.IncreaseCombo();
            //reset cooldown
            dummyManager.resetCooldown();

            //particle effect
            RepLossParticle.SetActive(true);

            
            ComboBaseParticle.GetComponent<ParticleSystem>().startSize = dummyManager.GetComboValue();
            
            ComboUpwardParticle.GetComponent<ParticleSystem>().startSize = 0.1f + (dummyManager.GetComboValue() * 2f )/100f;
        }
        else
        {
            RepGainParticle.SetActive(false);
            //particle effect
            RepGainParticle.SetActive(true);
            Reset();

            WwiseInterface.Instance.PlayRewardSound(RewardHandle.Fail);
            ScoreChange = dummyManager.GetComboMultiplier(score);
            ComboBaseParticle.GetComponent<ParticleSystem>().startSize = 0.1f;
            ComboUpwardParticle.GetComponent<ParticleSystem>().startSize = 0.01f;
        }
        
    }

    //reset combo
    public void Reset()
    {
        dummyManager.ResetCombo();
    }



    public void Invoke()
    {
        dummyManager.ActionPerformed();

        OnChangeScoreEvent(new ScoreEventArgs(ScoreChange));
    }




}
