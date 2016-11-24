using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class WwiseTester : MonoBehaviour {
    
    public float MusicPlayTime;
    public float SecondDelay;
    public Text CurrentEnumName;
    public Text CurrentSoundName;

    private List<Type> AvailableSoundEnums = new List<Type>() {
        typeof(UIHandle), typeof(MenuHandle), typeof(CombatHandle),
        typeof(KnightCombatVoiceHandle), typeof(KnightCombatSFXHandle),
        typeof(MonsterHandle),
        typeof(UniqueMeleeAudioHandle), typeof(UniqueRangedAudioHandle), typeof(UniqueSuicideAudioHandle),
        typeof(PrincessDialogueHandle), typeof(SwordDialogueHandle), typeof(RewardHandle),
        typeof(SheepAudioHandle)
    };

    private List<Type> AvailableMusicEnums = new List<Type>() {
        typeof(MusicHandle), typeof(AmbienceHandle)
    };

    public void StartSoundTest()
    {
        StartCoroutine(CoTestAllSFX(SecondDelay));
    }

    public void StartMusicTest()
    {
        StartCoroutine(CoTestAllMusic(MusicPlayTime));
    }

    private IEnumerator CoTestAllSFX(float secDelay)
    {
        foreach (Type T in AvailableSoundEnums) {
            CurrentEnumName.text = T.Name;
            if (T.Name != "MonsterHandle") {
                foreach (string SFXName in Enum.GetNames(T)) {
                    CurrentSoundName.text = SFXName;
                    InvokeWwiseSFXMethod(Enum.Parse(T, SFXName));
                    yield return new WaitForSeconds(secDelay);
                }
            }
            else {
                foreach (string monsterName in Enum.GetNames(T)) {
                    if (monsterName == "Other")
                        continue;

                    object m_handle = Enum.Parse(T, monsterName);
                    foreach (string actionName in Enum.GetNames(typeof(MonsterAudioHandle))) {
                        CurrentSoundName.text =
                            string.Format("{0} ({1})",
                                Enum.GetName(typeof(MonsterHandle), m_handle),
                                actionName);
                        WwiseInterface.Instance.PlayGeneralMonsterSound(
                            (MonsterHandle)m_handle,
                            (MonsterAudioHandle)Enum.Parse(typeof(MonsterAudioHandle), actionName),
                            gameObject);
                        yield return new WaitForSeconds(secDelay);
                    }
                }
            }
        }
    }

    private IEnumerator CoTestAllMusic(float secPlayTime)
    {
        foreach (Type T in AvailableMusicEnums) {
            CurrentEnumName.text = T.Name;
            foreach (string musicName in Enum.GetNames(T)) {
                CurrentSoundName.text = musicName;
                InvokeWwiseMusicMethod(Enum.Parse(T, musicName));
                if (!musicName.ToLower().Contains("stop"))
                    yield return new WaitForSeconds(secPlayTime);
                else
                    yield return new WaitForSeconds(1.0f);
            }
        }
    }

    private void InvokeWwiseSFXMethod(object handle)
    {
        var handleTypeName = handle.GetType().Name;
        switch (handleTypeName) {
            case "UIHandle":
                WwiseInterface.Instance.PlayUISound((UIHandle)handle); break;
            case "MenuHandle":
                WwiseInterface.Instance.PlayMenuSound((MenuHandle)handle); break;
            case "KnightDialogueHandle": break;
            case "PeasantDialogueHandle":
                WwiseInterface.Instance.PlayPeasantDialogue((PeasantDialogueHandle)handle); break;
            case "PrincessDialogueHandle":
                WwiseInterface.Instance.PlayPrincessDialogue((PrincessDialogueHandle)handle); break;
            case "SwordDialogueHandle":
                WwiseInterface.Instance.PlaySwordDialogue((SwordDialogueHandle)handle); break;
            case "RewardHandle":
                WwiseInterface.Instance.PlayRewardSound((RewardHandle)handle); break;
            case "CombatHandle":
                WwiseInterface.Instance.PlayCombatSound((CombatHandle)handle, gameObject); break;
            case "KnightCombatSFX":
                WwiseInterface.Instance.PlayKnightCombatSFX((KnightCombatSFXHandle)handle, gameObject); break;
            case "KnightCombatVoiceHandle":
                WwiseInterface.Instance.PlayKnightCombatVoiceSound((KnightCombatVoiceHandle)handle, gameObject); break;
            case "UniqueMeleeAudioHandle":
                WwiseInterface.Instance.PlayUniqueMeleeSound((UniqueMeleeAudioHandle)handle, gameObject); break;
            case "UniqueRangedAudioHandle":
                WwiseInterface.Instance.PlayUniqueRangedSound((UniqueRangedAudioHandle)handle, gameObject); break;
            case "UniqueSuicideAudioHandle":
                WwiseInterface.Instance.PlayUniqueSuicideSound((UniqueSuicideAudioHandle)handle, gameObject); break;
            default: break;
        }
    }

    private void InvokeWwiseMusicMethod(object handle)
    {
        var handleTypeName = handle.GetType().Name;
        switch (handleTypeName) { 
            case "MusicHandle":
                WwiseInterface.Instance.SetMusic((MusicHandle)handle); break;
            case "AmbienceHandle":
                WwiseInterface.Instance.SetAmbience((AmbienceHandle)handle); break;
            default: break;
        }
    }
}
