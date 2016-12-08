using System;
using System.Collections;
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
    ImpactFlesh, ImpactArmor, ImpactStone
}

#region Characters
public enum KnightCombatVoiceHandle
{
    Attack, OverreactPerfect, OverreactGreat, OverreactOk, Taunt, TauntAlt
}

public enum KnightCombatSFXHandle
{
    Walk, OverreactTomatoSplat
}

public enum MonsterHandle
{
    Other, Melee, Ranged, Suicide
}

public enum MonsterAudioHandle
{
    Aggro, Attack, Attacked, Death, Taunted, Walk
}

public enum UniqueMeleeAudioHandle
{
    Charge, AttackChargeUp
}

public enum UniqueRangedAudioHandle
{
    LookForStone, FindStone, PickupStone
}

public enum UniqueSuicideAudioHandle
{
    Charge, FlungAway
}

public enum PrincessDialogueHandle
{
    Crazy, Flirty, Happy, Sad, SuperCrazy, SuperFlirty, SuperHappy, SuperSad
}

public enum SwordDialogueHandle
{
    Angry, Crazy, Determined, ExplanatoryLong, ExplanatoryShort, HappyLong, HappyShort, Neutral
}
#endregion

public enum VolumeHandle
{
    Master, SFX, Music
}

public enum MusicHandle
{
    MusicStop, MusicQuest, MusicOnePlay
}

public enum RewardHandle
{
    PointCounter, ComboBoost, ComboBoost2, ComboBoost3, ComboStart, ComboEnd, Small, Fail
}

public enum SheepAudioHandle
{
    NeutralLoop, Taunted
}

public enum AmbienceHandle
{
    Hub, WorldOne
}

public enum GameStateHandle
{
    Paused, Unpaused
}
#endregion

#region Unused Handles
public enum KnightDialogueHandle
{
    Angry, Grumpy, Happy, Neutral
}

public enum PeasantDialogueHandle
{
    Angry, Crazy, Death, Happy, Neutral, Random, Sad, Scared
}
#endregion

public interface IWwiseInterface
{
    MusicHandle CurrentlyPlaying { get; }

    // Audio settings
    void SetVolume(float volume, VolumeHandle handle);

    // Non-Targeted audio.
    void SetAmbience(AmbienceHandle handle);
    void SetMusic(MusicHandle handle);
    void StopEvent(string eventName);
    void PlayRewardSound(RewardHandle handle);
    void PlayUISound(UIHandle handle);
    void PlayMenuSound(MenuHandle handle);
    void PlayPeasantDialogue(PeasantDialogueHandle handle);
    void PlayPrincessDialogue(PrincessDialogueHandle handle);
    void PlaySwordDialogue(SwordDialogueHandle handle);

    // Targeted audio
    void PlayKnightCombatSFX(KnightCombatSFXHandle handle, GameObject audioObject);
    void PlayKnightCombatVoiceSound(KnightCombatVoiceHandle handle, GameObject audioObject);
    void PlayGeneralMonsterSound(MonsterHandle m_handle, MonsterAudioHandle m_audioHandle, GameObject audioObject);
    void PlayUniqueMeleeSound(UniqueMeleeAudioHandle handle, GameObject audioObject);
    void PlayUniqueRangedSound(UniqueRangedAudioHandle handle, GameObject audioObject);
    void PlayUniqueSuicideSound(UniqueSuicideAudioHandle handle, GameObject audioObject);
    void PlayCombatSound(CombatHandle handle, GameObject audioObject);
    void PlaySheepSound(SheepAudioHandle handle, GameObject audioObject);
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

    public MusicHandle CurrentlyPlaying { get; private set; }

    void Awake()
    {
        var interfaces = FindObjectsOfType<WwiseInterface>();
        if (interfaces.Length > 1 && Instance != this) {
            UnityEngine.Object.DestroyImmediate(gameObject);
        }
        else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        //DontDestroyOnLoad(gameObject);
    }

    #region Audio settings
    public void SetVolume(float volume, VolumeHandle type)
    {
        volume = Mathf.Clamp(volume, 0, 100);
        var eventBuilder = new StringBuilder("_volume");
        eventBuilder.Insert(0, HandleToEventString(type));
        AkSoundEngine.SetRTPCValue(eventBuilder.ToString(), volume);
    }
    #endregion

    #region Non-Targeted Audio
    /// <summary>
    /// Sets the currently running background music loop.
    /// The method will first stop any currently playing music (if it is not the same), and start the new music loop.
    /// Stopping the music will stop it in it's entirety.
    /// </summary>
    /// <param name="delay">The desired delay (in seconds) before starting the new music.</param>
    /// <param name="handle">The desired music. MusicOne is used for the menus and quest hub, MusicQuest is used for the battle board.</param>
    public void SetMusic(MusicHandle handle) { SetMusic(handle, 0.33f); }
    public void SetMusic(MusicHandle handle, float delay)
    {
        if (CurrentlyPlaying != handle)
            StartCoroutine(SwitchMusic(handle, 0.33f));
    }
    public void StopEvent(string eventName) {
        uint eventID;
        eventID = AkSoundEngine.GetIDFromString(eventName);
        AkSoundEngine.ExecuteActionOnEvent(eventID, AkActionOnEventType.AkActionOnEventType_Stop, gameObject, 0, AkCurveInterpolation.AkCurveInterpolation_Sine);
    }

