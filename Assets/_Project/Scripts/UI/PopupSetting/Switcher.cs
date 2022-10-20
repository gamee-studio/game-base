using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Switcher : MonoBehaviour
{
    [Header("Datas")]
    public SwitchState SwitchState = SwitchState.Idle;
    public bool IsOn;
    [Header("Components")]
    public SettingType SettingType;
    public Sprite On;
    public Sprite Off;
    public Image Switch;
    public Transform OffPos;
    public Transform OnPos;
    public TextMeshProUGUI SwitchText;
    [Header("Config attribute")] 
    [Range(0.1f,3f)] public float TimeSwitching = .5f;

    private void SetupData()
    {
        switch (SettingType)
        {
            case SettingType.BackgroundSound:
                IsOn = Data.BgSoundState;
                break;
            case SettingType.FxSound:
                IsOn = Data.FxSoundState;
                break;
            case SettingType.Vibration:
                IsOn = Data.VibrateState;
                break;
        }
    }
    
    private void SetupUI()
    {
        if (SwitchText) SwitchText.text = IsOn ? "On" : "Off";
        if (IsOn)
        {
            Switch.sprite = On;
        }
        else
        {
            Switch.sprite = Off;
        }
    }

    private void Setup()
    {
        SetupData();
        SetupUI();
    }
    
    private void OnEnable()
    {
        Switch.transform.position = IsOn ? OnPos.position : OffPos.position;
        Setup();
    }

    public void Switching()
    {
        if (SwitchState == SwitchState.Moving) return;
        SwitchState = SwitchState.Moving;
        if (IsOn)
        {
            Switch.transform.DOMove(OffPos.position, TimeSwitching);
        }
        else
        {
            Switch.transform.DOMove(OnPos.position, TimeSwitching);
        }
        DOTween.Sequence().AppendInterval(TimeSwitching / 2f).SetEase(Ease.Linear).AppendCallback(() =>
        {
            switch (SettingType)
            {
                case SettingType.BackgroundSound:
                    Data.BgSoundState = !IsOn;
                    break;
                case SettingType.FxSound:
                    Data.FxSoundState = !IsOn;
                    break;
                case SettingType.Vibration:
                    Data.VibrateState = !IsOn;
                    break;
            }
            Setup();
        }).OnComplete(() =>
        {
            SwitchState = SwitchState.Idle;
        });
    }
}

public enum SettingType
{
    BackgroundSound,
    FxSound,
    Vibration,
}

public enum SwitchState
{
    Idle,
    Moving,
}