using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName ="SoundConfig",menuName = "ScriptableObject/SoundConfig")]
public class SoundConfig : ScriptableObject
{
    public List<SoundData> soundData;
    
    public SoundData GetSoundDataByType(string soundName)
    {
        return soundData.Find(item => item.name == soundName);
    }
    
}

[Serializable]
public class SoundData
{
    public string name;
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