    public void SetAmbience(AmbienceHandle handle)
    {
        switch (handle) {
            case AmbienceHandle.Hub:
                AkSoundEngine.PostEvent("start_hub_ambience", gameObject); break;
            case AmbienceHandle.WorldOne:
                AkSoundEngine.PostEvent("start_world_1_ambience", gameObject); break;
            default:
                LogError(handle); return;
        }
    }

    public void PlayRewardSound(RewardHandle handle)
    {
        if (handle == RewardHandle.PointCounter) {
            AkSoundEngine.PostEvent("pointCounter", gameObject);
            return;
        }
        StringBuilder eventBuilder = new StringBuilder("reward_");
        eventBuilder.Append(HandleToEventString(handle));

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
                LogError(handle); return;
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
                LogError(handle); return;
        }
    }

    public void PlayPeasantDialogue(PeasantDialogueHandle handle)
    {
        StringBuilder eventBuilder = new StringBuilder("peasant_");
        eventBuilder.Append(HandleToEventString(handle));

        AkSoundEngine.PostEvent(eventBuilder.ToString(), gameObject);
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


    #endregion

    #region Targeted Audio
    public void PlayKnightCombatSFX(KnightCombatSFXHandle handle, GameObject audioObject)
    {
        StringBuilder eventBuilder = new StringBuilder("knight_");
        eventBuilder.Append(HandleToEventString(handle));

        AkSoundEngine.PostEvent(eventBuilder.ToString(), audioObject);
    }

    public void PlayKnightCombatVoiceSound(KnightCombatVoiceHandle handle, GameObject audioObject)
    {
        StringBuilder eventBuilder = new StringBuilder("knight_");
        eventBuilder.Append(HandleToEventString(handle));

        AkSoundEngine.PostEvent(eventBuilder.ToString(), audioObject);
    }

    public void PlayGeneralMonsterSound(MonsterHandle m_handle, MonsterAudioHandle m_audioHandle, GameObject audioObject)
    {
        if (m_handle == MonsterHandle.Other)
            return; // Hack to avoid sheep and others from playing sounds and throwing exceptions.

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

    public void PlayUniqueMeleeSound(UniqueMeleeAudioHandle handle, GameObject audioObject)
    {
        StringBuilder eventBuilder = new StringBuilder("melee_");
        eventBuilder.Append(HandleToEventString(handle));

        AkSoundEngine.PostEvent(eventBuilder.ToString(), audioObject);
    }

    public void PlayUniqueRangedSound(UniqueRangedAudioHandle handle, GameObject audioObject)
    {
        StringBuilder eventBuilder = new StringBuilder("ranged_");
        eventBuilder.Append(HandleToEventString(handle));

        AkSoundEngine.PostEvent(eventBuilder.ToString(), audioObject);
    }

    public void PlayUniqueSuicideSound(UniqueSuicideAudioHandle handle, GameObject audioObject)
    {
        StringBuilder eventBuilder = new StringBuilder("suicide_");
        eventBuilder.Append(HandleToEventString(handle));

        AkSoundEngine.PostEvent(eventBuilder.ToString(), audioObject);
    }

    public void PlayCombatSound(CombatHandle handle, GameObject audioObject)
    {
        StringBuilder eventBuilder = new StringBuilder();
        eventBuilder.Append(HandleToEventString(handle));

        AkSoundEngine.PostEvent(eventBuilder.ToString(), audioObject);
    }

    public void PlaySheepSound(SheepAudioHandle handle, GameObject audioObject)
    {
        StringBuilder eventBuilder = new StringBuilder("sheep_");
        eventBuilder.Append(HandleToEventString(handle));

        AkSoundEngine.PostEvent(eventBuilder.ToString(), audioObject);
    }
    #endregion

    #region Private Helper Methods
    private string HandleToEventString(Enum handle)
    {
        var enumName = Enum.GetName(handle.GetType(), handle);
        Regex pattern = new Regex(@"([A-Z]+[a-z]*\d*)");
        MatchCollection matches = pattern.Matches(enumName);

        string[] matchesArray = new string[matches.Count];
        for (int i = 0; i < matchesArray.Length; i++) 
            matchesArray[i] = matches[i].ToString().ToLower();

        return string.Join("_", matchesArray);
    }

    /// <summary>
    /// Switches the music with a minor delay.
    /// </summary>
    /// <param name="delay">The delay (in seconds) between stopping the music and starting the next piece.</param>
    /// <param name="handle">The handle of the desired music loop.</param>
    /// <returns>Nothing.</returns>
    private IEnumerator SwitchMusic(MusicHandle handle, float delay)
    {
        AkSoundEngine.PostEvent("musicStop", gameObject);

        yield return new WaitForSeconds(delay);

        switch (handle) {
            case MusicHandle.MusicOnePlay:
                AkSoundEngine.PostEvent("music1Play", gameObject);
                CurrentlyPlaying = MusicHandle.MusicOnePlay;
                break;
            case MusicHandle.MusicQuest:
                AkSoundEngine.PostEvent("musicquest", gameObject);
                CurrentlyPlaying = MusicHandle.MusicQuest;
                break;
            case MusicHandle.MusicStop:
                Debug.Log("PLZ STOP");

                CurrentlyPlaying = MusicHandle.MusicStop;
                break;
            default:
                LogError(handle); break;
        }

    }




    private void LogError(Enum handle)
    {
        Debug.LogWarning(
            string.Format("Handle of type '{0}' has no defined sound for value '{1}'.",
            handle.GetType().Name, Enum.GetName(handle.GetType(), handle)));
    }
    #endregion
}
