using System;
using UnityEngine;
using UnityEngine.Serialization;

public class SoundController : SingletonDontDestroy<SoundController>
{
    public AudioSource backgroundAudio;
    public AudioSource fxAudio;
    public SoundConfig SoundConfig => ConfigController.Sound;

    public void Start()
    {
        Setup();

        Observer.MusicChanged += OnMusicChanged;
        Observer.SoundChanged += OnSoundChanged;
    }

    private void OnMusicChanged()
    {
        backgroundAudio.mute = !Data.BgSoundState;
    }
    
    private void OnSoundChanged()
    {
        fxAudio.mute = !Data.FxSoundState;
    }
    
    public void Setup()
    {
        OnMusicChanged();
        OnSoundChanged();
    }

    public void PlayFX(SoundType soundType)
    {
        SoundData soundData = SoundConfig.GetSoundDataByType(soundType);

        if (soundData != null)
        {
            fxAudio.PlayOneShot(soundData.GetRandomAudioClip());
        }
        else
        {
            Debug.LogWarning("Can't found sound data");
        }
    }

    public void PlayBackground(SoundType soundType)
    {
        SoundData soundData = SoundConfig.GetSoundDataByType(soundType);

        if (soundData != null)
        {
            backgroundAudio.clip = soundData.GetRandomAudioClip();
            backgroundAudio.Play();
        }
        else
        {
            Debug.LogWarning("Can't found sound data");
        }
    }

    public void PauseBackground()
    {
        if (backgroundAudio)
        {
            backgroundAudio.Pause();
        }
    }
}
