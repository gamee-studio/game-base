using System;
using System.Collections.Generic;
using System.Linq;
using Pancake;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName ="SoundConfig",menuName = "ScriptableObject/SoundConfig")]
public class SoundConfig : ScriptableObject
{
    public List<SoundData> soundData;

    #if UNITY_EDITOR
    [Button]
    public void UpdateSoundData()
    {
        for (int i = 0; i < Enum.GetNames(typeof(SoundType)).Length; i++)
        {
            SoundData data = new SoundData {soundType = (SoundType) i};
            if (IsItemExistedBySoundType(data.soundType)) continue;
            soundData.Add(data);
        }

        soundData = soundData.GroupBy(elem => elem.soundType).Select(group => group.First()).ToList();
    }
    #endif

    private bool IsItemExistedBySoundType(SoundType soundType)
    {
        foreach (SoundData item in soundData)
        {
            if (item.soundType == soundType)
            {
                return true;
            }
        }

        return false;
    }

    public SoundData GetSoundDataByType(SoundType soundType)
    {
        return soundData.Find(item => item.soundType == soundType);
    }
    
}

[Serializable]
public class SoundData
{
    public SoundType soundType;
    public List<AudioClip> clips;

    public AudioClip GetRandomAudioClip()
    {
        if (clips.Count > 0)
        {
            return clips[Random.Range(0, clips.Count)];
        }

        return null;
    }
}

public enum SoundType
{
    BackgroundInGame,
    BackgroundHome,
    ClickButton,
    PurchaseFail,
    PurchaseSucceed,
    ClaimReward,
    StartLevel,
    WinLevel,
    LoseLevel,
    ShowPopupWin,
    ShowPopupLose,
    CoinMove,
}
