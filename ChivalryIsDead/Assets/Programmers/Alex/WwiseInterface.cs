using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

#region Handle Enums
public enum UIHandle
{
    DialogueSpeechBubblePop, DialogueIconPop, QuestFinishedPositiveRep, QuestFinishedNegativeRep, QuestInitiated, WorldTransition
}

public enum MenuHandle
{
    ForwardButtonPressed, BackwardsButtonPressed, PlayButtonPressed
}

public enum CombatHandle
{
    ImpactFlesh
}

#region Characters
public enum KnightDialogueHandle
{
    Angry, Grumpy, Happy, Neutral
}

public enum KnightCombatHandle
{
    Attack, GainRep, LoseRepBig, LoseRepSmall, LoseRepCombo, OverreactPerfect, OverreactGreat, OverreactOk, Taunt, Walk
}

public enum MonsterHandle
{
    Other, Melee, Ranged, Suicide
}

public enum MonsterAudioHandle
{
    Aggro, Attack, Attacked, Death, Taunted, Walk
}

public enum UniqueMonsterAudioHandle
{
    MeleeCharge, RangeReload, SuicideCharge, SuicideExplode, SuicideFlungAway
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
    Angry, Crazy, Determined, ExplanatoryLong, ExplanatoryShort, HappyLong, HappyShort, Neutral
}
#endregion

public enum MusicHandle
{
    MusicOnePlay, MusicStop
}

public enum RewardHandle
{
    ComboBoost, ComboStart, ComboEnd, Small
}

public enum AmbienceHandle
{
    Hub, WorldOne
}
#endregion

public interface IWwiseInterface
{
    void SetAmbience(AmbienceHandle handle);
    void SetMusic(MusicHandle handle);
    void PlayRewardSound(RewardHandle handle);
    void PlayUISound(UIHandle handle);
    void PlayMenuSound(MenuHandle handle);
    void PlayKnightCombatSound(KnightCombatHandle handle, GameObject audioObject);
    void PlayGeneralMonsterSound(MonsterHandle m_handle, MonsterAudioHandle m_audioHandle, GameObject audioObject);
    void PlayUniqueMonsterSound(UniqueMonsterAudioHandle handle, GameObject audioObject);
    void PlayCombatSound(CombatHandle handle, GameObject audioObject);
    void PlayPeasantDialogue(PeasantDialogueHandle handle);
    void PlayPrincessDialogue(PrincessDialogueHandle handle);
    void PlaySwordDialogue(SwordDialogueHandle handle);
}

/// <summary>
/// Uses a bunch of switches to play sounds.
/// Should be fixed to use enum reflection where possible instead.
/// 
/// Audio that is hacked due to communication issues between sound engineers
/// and Wwise programmer:
/// - 'MonsterHandle.Ranged' and 'MonsterAudioHandle.Attack' coerced to 'ranged_throw'
/// - Music uses custom sanitizer.
/// 
/// </summary>
public class WwiseInterface : MonoBehaviour, IWwiseInterface
{
    public static IWwiseInterface Instance;

    void Awake()
    {
        Instance = this;
    }

    public void SetMusic(MusicHandle handle)
    {
        switch (handle) {
            case MusicHandle.MusicOnePlay:
                AkSoundEngine.PostEvent("music1Play", gameObject); break;
            case MusicHandle.MusicStop:
                AkSoundEngine.PostEvent("musicStop", gameObject); break;
            default:
                DebugError(handle); return;
        }
    }

    public void SetAmbience(AmbienceHandle handle)
    {
        switch (handle) {
            case AmbienceHandle.Hub:
                AkSoundEngine.PostEvent("start_hub_ambience", gameObject); break;
            case AmbienceHandle.WorldOne:
                AkSoundEngine.PostEvent("start_world_1_ambience", gameObject); break;
            default:
                DebugError(handle); return;
        }
    }

