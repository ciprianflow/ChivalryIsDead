using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

#region Handle Enums
public enum AmbienceHandle
{
    Hub, WorldOne
}

public enum UIHandle
{
    DialogueSpeechBubblePop, DialogueIconPop, QuestFinishedPositiveRep, QuestFinishedNegativeRep, QuestInitiated, WorldTransition
}

public enum MenuHandle
{
    ForwardButtonPressed, BackwardsButtonPressed, PlayButtonPressed
}

public enum KnightCombatHandle
{
    Attack, GainRep, LoseRepBig, LoseRepSmall, LoseRepCombo, OverreactPerfect, OverreactGreat, OverreactOK, Taunt, Walk
}

public enum MonsterHandle
{
    MeleeFishman, RangedTroll, SuicideGnome
}

public enum MonsterAudioHandle
{
    Aggro, Attack, Attacked, Death, Taunted, Walk
}

public enum UniqueMonsterAudioHandle
{
    MeleeCharge, RangeReload
}

public enum PeasantDialogueHandle
{
    Angry, Crazy, Death, Happy, Neutral, Random, Sad, Scared
}

public enum PrincessDialogueHandle
{
    Crazy, Happy, Sad, SuperCrazy, SuperHappy, SuperSad, Whimpy
}

public enum SwordDialogueHandle
{
    Angry, Explanatory, Funny, Happy, Neutral
}
#endregion


public interface IWwiseInterface
{
    void SetAmbience(AmbienceHandle handle);
    void PlayUISound(UIHandle handle);
    void PlayMenuSound(MenuHandle handle);
    void PlayKnightCombatSound(KnightCombatHandle handle, GameObject audioObject);
    void PlayGeneralMonsterSound(MonsterHandle m_handle, MonsterAudioHandle m_audioHandle, GameObject audioObject);
    void PlayUniqueMonsterSound(UniqueMonsterAudioHandle handle, GameObject audioObject);
    void PlayPeasantDialogue(PeasantDialogueHandle handle);
    void PlayPrincessDialogue(PrincessDialogueHandle handle);
    void PlaySwordDialogue(SwordDialogueHandle handle);
}

/// <summary>
/// Uses a bunch of switches to play sounds.
/// Should be fixed to use enum reflection where possible instead.
/// </summary>
public class WwiseInterface : MonoBehaviour, IWwiseInterface
{
    public static IWwiseInterface Instance;

    void Awake()
    {
        Instance = this;
    }

    public void SetAmbience(AmbienceHandle handle)
    {
        switch (handle) {
            case AmbienceHandle.Hub:
                AkSoundEngine.PostEvent("start_hub_ambience", gameObject); break;
            case AmbienceHandle.WorldOne:
                AkSoundEngine.PostEvent("start_world_1_ambience", gameObject); break;
            default:
                Debug.LogWarning(
                    string.Format("Handle of type '{0}' has no defined sound for value '{1}'.",
                    typeof(AmbienceHandle).Name, Enum.GetName(typeof(AmbienceHandle), handle)));
                return;
        }
    }

    public void PlayUISound(UIHandle handle)
    {
        switch (handle) {
            case UIHandle.DialogueSpeechBubblePop:
                AkSoundEngine.PostEvent("dialogue_a_speech_bubble_pop", gameObject); break;
            case UIHandle.DialogueIconPop:
                AkSoundEngine.PostEvent("dialogue_b_icon_pop", gameObject); break;
            case UIHandle.QuestInitiated:
                AkSoundEngine.PostEvent("quest_initiated", gameObject); break;
            case UIHandle.QuestFinishedNegativeRep:
                AkSoundEngine.PostEvent("quest_finished_negative_rep", gameObject); break;
            case UIHandle.QuestFinishedPositiveRep:
                AkSoundEngine.PostEvent("quest_finished_positive_rep", gameObject); break;
            case UIHandle.WorldTransition:
                AkSoundEngine.PostEvent("world_transition", gameObject); break;
            default:
                Debug.LogWarning(
                    string.Format("Handle of type '{0}' has no defined sound for value '{1}'.",
                    typeof(UIHandle).Name, Enum.GetName(typeof(UIHandle), handle)));
                return;
        }
    }

    public void PlayMenuSound(MenuHandle handle)
    {
        switch (handle) {
            case MenuHandle.ForwardButtonPressed:
                AkSoundEngine.PostEvent("normal_button_forward_pressed", gameObject); break;
            case MenuHandle.BackwardsButtonPressed:
                AkSoundEngine.PostEvent("normal_button_backwards_pressed", gameObject); break;
            case MenuHandle.PlayButtonPressed:
                AkSoundEngine.PostEvent("play_button_pressed", gameObject); break;
            default:
                Debug.LogWarning(
                    string.Format("Handle of type '{0}' has no defined sound for value '{1}'.",
                    typeof(AmbienceHandle).Name, Enum.GetName(typeof(AmbienceHandle), handle)));
                return;
        }
    }

    public void PlayKnightCombatSound(KnightCombatHandle handle, GameObject audioObject)
    {
        StringBuilder eventBuilder = new StringBuilder("knight_");
        switch (handle) {
            case KnightCombatHandle.Attack:
                eventBuilder.Append("attack"); break;
            case KnightCombatHandle.GainRep:
                eventBuilder.Append("gain_rep"); break;
            case KnightCombatHandle.LoseRepBig:
                eventBuilder.Append("loose_rep_big"); break;
            case KnightCombatHandle.LoseRepCombo:
                eventBuilder.Append("loose_rep_combo"); break;
            case KnightCombatHandle.LoseRepSmall:
                eventBuilder.Append("loose_rep_small"); break;
            case KnightCombatHandle.OverreactGreat:
                eventBuilder.Append("overreact_great"); break;
            case KnightCombatHandle.OverreactOK:
                eventBuilder.Append("overreact_ok"); break;
            case KnightCombatHandle.OverreactPerfect:
                eventBuilder.Append("overreact_perfect"); break;
            case KnightCombatHandle.Taunt:
                eventBuilder.Append("taunt"); break;
            case KnightCombatHandle.Walk:
                eventBuilder.Append("walk"); break;
            default:
                Debug.LogWarning(
                    string.Format("Handle of type '{0}' has no defined sound for value '{1}'.",
                    typeof(KnightCombatHandle).Name, Enum.GetName(typeof(KnightCombatHandle), handle)));
                return;
        }

        AkSoundEngine.PostEvent(eventBuilder.ToString(), audioObject);
    }

    public void PlayGeneralMonsterSound(MonsterHandle m_handle, MonsterAudioHandle m_audioHandle, GameObject audioObject)
    {
        StringBuilder eventBuilder = new StringBuilder();

        switch (m_handle) {
            case MonsterHandle.MeleeFishman:
                eventBuilder.Append("melee_"); break;
            case MonsterHandle.RangedTroll:
                eventBuilder.Append("ranged_"); break;
            case MonsterHandle.SuicideGnome:
                eventBuilder.Append("suicide_"); break;
            default:
                Debug.LogWarning(
                    string.Format("Handle of type '{0}' has no defined sound for value '{1}'.",
                    typeof(MonsterHandle).Name, Enum.GetName(typeof(MonsterHandle), m_handle)));
                return;
        }

        switch (m_audioHandle) {
            case MonsterAudioHandle.Aggro:
                eventBuilder.Append("aggro"); break;
            case MonsterAudioHandle.Attack:
                eventBuilder.Append("attack"); break;
            case MonsterAudioHandle.Attacked:
                eventBuilder.Append("attacked"); break;
            case MonsterAudioHandle.Death:
                eventBuilder.Append("death"); break;
            case MonsterAudioHandle.Taunted:
                eventBuilder.Append("taunted"); break;
            case MonsterAudioHandle.Walk:
                eventBuilder.Append("walk"); break;
            default:
                Debug.LogWarning(
                    string.Format("Handle of type '{0}' has no defined sound for value '{1}'.",
                    typeof(MonsterAudioHandle).Name, Enum.GetName(typeof(MonsterAudioHandle), m_audioHandle)));
                return;
        }

        AkSoundEngine.PostEvent(eventBuilder.ToString(), audioObject);
    }