    public void PlayRewardSound(RewardHandle handle)
    {
        StringBuilder eventBuilder = new StringBuilder("reward_");

        eventBuilder.Append(HandleToEventStringCamel(handle));

        AkSoundEngine.PostEvent(eventBuilder.ToString(), gameObject);
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
                DebugError(handle); return;
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
                DebugError(handle); return;
        }
    }

    public void PlayKnightCombatSound(KnightCombatHandle handle, GameObject audioObject)
    {
        StringBuilder eventBuilder = new StringBuilder("knight_");
        eventBuilder.Append(HandleToEventString(handle));

        AkSoundEngine.PostEvent(eventBuilder.ToString(), audioObject);
    }

    public void PlayGeneralMonsterSound(MonsterHandle m_handle, MonsterAudioHandle m_audioHandle, GameObject audioObject)
    {
        if (m_handle == MonsterHandle.Other)
            return; // Placeholder to avoid sheep and others from playing sounds and throwing exceptions.

        StringBuilder eventBuilder = new StringBuilder();

        eventBuilder.Append(HandleToEventString(m_handle) + "_");
        eventBuilder.Append(HandleToEventString(m_audioHandle));

        // This should be phased out with proper naming of the events.
        try {
            AkSoundEngine.PostEvent(eventBuilder.ToString(), audioObject);
        } catch (Exception) {
            if (m_handle == MonsterHandle.Ranged && m_audioHandle == MonsterAudioHandle.Attack)
                AkSoundEngine.PostEvent("ranged_throw", audioObject);
            else {
                Debug.LogWarning(
                    string.Format("Invalid handle. Handles presented were:" + Environment.NewLine +
                    "'{1}' of type '{2}', and '{3}' of type '{4}'",
                    typeof(MonsterHandle).Name, Enum.GetName(typeof(MonsterHandle), m_handle),
                    typeof(MonsterAudioHandle).Name, Enum.GetName(typeof(MonsterAudioHandle), m_audioHandle)));
                return;
            }


        }
    }

    public void PlayUniqueMonsterSound(UniqueMonsterAudioHandle handle, GameObject audioObject)
    {
        switch (handle) {
            case UniqueMonsterAudioHandle.MeleeCharge:
                AkSoundEngine.PostEvent("melee_charge", audioObject); break;
            case UniqueMonsterAudioHandle.RangeReload:
                AkSoundEngine.PostEvent("ranged_take_out_throwing_object", audioObject); break;
            case UniqueMonsterAudioHandle.SuicideCharge:
                AkSoundEngine.PostEvent("suicide_charge", audioObject); break;
            case UniqueMonsterAudioHandle.SuicideFlungAway:
                AkSoundEngine.PostEvent("suicide_flung_away", audioObject); break;
            case UniqueMonsterAudioHandle.SuicideExplode:
                AkSoundEngine.PostEvent("suicide_explode", audioObject); break;
            default:
                DebugError(handle); return;
        }
    }

    public void PlayPeasantDialogue(PeasantDialogueHandle handle)
    {
        StringBuilder eventBuilder = new StringBuilder("peasant_");
        eventBuilder.Append(HandleToEventString(handle));

        AkSoundEngine.PostEvent(eventBuilder.ToString(), gameObject);
        // Fixed as of 23-11-2016.
        //try {
        //    AkSoundEngine.PostEvent(eventBuilder.ToString(), gameObject);
        //} catch (Exception) {
        //    if (handle == PeasantDialogueHandle.Neutral)
        //        AkSoundEngine.PostEvent("peasany_neutral", gameObject);
        //}
    }

    public void PlayPrincessDialogue(PrincessDialogueHandle handle)
    {
        StringBuilder eventBuilder = new StringBuilder("princess_");
        eventBuilder.Append(HandleToEventString(handle));

        AkSoundEngine.PostEvent(eventBuilder.ToString(), gameObject);
    }

    public void PlaySwordDialogue(SwordDialogueHandle handle)
    {
        StringBuilder eventBuilder = new StringBuilder("sword_");
        eventBuilder.Append(HandleToEventString(handle));

        AkSoundEngine.PostEvent(eventBuilder.ToString(), gameObject);
    }

    public void PlayCombatSound(CombatHandle handle, GameObject audioObject)
    {
        StringBuilder eventBuilder = new StringBuilder();
        eventBuilder.Append(HandleToEventString(handle));

        AkSoundEngine.PostEvent(eventBuilder.ToString(), audioObject);
    }

    private string HandleToEventString(Enum handle)
    {
        var enumName = Enum.GetName(handle.GetType(), handle);
        Regex pattern = new Regex(@"([A-Z][a-z]+)");
        MatchCollection matches = pattern.Matches(enumName);

        string[] matchesArray = new string[matches.Count];
        for (int i = 0; i < matchesArray.Length; i++) 
            matchesArray[i] = matches[i].ToString().ToLower();

        return string.Join("_", matchesArray);
    }

    private string HandleToEventStringCamel(Enum handle)
    {
        // THIS IS WHY YOU MAINTAIN NAMING CONVENTIONS!!
        // Staight ripped from HandleToEventString method.
        // Adjustments made to control for camelcasing instead of underscores.
        var enumName = Enum.GetName(handle.GetType(), handle);
        Regex pattern = new Regex(@"([A-Z][a-z]+)");
        MatchCollection matches = pattern.Matches(enumName);

        string[] matchesArray = new string[matches.Count];
        matchesArray[0] = matchesArray[0].ToLower();
        return string.Join("", matchesArray);
    }

    private void DebugError(Enum handle)
    {
        Debug.LogWarning(
            string.Format("Handle of type '{0}' has no defined sound for value '{1}'.",
            handle.GetType().Name, Enum.GetName(handle.GetType(), handle)));
    }
}