    public void PlayUniqueMonsterSound(UniqueMonsterAudioHandle handle, GameObject audioObject)
    {
        switch (handle) {
            case UniqueMonsterAudioHandle.MeleeCharge:
                AkSoundEngine.PostEvent("melee_charge", audioObject); break;
            case UniqueMonsterAudioHandle.RangeReload:
                AkSoundEngine.PostEvent("ranged_take_out_throwing_object", audioObject); break;
            default:
                Debug.LogWarning(
                    string.Format("Handle of type '{0}' has no defined sound for value '{1}'.",
                    typeof(UniqueMonsterAudioHandle).Name, Enum.GetName(typeof(UniqueMonsterAudioHandle), handle)));
                return;
        }
    }

    public void PlayPeasantDialogue(PeasantDialogueHandle handle)
    {
        StringBuilder eventBuilder = new StringBuilder("peasant_");

        //var eventName = Enum.GetName(typeof(PeasantDialogueHandle), handle).ToLower();
        //eventBuilder.Append(eventName);

        switch (handle) {
            case PeasantDialogueHandle.Angry:
                eventBuilder.Append("angry"); break;
            case PeasantDialogueHandle.Crazy:
                eventBuilder.Append("crazy"); break;
            case PeasantDialogueHandle.Death: 
                eventBuilder.Append("death"); break;
            case PeasantDialogueHandle.Happy:
                eventBuilder.Append("happy"); break;
            case PeasantDialogueHandle.Neutral:
                eventBuilder.Append("neutral"); break;
            case PeasantDialogueHandle.Random:
                eventBuilder.Append("random"); break;
            case PeasantDialogueHandle.Sad:
                eventBuilder.Append("sad"); break;
            case PeasantDialogueHandle.Scared:
                eventBuilder.Append("scared"); break;
            default:
                Debug.LogWarning(
                    string.Format("Handle of type '{0}' has no defined sound for value '{1}'.",
                    typeof(PeasantDialogueHandle).Name, Enum.GetName(typeof(PeasantDialogueHandle), handle)));
                return;
        }

        AkSoundEngine.PostEvent(eventBuilder.ToString(), gameObject);
    }

    public void PlayPrincessDialogue(PrincessDialogueHandle handle)
    {
        StringBuilder eventBuilder = new StringBuilder("princess_");

        switch (handle) {
            case PrincessDialogueHandle.Crazy:
                eventBuilder.Append("crazy"); break;
            case PrincessDialogueHandle.Happy:
                eventBuilder.Append("happy"); break;
            case PrincessDialogueHandle.Sad:
                eventBuilder.Append("sad"); break;
            case PrincessDialogueHandle.SuperCrazy:
                eventBuilder.Append("super_crazy"); break;
            case PrincessDialogueHandle.SuperHappy:
                eventBuilder.Append("super_happy"); break;
            case PrincessDialogueHandle.SuperSad:
                eventBuilder.Append("super_sad"); break;
            case PrincessDialogueHandle.Whimpy:
                eventBuilder.Append("whimpy"); break;
            default:
                Debug.LogWarning(
                    string.Format("Handle of type '{0}' has no defined sound for value '{1}'.",
                    typeof(PrincessDialogueHandle).Name, Enum.GetName(typeof(PrincessDialogueHandle), handle)));
                return;
        }

        AkSoundEngine.PostEvent(eventBuilder.ToString(), gameObject);
    }

    public void PlaySwordDialogue(SwordDialogueHandle handle)
    {
        StringBuilder eventBuilder = new StringBuilder("sword_");
        switch (handle) {
            case SwordDialogueHandle.Angry:
                eventBuilder.Append("angry"); break;
            case SwordDialogueHandle.Explanatory:
                eventBuilder.Append("explanatory"); break;
            case SwordDialogueHandle.Funny:
                eventBuilder.Append("funny"); break;
            case SwordDialogueHandle.Happy:
                eventBuilder.Append("happy"); break;
            case SwordDialogueHandle.Neutral:
                eventBuilder.Append("neutral"); break;
            default:
                Debug.LogWarning(
                    string.Format("Handle of type '{0}' has no defined sound for value '{1}'.",
                    typeof(SwordDialogueHandle).Name, Enum.GetName(typeof(SwordDialogueHandle), handle)));
                return;
        }

        AkSoundEngine.PostEvent(eventBuilder.ToString(), gameObject);
    }

}
